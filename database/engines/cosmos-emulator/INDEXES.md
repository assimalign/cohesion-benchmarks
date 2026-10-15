# Unranked marketplace emulator

Gateway mode, bulk execution enabled, one container per collection, partition key `/id`.
The emulator's self-signed certificate is accepted only by this emulator target's client.
The original marketplace key is retained as `key`; `id` is its reversible base64url encoding
because Cosmos name-based item addressing cannot safely use `/` in an item id.

Indexes are disabled for ingestion, then replaced with consistent explicit paths. Orders:
customer/id, placedAt, status and composites (customer/id ASC, placedAt DESC, key ASC),
(status ASC, placedAt ASC, key ASC), (placedAt ASC|DESC, key ASC). Products: category,
variants/[]/price, name and (name ASC, key ASC). Reviews: productId, createdAt and their
composite. All containers explicitly index key and retain the built-in identity index; an
explicit `/id/?` path is rejected by this emulator with HTTP 400 / 0x800A0B00. Default wildcard indexing is excluded.

The product category and multivalue price indexes are separate: a Cosmos composite index
cannot include an array path. This is an explicit index-equivalence limitation. Write
sandbox containers use equivalent policies. The smoke record states the scale actually
attempted; the adapter never silently truncates the supplied manifest. No emulator timing is ranked.
