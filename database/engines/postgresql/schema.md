# Commerce schema

`SqlSchema` renders `CommerceSchema`. The adapter creates the database from template0
with UTF8 encoding and C collation/ctype; string columns also explicitly use COLLATE "C".
Primary keys exist during ingestion. All 14 secondary indexes, including the two
unique indexes, are built after ingestion, followed by ANALYZE. No foreign keys.
