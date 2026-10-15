using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assimalign.Cohesion.Benchmarks.Database.Datasets;
using Shouldly;
using Xunit;
using static Assimalign.Cohesion.Benchmarks.Database.Tests.Datasets.DatasetRun;

namespace Assimalign.Cohesion.Benchmarks.Database.Tests.Datasets;

/// <summary>Exhaustive tiny-scale graph endpoints, friendships, forum membership and reply chronology.</summary>
public sealed class SocialIntegrityTests
{
    [Fact]
    public async Task Generate_SocialPointZeroOne_SatisfiesGraphAndThreadInvariants()
    {
        using var run=await Generate(DatasetModel.Graph);
        var descriptor=new SocialDescriptor(run.Manifest);
        var nodes=new Dictionary<string,Dictionary<long,Dictionary<string,string?>>>();
        foreach(var node in SocialSchema.Nodes)
        {
            var rows=new Dictionary<long,Dictionary<string,string?>>();
            foreach(var row in run.Csv(node.File))
            {
                long id=Long(row,"id"); rows.TryAdd(id,row).ShouldBeTrue();
                foreach(var property in node.Properties) if(!property.Nullable) row[property.Name].ShouldNotBeNull();
            }
            nodes.Add(node.Label,rows);
            rows.Keys.Max().ShouldBe(run.Manifest.Tables.Single(t=>t.Name==node.File).MaximumId);
            ((long)rows.Count).ShouldBe(run.Manifest.Tables.Single(t=>t.Name==node.File).Rows);
            descriptor.MaximumIds()[node.File].ShouldBe(rows.Keys.Max());
        }
        nodes["Person"].Count.ShouldBe(300); nodes["Post"].Count.ShouldBe(12000); nodes["Comment"].Count.ShouldBe(30000);
        foreach(var (id,row) in nodes["Person"]) { row["firstName"].ShouldBe(descriptor.FirstName(id)); row["lastName"].ShouldBe(descriptor.LastName(id)); }
        foreach(var (id,row) in nodes["Tag"]) row["name"].ShouldBe(SocialDescriptor.TagName(id));
        var relations=new Dictionary<string,List<Dictionary<string,string?>>>();
        foreach(var relation in SocialSchema.Relationships)
        {
            var rows=run.Csv(relation.File).ToList(); relations.Add(relation.File,rows);
            var pairs=new HashSet<(long,long)>();
            foreach(var row in rows)
            {
                long src=Long(row,"src"),dst=Long(row,"dst");
                nodes[relation.SourceLabel].ContainsKey(src).ShouldBeTrue();
                nodes[relation.DestinationLabel].ContainsKey(dst).ShouldBeTrue();
                pairs.Add((src,dst)).ShouldBeTrue();
            }
            ((long)rows.Count).ShouldBe(run.Manifest.Tables.Single(t=>t.Name==relation.File).Rows);
            run.Manifest.Tables.Single(t=>t.Name==relation.File).MaximumId.ShouldBe(0);
        }
        var cities=relations["person_islocatedin_place"].ToDictionary(r=>Long(r,"src"),r=>Long(r,"dst"));
        foreach(var (id,city) in cities)
        {
            city.ShouldBe(descriptor.PersonCity(id));
            nodes["Place"][city]["name"].ShouldBe(descriptor.City(id).Name);
        }
        var knows=new HashSet<(long,long)>(); var knownDates=new Dictionary<(long,long),DateTime>(); var degrees=new Dictionary<long,int>(); int local=0;
        foreach(var row in relations["person_knows_person"])
        {
            long src=Long(row,"src"),dst=Long(row,"dst"); src.ShouldBeLessThan(dst); knows.Add((src,dst)).ShouldBeTrue();
            knownDates.Add((src,dst),Time(row,"creationDate"));
            Time(row,"creationDate").ShouldBe(descriptor.FriendshipDate(src,dst));
            degrees[src]=degrees.GetValueOrDefault(src)+1; degrees[dst]=degrees.GetValueOrDefault(dst)+1;
            if(cities[src]==cities[dst]) local++;
        }
        (2.0*knows.Count/descriptor.Persons).ShouldBeInRange(20,55);
        knownDates.Values.Distinct().Count().ShouldBeGreaterThan((int)(knows.Count*0.99));
        (local/(double)knows.Count).ShouldBeGreaterThan(0.03);
        degrees.Values.Max().ShouldBeGreaterThan(2*degrees.Values.OrderBy(x=>x).ElementAt(degrees.Count/2));
        var members=relations["forum_hasmember_person"].ToDictionary(r=>(Long(r,"src"),Long(r,"dst")),r=>Time(r,"joinDate"));
        var forumByPost=relations["forum_containerof_post"].ToDictionary(r=>Long(r,"dst"),r=>Long(r,"src"));
        var postCreators=relations["post_hascreator_person"].ToDictionary(r=>Long(r,"src"),r=>Long(r,"dst"));
        var commentCreators=relations["comment_hascreator_person"].ToDictionary(r=>Long(r,"src"),r=>Long(r,"dst"));
        foreach(var (id,post) in nodes["Post"])
        {
            Time(post,"creationDate").ShouldBe(descriptor.PostDate(id));
            members.ContainsKey((forumByPost[id],postCreators[id])).ShouldBeTrue();
            Time(post,"creationDate").ShouldBeGreaterThan(members[(forumByPost[id],postCreators[id])]);
            post["content"]!.Length.ShouldBeInRange(20,2000); Long(post,"length").ShouldBe(post["content"]!.Length);
        }
        var replyPost=relations["comment_replyof_post"].ToDictionary(r=>Long(r,"src"),r=>Long(r,"dst"));
        var replyComment=relations["comment_replyof_comment"].ToDictionary(r=>Long(r,"src"),r=>Long(r,"dst"));
        (replyPost.Count+replyComment.Count).ShouldBe(nodes["Comment"].Count);
        var threads=new Dictionary<long,long>();
        foreach(var (id,comment) in nodes["Comment"].OrderBy(x=>x.Key))
        {
            replyPost.ContainsKey(id).ShouldNotBe(replyComment.ContainsKey(id));
            long post;
            if(replyPost.TryGetValue(id,out post)) Time(comment,"creationDate").ShouldBeGreaterThan(Time(nodes["Post"][post],"creationDate"));
            else
            {
                long parent=replyComment[id]; parent.ShouldBeLessThan(id);
                Time(comment,"creationDate").ShouldBeGreaterThan(Time(nodes["Comment"][parent],"creationDate"));
                post=threads[parent];
            }
            threads.Add(id,post);
            post.ShouldBe(1+(id-1)%descriptor.Posts);
            members.ContainsKey((forumByPost[post],commentCreators[id])).ShouldBeTrue();
            comment["content"]!.Length.ShouldBeInRange(20,2000); Long(comment,"length").ShouldBe(comment["content"]!.Length);
        }
        int knownLikes=0,likeCount=0;
        foreach(string file in new[]{"person_likes_post","person_likes_comment"})
        {
            bool post=file=="person_likes_post";
            foreach(var row in relations[file])
            {
                long liker=Long(row,"src"),message=Long(row,"dst");
                var node=nodes[post?"Post":"Comment"][message];
                Time(row,"creationDate").ShouldBeGreaterThan(Time(node,"creationDate"));
                long creator=post?postCreators[message]:commentCreators[message];
                var pair=(Math.Min(liker,creator),Math.Max(liker,creator));
                if(knows.Contains(pair)) { knownLikes++; Time(row,"creationDate").ShouldBeGreaterThan(knownDates[pair]); }
                likeCount++;
            }
        }
        likeCount.ShouldBe(60000); (knownLikes/(double)likeCount).ShouldBeGreaterThan(0.3);
    }
}
