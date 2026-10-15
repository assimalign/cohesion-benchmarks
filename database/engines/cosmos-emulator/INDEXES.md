# Unranked marketplace emulator

Gateway mode, bulk execution enabled, one container per collection, partition key `/id`.
The emulator's self-signed certificate is accepted only by this emulator target's client.
The original marketplace key is retained as `key`; `id` is its reversible base64url encoding
because Cosmos name-based item addressing cannot safely use `/` in an item id.

Indexes are disabled for ingestion, then replaced with consistent explicit paths. Orders:
customer/id, placedAt, status and composites (customer/id ASC, placedAt DESC, key ASC),
(status ASC, placedAt ASC, key ASC), (placedAt ASC|DESC, key ASC). Products: category,
variants/[]/price, name and (name ASC, key ASC). Reviews: productId, createdAt and their
composite. All containers index id and key. Default wildcard indexing is excluded.

The product category and multivalue price indexes are separate: a Cosmos composite index
cannot include an array path. This is an explicit index-equivalence limitation. Write
sandbox containers use equivalent policies. Functional scale is capped at 0.1; the smoke
record states the scale actually attempted. No emulator timing is ranked.
