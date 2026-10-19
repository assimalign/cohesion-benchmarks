using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Social graph with bounded-memory friendship construction, member-authored posts and ordered reply threads.</summary>
public sealed class SocialGenerator : IDatasetGenerator
{
    public DatasetModel Model => DatasetModel.Graph;
    public string Name => "social";

    public async ValueTask<DatasetManifest> GenerateAsync(DatasetGenerationOptions options,IProgress<DatasetProgress>? progress,CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var session=new GenerationSession(options,Model,progress);
        var values=new SocialDescriptor(options.Seed,options.Scale);
        WritePlaces(session,values);
        WriteTags(session);
        await session.Ranges(values.Persons,200000,(part,start,end)=>WritePeople(session,values,part,start,end,cancellationToken),cancellationToken);
        await session.Ranges(values.Forums,300000,(part,start,end)=>WriteForums(session,values,part,start,end,cancellationToken),cancellationToken);
        await session.Ranges(values.Posts,6000000,(part,start,end)=>WritePosts(session,values,part,start,end,cancellationToken),cancellationToken);
        await session.Ranges(values.Comments,17500000,(part,start,end)=>WriteComments(session,values,part,start,end,cancellationToken),cancellationToken);
        await session.Ranges(values.Persons,40000,(part,start,end)=>WriteLikes(session,values,part,start,end,cancellationToken),cancellationToken);
        return session.Finish(Name,SocialSchema.LoadOrder,values.MaximumIds());
    }

    private static void Check(long id,CancellationToken token) { if((id&255)==0) token.ThrowIfCancellationRequested(); }
    private static string Url(string kind,long id) => "https://social.example.test/"+kind+"/"+id.ToString(CultureInfo.InvariantCulture);
    private static string Ip(long id) => "192.0.2."+((id-1)%254+1).ToString(CultureInfo.InvariantCulture);

    private static void WritePlaces(GenerationSession session,SocialDescriptor v)
    {
        using var p=new SocialPart(session,0,"place","place_ispartof_place","organisation","organisation_islocatedin_place");
        for(int i=0;i<DatasetText.Cities.Length;i++)
        {
            long id=i+1; City city=DatasetText.Cities[i];
            p["place"].WriteRow(id,city.Name,Url("place",id),"City");
            p["place_ispartof_place"].WriteRow(id,v.CountryPlace(city.CountryCode));
            p["organisation"].WriteRow(id,"University",city.Name+" University",Url("organisation",id));
            p["organisation_islocatedin_place"].WriteRow(id,id);
            long company=id+DatasetText.Cities.Length;
            p["organisation"].WriteRow(company,"Company",DatasetText.Brands[i%DatasetText.Brands.Length]+" "+city.Name,Url("organisation",company));
            p["organisation_islocatedin_place"].WriteRow(company,id);
        }
        for(int i=0;i<SocialDescriptor.Countries.Length;i++)
        {
            long id=DatasetText.Cities.Length+i+1; string country=SocialDescriptor.Countries[i];
            p["place"].WriteRow(id,country,Url("place",id),"Country");
            p["place_ispartof_place"].WriteRow(id,DatasetText.Cities.Length+SocialDescriptor.Countries.Length+SocialDescriptor.Continent(country)+1);
        }
        for(int i=0;i<SocialDescriptor.Continents.Length;i++)
        {
            long id=DatasetText.Cities.Length+SocialDescriptor.Countries.Length+i+1;
            p["place"].WriteRow(id,SocialDescriptor.Continents[i],Url("place",id),"Continent");
        }
    }

    private static void WriteTags(GenerationSession session)
    {
        using var p=new SocialPart(session,0,"tag","tagclass","tag_hastype_tagclass","tagclass_ispartof_tagclass");
        for(int id=1;id<=SocialDescriptor.TagClasses;id++)
        {
            p["tagclass"].WriteRow(id,DatasetText.Topics[id-1]+" topics",Url("tagclass",id));
            if(id>1) p["tagclass_ispartof_tagclass"].WriteRow(id,1+(id-2)/5);
        }
        for(int id=1;id<=SocialDescriptor.Tags;id++)
        {
            p["tag"].WriteRow(id,SocialDescriptor.TagName(id),Url("tag",id));
            p["tag_hastype_tagclass"].WriteRow(id,1+(id-1)%SocialDescriptor.TagClasses);
        }
    }

    private static void WritePeople(GenerationSession session,SocialDescriptor v,int part,long start,long end,CancellationToken token)
    {
        using var p=new SocialPart(session,part,"person","person_islocatedin_place","person_studyat_organisation",
            "person_workat_organisation","person_hasinterest_tag","person_knows_person");
        for(long rank=start;rank<=end;rank++)
        {
            Check(rank,token); long id=v.PersonIds.Permute(rank),city=v.CityByRank(rank);
            var r=v.Random("person.details",id); var birth=new DateOnly(1950,1,1).AddDays(r.Int(0,20000));
            p["person"].WriteRow(id,v.FirstName(id),v.LastName(id),r.Chance(0.5)?"female":"male",birth,DatasetTime.Start,Ip(id),
                r.Chance(0.6)?"Browser A":"Browser B","person."+id+"@example.test","en");
            p["person_islocatedin_place"].WriteRow(id,city);
            if(r.Chance(0.8)) p["person_studyat_organisation"].WriteRow(id,city,Math.Min(2025,birth.Year+22));
            if(r.Chance(0.85)) p["person_workat_organisation"].WriteRow(id,city+DatasetText.Cities.Length,Math.Min(2025,birth.Year+24));
            int offset=r.Int(0,SocialDescriptor.Tags-1),interests=r.Int(1,5);
            for(int i=0;i<interests;i++) p["person_hasinterest_tag"].WriteRow(id,1+(offset+i)%SocialDescriptor.Tags);
            foreach(long friendRank in v.FriendRanks(rank))
            {
                long friend=v.PersonIds.Permute(friendRank);
                p["person_knows_person"].WriteRow(Math.Min(id,friend),Math.Max(id,friend),v.FriendshipDate(id,friend));
            }
        }
    }

    private static void WriteForums(GenerationSession session,SocialDescriptor v,int part,long start,long end,CancellationToken token)
    {
        using var p=new SocialPart(session,part,"forum","forum_hasmoderator_person","forum_hasmember_person","forum_hastag_tag");
        for(long id=start;id<=end;id++)
        {
            Check(id,token); var r=v.Random("forum",id);
            p["forum"].WriteRow(id,DatasetText.Pick(DatasetText.Topics,ref r)+" discussion "+id,DatasetTime.Start);
            p["forum_hasmoderator_person"].WriteRow(id,v.PersonIds.Permute(id));
            for(long rank=id;rank<=v.Persons;rank+=v.Forums) p["forum_hasmember_person"].WriteRow(id,v.PersonIds.Permute(rank),DatasetTime.Start.AddSeconds(1));
            int tag=r.Int(1,SocialDescriptor.Tags); p["forum_hastag_tag"].WriteRow(id,tag);
            p["forum_hastag_tag"].WriteRow(id,1+tag%SocialDescriptor.Tags);
        }
    }

    private static void WritePosts(GenerationSession session,SocialDescriptor v,int part,long start,long end,CancellationToken token)
    {
        using var p=new SocialPart(session,part,"post_hascreator_person","post_islocatedin_place","post_hastag_tag","forum_containerof_post");
        using var nodes=new SocialNodeParts(session,"post",500000);
        for(long id=start;id<=end;id++)
        {
            Check(id,token); var r=v.Random("post.text",id); long rank=v.PostCreatorRank(id);
            string content=DatasetText.Paragraph(ref r,20,2000,70,1.05);
            nodes.Writer(id).WriteRow(id,r.Chance(0.1)?Url("images",id)+".jpg":null,v.PostDate(id),Ip(id),r.Chance(0.6)?"Browser A":"Browser B","en",content,content.Length);
            p["post_hascreator_person"].WriteRow(id,v.PersonIds.Permute(rank));
            p["post_islocatedin_place"].WriteRow(id,v.CityByRank(rank));
            p["forum_containerof_post"].WriteRow(v.PostForum(id),id);
            int tag=r.Int(1,SocialDescriptor.Tags); p["post_hastag_tag"].WriteRow(id,tag);
            if(r.Chance(0.4)) p["post_hastag_tag"].WriteRow(id,1+tag%SocialDescriptor.Tags);
        }
    }

    private static void WriteComments(GenerationSession session,SocialDescriptor v,int part,long start,long end,CancellationToken token)
    {
        using var p=new SocialPart(session,part,"comment_hascreator_person","comment_islocatedin_place","comment_hastag_tag","comment_replyof_post","comment_replyof_comment");
        using var nodes=new SocialNodeParts(session,"comment",700000);
        for(long id=start;id<=end;id++)
        {
            Check(id,token); var r=v.Random("comment.text",id); var c=v.Comment(id);
            string content=DatasetText.Paragraph(ref r,20,2000,48,1.05);
            nodes.Writer(id).WriteRow(id,c.Date,Ip(id),r.Chance(0.6)?"Browser A":"Browser B",content,content.Length);
            p["comment_hascreator_person"].WriteRow(id,c.Creator);
            p["comment_islocatedin_place"].WriteRow(id,v.PersonCity(c.Creator));
            p[c.ToComment?"comment_replyof_comment":"comment_replyof_post"].WriteRow(id,c.Parent);
            if(r.Chance(0.3)) p["comment_hastag_tag"].WriteRow(id,r.Int(1,SocialDescriptor.Tags));
        }
    }

    private static void WriteLikes(GenerationSession session,SocialDescriptor v,int part,long start,long end,CancellationToken token)
    {
        using var p=new SocialPart(session,part,"person_likes_post","person_likes_comment");
        for(long rank=start;rank<=end;rank++)
        {
            Check(rank,token);
            long person=v.PersonIds.Permute(rank); var r=v.Random("likes",person); long[] friends=v.FriendRanks(rank);
            int count=(int)Math.Min(v.Posts+v.Comments,v.Likes/v.Persons+(rank<=v.Likes%v.Persons?1:0));
            var seen=new HashSet<long>();
            for(int i=0;i<count;i++)
            {
                long message; int attempts=0;
                do
                {
                    if(attempts++>=8) message=r.Long(1,v.Posts+v.Comments);
                    else if(friends.Length>0 && r.Chance(0.55))
                    {
                        long creatorRank=friends[r.Int(0,friends.Length-1)];
                        message=creatorRank+v.Persons*r.Long(0,(v.Posts-creatorRank)/v.Persons);
                    }
                    else message=v.MessageIds.Permute(v.MessagePopularity.Sample(ref r));
                } while(!seen.Add(message));
                bool post=message<=v.Posts;
                long target=post?message:message-v.Posts;
                DateTime date; long creator;
                if(post) { date=v.PostDate(target); creator=v.PersonIds.Permute(v.PostCreatorRank(target)); }
                else { var comment=v.Comment(target); date=comment.Date; creator=comment.Creator; }
                if(creator!=person)
                {
                    DateTime affinity=v.FriendshipDate(person,creator);
                    if(affinity>date) date=affinity;
                }
                date=DatasetTime.After(date,ref r,604800);
                p[post?"person_likes_post":"person_likes_comment"].WriteRow(person,target,date);
            }
        }
    }
}
