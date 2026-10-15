using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

internal sealed class SocialValues
{
    internal readonly ulong Seed;
    internal readonly long Persons,Forums,Posts,Comments,Likes;
    internal const int Tags=1000,TagClasses=20;
    internal readonly IdPermutation PersonIds;
    private readonly ZipfDistribution degrees=new(400,1.5);
    internal readonly ZipfDistribution MessagePopularity;
    internal readonly IdPermutation MessageIds;
    internal static readonly string[] Countries=BuildCountries();
    internal static readonly string[] Continents=["Africa","Asia","Europe","North America","Oceania","South America"];

    internal SocialValues(ulong seed,double scale)
    {
        Seed=seed; Persons=GenerationSession.Count(scale,30000); Forums=Math.Min(Persons,GenerationSession.Count(scale,1000));
        Posts=GenerationSession.Count(scale,1200000); Comments=GenerationSession.Count(scale,3000000); Likes=GenerationSession.Count(scale,6000000);
        PersonIds=new(Persons,seed,CounterRandom.Stream("social.person.ids"));
        MessagePopularity=new(Posts+Comments,1.08); MessageIds=new(Posts+Comments,seed,CounterRandom.Stream("social.message.popularity"));
    }
    internal EntityRandom Random(string stream,long id) => new(Seed,"social."+stream,id);
    internal long CityByRank(long rank) => 1+(rank-1)%DatasetText.Cities.Length;
    internal long PersonCity(long id) => CityByRank(PersonIds.Invert(id));
    internal string FirstName(long id) { var r=Random("person.name",id); return DatasetText.Pick(DatasetText.FirstNames,ref r); }
    internal string LastName(long id) { var r=Random("person.last",id); return DatasetText.Pick(DatasetText.LastNames,ref r); }
    internal static string TagName(long id) => DatasetText.Topics[(id-1)%DatasetText.Topics.Length]+"-"+id.ToString(CultureInfo.InvariantCulture);
    internal long PostCreatorRank(long id) => 1+(id-1)%Persons;
    internal long PersonForum(long rank) => 1+(rank-1)%Forums;
    internal long PostForum(long id) => PersonForum(PostCreatorRank(id));
    internal DateTime PostDate(long id) { var r=Random("post.date",id); return DatasetTime.Sample(ref r,100); }
    internal long CommentPost(long id) => 1+(id-1)%Posts;
    internal (bool ToComment,long Parent,long Creator,DateTime Date) Comment(long id)
    {
        long post=CommentPost(id),level=(id-1)/Posts,forum=PostForum(post);
        var r=Random("comment.identity",id);
        bool toComment=level>0 && r.Chance(0.7);
        long members=1+(Persons-forum)/Forums,rank=forum+Forums*r.Long(0,members-1);
        DateTime date=PostDate(post);
        long levels=1+(Comments-1)/Posts;
        long step=Math.Min(86400,((long)(DatasetTime.End-date).TotalSeconds-10)/(levels+1));
        date=date.AddSeconds(level*step+r.Long(1,step));
        return(toComment,toComment?id-Posts:post,PersonIds.Permute(rank),date);
    }

    // Only the smaller latent rank proposes a pair. Local and global permutations sample without replacement.
    internal long[] FriendRanks(long rank)
    {
        long remaining=Persons-rank;
        if(remaining==0) return [];
        var r=Random("friend.degree",rank);
        int count=(int)Math.Min(remaining,5+degrees.Sample(ref r));
        long local=remaining/DatasetText.Cities.Length;
        int localCount=(int)Math.Min(local,(long)Math.Ceiling(count*0.65));
        var result=new long[count];
        var seen=new HashSet<long>();
        if(localCount>0)
        {
            var permutation=new IdPermutation(local,Seed,CounterRandom.Value(Seed,13,rank));
            for(int i=0;i<localCount;i++) { long target=rank+DatasetText.Cities.Length*permutation.Permute(i+1); result[i]=target; seen.Add(target); }
        }
        var global=new IdPermutation(remaining,Seed,CounterRandom.Value(Seed,17,rank));
        int next=localCount;
        for(long j=1;next<count;j++) { long target=rank+global.Permute(j); if(seen.Add(target)) result[next++]=target; }
        return result;
    }

    internal long PlaceCount => DatasetText.Cities.Length+Countries.Length+Continents.Length;
    internal long CountryPlace(string code) => DatasetText.Cities.Length+Array.IndexOf(Countries,code)+1;
    internal static int Continent(string code) => code switch
    {
        "ZA" or "KE" or "NG" or "GH" or "EG" or "MA" or "TN"=>0,
        "JP" or "KR" or "SG" or "IN" or "TH" or "VN" or "ID" or "MY" or "TR"=>1,
        "US" or "CA" or "MX"=>3, "AU" or "NZ"=>4, "BR" or "AR" or "CL" or "CO" or "PE"=>5, _=>2
    };
    private static string[] BuildCountries() { var list=new List<string>(); foreach(City c in DatasetText.Cities) if(!list.Contains(c.CountryCode)) list.Add(c.CountryCode); return list.ToArray(); }
    internal Dictionary<string,long> MaximumIds()
    {
        var values=new Dictionary<string,long>(StringComparer.Ordinal) {
            ["place"]=PlaceCount,["organisation"]=DatasetText.Cities.Length*2,["tagclass"]=TagClasses,["tag"]=Tags,
            ["person"]=Persons,["forum"]=Forums,["post"]=Posts,["comment"]=Comments
        };
        foreach(var relation in SocialSchema.Relationships) values[relation.File]=0; // Edges have endpoint keys, no separate generated id.
        return values;
    }
}
