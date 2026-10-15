using System;
using System.Collections.Generic;
using System.Globalization;

namespace Assimalign.Cohesion.Benchmarks.Database.Graph;

/// <summary>Exact Cypher and GQL projections, predicates, total orders, and reversible write fixtures.</summary>
public static class GraphQueries
{
    public static GraphParameters Create(string operation, long person, long other, long post, long tag, string name, long inserted)
    {
        long date = checked(1_900_000_000_000L + inserted);
        string content = "Benchmark 'post' \\ " + inserted.ToString(CultureInfo.InvariantCulture);
        var values = new Dictionary<string, object?> { ["person"] = person, ["other"] = other, ["post"] = post,
            ["tag"] = tag, ["name"] = name, ["id"] = inserted, ["date"] = date, ["content"] = content, ["length"] = (long)content.Length };
        string p = "(p:Person {id:" + GraphMapping.Literal(person) + "})";
        const string messageProjection = "CASE WHEN m:Post THEN 'Post' ELSE 'Comment' END AS kind, m.id AS id, m.creationDate AS created, a.id AS author, m.content AS content";
        const string messageOrder = " ORDER BY created DESC, id DESC, kind ASC";
        const string profile = "p.id,p.firstName,p.lastName,p.gender,p.birthday,p.creationDate,p.locationIP,p.browserUsed,p.email,p.speaks,c.id,c.name";
        string cypher;
        string[] gql;
        bool paths = false;
        string? cleanupCypher = null, cleanupGql = null;
        switch (operation)
        {
            case "graph.person-profile":
                cypher = "MATCH (p:Person {id:$person})-[:IS_LOCATED_IN]->(c:Place {type:'City'}) RETURN " + profile;
                gql = ["MATCH " + p + "-[:IS_LOCATED_IN]->(c:Place {type:'City'}) RETURN " + profile]; break;
            case "graph.person-recent-messages":
                cypher = "MATCH (a:Person {id:$person})<-[:HAS_CREATOR]-(m:Post|Comment) RETURN " + messageProjection + messageOrder + " LIMIT 10";
                gql = ["MATCH route=" + p + "<-[:HAS_CREATOR]-(m:Post|Comment) RETURN route"]; paths = true; break;
            case "graph.person-friends":
                cypher = "MATCH (p:Person {id:$person})-[r:KNOWS]-(f:Person) RETURN f.id AS id,f.firstName,f.lastName,r.creationDate AS created ORDER BY created DESC,id ASC";
                gql = ["MATCH " + p + "-[r:KNOWS]-(f:Person) RETURN f.id,f.firstName,f.lastName,r.creationDate"]; break;
            case "graph.friends-recent-posts":
                cypher = "MATCH (p:Person {id:$person})-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post) RETURN DISTINCT " + messageProjection + messageOrder + " LIMIT 20";
                gql = ["MATCH route=" + p + "-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post) RETURN route"]; paths = true; break;
            case "graph.fof-recent-messages":
                cypher = "MATCH (p:Person {id:$person})-[:KNOWS*1..2]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN DISTINCT " + messageProjection + messageOrder + " LIMIT 20";
                gql = ["MATCH route=" + p + "-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN route",
                    "MATCH route=" + p + "-[:KNOWS]-(:Person)-[:KNOWS]-(a:Person)<-[:HAS_CREATOR]-(m:Post|Comment) WHERE a.id <> p.id RETURN route"];
                paths = true; break;
            case "graph.friends-within-3-by-name":
                cypher = "MATCH path=(p:Person {id:$person})-[:KNOWS*1..3]-(f:Person {firstName:$name}) WHERE f.id <> p.id WITH f,min(length(path)) AS distance RETURN f.id AS id,f.firstName,f.lastName,distance ORDER BY distance,id LIMIT 20";
                gql = new string[3];
                for (int hops = 1; hops <= 3; hops++)
                {
                    string pattern = p;
                    for (int i = 1; i < hops; i++) pattern += "-[:KNOWS]-(:Person)";
                    gql[hops - 1] = "MATCH " + pattern + "-[:KNOWS]-(f:Person {firstName:" + GraphMapping.Literal(name) + "}) WHERE f.id <> p.id RETURN f.id,f.firstName,f.lastName";
                }
                break;
            case "graph.shortest-path-knows":
                cypher = "MATCH (a:Person {id:$person}),(b:Person {id:$other}) OPTIONAL MATCH path=shortestPath((a)-[:KNOWS*]-(b)) RETURN length(path)";
                gql = ["MATCH ANY SHORTEST " + p + "-[:KNOWS]-(b:Person {id:" + GraphMapping.Literal(other) + "}) RETURN p"]; break;
            case "graph.friend-recommendation":
                cypher = "MATCH (p:Person {id:$person})-[:KNOWS]-(f:Person)-[:KNOWS]-(c:Person) WHERE c.id <> p.id AND NOT EXISTS { MATCH (p)-[:KNOWS]-(c) } WITH c,count(DISTINCT f) AS mutual RETURN c.id AS id,c.firstName,c.lastName,mutual ORDER BY mutual DESC,id LIMIT 10";
                gql = ["MATCH " + p + "-[:KNOWS]-(f:Person)-[:KNOWS]-(c:Person) WHERE c.id <> p.id AND NOT EXISTS { MATCH (p)-[:KNOWS]-(c) } RETURN c.id,f.id"]; break;
            case "graph.tag-cooccurrence":
                cypher = "MATCH (p:Person {id:$person})-[:KNOWS]-(f:Person)<-[:HAS_CREATOR]-(m:Post)-[:HAS_TAG]->(t:Tag {id:$tag}), (m)-[:HAS_TAG]->(u:Tag) WHERE u.id <> t.id WITH u,count(DISTINCT m) AS frequency RETURN u.id AS id,u.name,frequency ORDER BY frequency DESC,id";
                gql = ["MATCH " + p + "-[:KNOWS]-(f:Person)<-[:HAS_CREATOR]-(m:Post)-[:HAS_TAG]->(t:Tag {id:" + GraphMapping.Literal(tag) + "}), (m)-[:HAS_TAG]->(u:Tag) WHERE u.id <> t.id RETURN u.id,u.name,m.id"]; break;
            case "graph.country-person-count":
                cypher = "MATCH (p:Person)-[:IS_LOCATED_IN]->(:Place {type:'City'})-[:IS_PART_OF]->(c:Place {type:'Country'}) RETURN c.id AS id,c.name,count(DISTINCT p) AS persons ORDER BY id";
                gql = ["MATCH (p:Person)-[:IS_LOCATED_IN]->(:Place {type:'City'})-[:IS_PART_OF]->(c:Place {type:'Country'}) RETURN c.id,c.name,p.id"]; break;
            case "graph.add-post":
                cypher = "MATCH (a:Person {id:$person}),(t:Tag {id:$tag}) CREATE (m:Post {id:$id,creationDate:$date,content:$content,locationIP:'127.0.0.1',browserUsed:'dbbench',language:'en',length:$length})-[:HAS_CREATOR]->(a),(m)-[:HAS_TAG]->(t) RETURN m.id,m.creationDate,a.id,t.id";
                gql = ["MATCH (a:Person {id:" + GraphMapping.Literal(person) + "}),(t:Tag {id:" + GraphMapping.Literal(tag) + "}) INSERT (m:Post " + GraphMapping.Properties(new Dictionary<string, object?> { ["id"] = inserted, ["creationDate"] = date, ["content"] = content,
                    ["locationIP"] = "127.0.0.1", ["browserUsed"] = "dbbench", ["language"] = "en", ["length"] = (long)content.Length }) + ")-[:HAS_CREATOR]->(a),(m)-[:HAS_TAG]->(t) RETURN m.id,m.creationDate,a.id,t.id"];
                cleanupCypher = "MATCH (m:Post {id:$id}) DETACH DELETE m";
                cleanupGql = "MATCH (m:Post {id:" + GraphMapping.Literal(inserted) + "}) DETACH DELETE m"; break;
            case "graph.add-like":
            case "graph.add-friendship":
                string type = operation == "graph.add-like" ? "LIKES" : "KNOWS";
                string label = type == "LIKES" ? "Post" : "Person";
                string targetParameter = type == "LIKES" ? "$post" : "$other";
                string targetValue = GraphMapping.Literal(type == "LIKES" ? post : other);
                cypher = $"MATCH (a:Person {{id:$person}}),(b:{label} {{id:{targetParameter}}}) CREATE (a)-[r:{type} {{creationDate:$date}}]->(b) RETURN a.id,b.id,r.creationDate";
                gql = [$"MATCH (a:Person {{id:{GraphMapping.Literal(person)}}}),(b:{label} {{id:{targetValue}}}) INSERT (a)-[r:{type} {{creationDate:{GraphMapping.Literal(date)}}}]->(b) RETURN a.id,b.id,r.creationDate"];
                cleanupCypher = $"MATCH (a:Person {{id:$person}})-[r:{type} {{creationDate:$date}}]->(b:{label} {{id:{targetParameter}}}) DELETE r";
                cleanupGql = $"MATCH (a:Person {{id:{GraphMapping.Literal(person)}}})-[r:{type} {{creationDate:{GraphMapping.Literal(date)}}}]->(b:{label} {{id:{targetValue}}}) DELETE r"; break;
            default: throw new ArgumentException("Unknown graph operation: " + operation, nameof(operation));
        }
        return new(cypher, values, gql, paths, cleanupCypher, cleanupGql);
    }
}
