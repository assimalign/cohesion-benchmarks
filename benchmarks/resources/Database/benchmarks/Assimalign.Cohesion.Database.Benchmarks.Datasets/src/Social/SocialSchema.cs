using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>LDBC-style social files and logical labels, endpoints, properties and indexes.</summary>
public static class SocialSchema
{
    private static SchemaColumn B(string n) => new(n,LogicalType.BigInt);
    private static SchemaColumn V(string n,int length=200,bool nullable=false) => new(n,LogicalType.VarChar,nullable,length);
    private static SchemaColumn T(string n) => new(n,LogicalType.Timestamp);
    private static SchemaIndex Id(string label) => new("ux_"+label+"_id",["id"],true);
    public static IReadOnlyList<SchemaNode> Nodes { get; } = new SchemaNode[]
    {
        new("Place","place",[B("id"),V("name"),V("url",500),V("type",20)],[Id("Place")]),
        new("Organisation","organisation",[B("id"),V("type",20),V("name"),V("url",500)],[Id("Organisation")]),
        new("TagClass","tagclass",[B("id"),V("name"),V("url",500)],[Id("TagClass")]),
        new("Tag","tag",[B("id"),V("name"),V("url",500)],[Id("Tag"),new("ix_Tag_name",["name"])]),
        new("Person","person",[B("id"),V("firstName",80),V("lastName",80),V("gender",20),new("birthday",LogicalType.Date),
            T("creationDate"),V("locationIP",40),V("browserUsed",40),V("email",254),V("speaks",100)],[Id("Person"),new("ix_Person_firstName",["firstName"])]),
        new("Forum","forum",[B("id"),V("title"),T("creationDate")],[Id("Forum")]),
        new("Post","post",[B("id"),V("imageFile",500,true),T("creationDate"),V("locationIP",40),V("browserUsed",40),
            V("language",10),new("content",LogicalType.Text),new("length",LogicalType.Integer)],[Id("Post"),new("ix_Post_creationDate",["creationDate"])]),
        new("Comment","comment",[B("id"),T("creationDate"),V("locationIP",40),V("browserUsed",40),
            new("content",LogicalType.Text),new("length",LogicalType.Integer)],[Id("Comment")])
    };
    public static IReadOnlyList<SchemaRelationship> Relationships { get; } = new SchemaRelationship[]
    {
        new("IS_PART_OF","place_ispartof_place","Place","Place",[]),
        new("IS_LOCATED_IN","organisation_islocatedin_place","Organisation","Place",[]),
        new("IS_PART_OF","tagclass_ispartof_tagclass","TagClass","TagClass",[]),
        new("HAS_TYPE","tag_hastype_tagclass","Tag","TagClass",[]),
        new("IS_LOCATED_IN","person_islocatedin_place","Person","Place",[]),
        new("STUDY_AT","person_studyat_organisation","Person","Organisation",[new("year",LogicalType.Integer)]),
        new("WORK_AT","person_workat_organisation","Person","Organisation",[new("year",LogicalType.Integer)]),
        new("HAS_INTEREST","person_hasinterest_tag","Person","Tag",[]),
        new("KNOWS","person_knows_person","Person","Person",[T("creationDate")]),
        new("HAS_MODERATOR","forum_hasmoderator_person","Forum","Person",[]),
        new("HAS_MEMBER","forum_hasmember_person","Forum","Person",[T("joinDate")]),
        new("HAS_TAG","forum_hastag_tag","Forum","Tag",[]),
        new("CONTAINER_OF","forum_containerof_post","Forum","Post",[]),
        new("HAS_CREATOR","post_hascreator_person","Post","Person",[]),
        new("IS_LOCATED_IN","post_islocatedin_place","Post","Place",[]),
        new("HAS_TAG","post_hastag_tag","Post","Tag",[]),
        new("HAS_CREATOR","comment_hascreator_person","Comment","Person",[]),
        new("IS_LOCATED_IN","comment_islocatedin_place","Comment","Place",[]),
        new("REPLY_OF","comment_replyof_post","Comment","Post",[]),
        new("REPLY_OF","comment_replyof_comment","Comment","Comment",[]),
        new("HAS_TAG","comment_hastag_tag","Comment","Tag",[]),
        new("LIKES","person_likes_post","Person","Post",[T("creationDate")]),
        new("LIKES","person_likes_comment","Person","Comment",[T("creationDate")])
    };
    public static IReadOnlyList<string> LoadOrder { get; } = BuildOrder();
    private static string[] BuildOrder()
    {
        var names=new string[Nodes.Count+Relationships.Count]; int i=0;
        foreach(var n in Nodes) names[i++]=n.File;
        foreach(var r in Relationships) names[i++]=r.File;
        return names;
    }
    public static string[] Header(string file)
    {
        foreach(var n in Nodes) if(n.File==file) { var h=new string[n.Properties.Count]; for(int i=0;i<h.Length;i++) h[i]=n.Properties[i].Name; return h; }
        foreach(var r in Relationships) if(r.File==file) { var h=new string[r.Properties.Count+2]; h[0]="src";h[1]="dst"; for(int i=0;i<r.Properties.Count;i++) h[i+2]=r.Properties[i].Name; return h; }
        throw new ArgumentException("Unknown social file.",nameof(file));
    }
}
