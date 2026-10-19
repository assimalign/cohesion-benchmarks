# Marketplace static indexes

One database, RavenDB.Client 7.1.12 against server 7.1. Only named `Bench/<collection>/<index>`
static indexes are queried. Orders: CustomerDate (Customer, PlacedAt, Key), StatusDate
(Status, PlacedAt, Key), Date (PlacedAt, Key). Products: CategoryPrice (Category,
Prices = variants.Select(v => v.price), Key), Name (Name, Key). Reviews: ProductDate
(Product, CreatedAt, Key). Strings other than timestamps use `FieldIndexing.Exact`.

Key is the original `id` and supplies total-order ties. Raven indexes contain fields rather
than Mongo-style B-tree column prefixes. Exact equality and exact prefix predicates prevent
RavenDB's default lowercasing. Wait for all static indexes to be non-stale after load and
request non-stale results on every query. Auto-index presence or index errors fail the wait.
Write sandbox collections in the same database have equivalent static indexes.
