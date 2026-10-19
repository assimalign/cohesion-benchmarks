# Commerce schema

`SqlSchema` renders `CommerceSchema`. The adapter creates the database with
Latin1_General_100_BIN2_UTF8 collation, VARCHAR strings, BIT booleans and DATETIME2(0)
timestamps. It applies RECOVERY SIMPLE and DELAYED_DURABILITY DISABLED before loading.
Primary keys exist during ingestion; the 14 secondary indexes and UPDATE STATISTICS
follow ingestion. No foreign keys. Credentials come only from the development catalog.
