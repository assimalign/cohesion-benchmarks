# Marketplace indexes

All collections use simple (binary, case-sensitive) collation. The `_id` value is the original
marketplace key, also retained in `id`. Every insert/update/replace uses `w:1,j:true`.

After unordered batch ingestion: orders `{customer.id:1,placedAt:-1}`, `{status:1,placedAt:1}`,
`{placedAt:1}`; products `{category:1,variants.price:1}` (multikey), `{name:1}`;
reviews `{productId:1,createdAt:1}`. The standalone date index serves the 30-day aggregation
and is supplied to every engine. Write sandbox collections receive the same indexes.
