# Content benchmarks

The suite has 52 cases: the original seven plus 45 added cases. A case is one benchmark method
for one parameter combination. Existing benchmark identities are preserved.

| Package (without `Assimalign.Cohesion.`) | Added cases | Work measured |
| --- | ---: | --- |
| Content | 1 | Enumerate a composite and drain its eight content parts |
| Content.Binary | 4 | Checked packed-record reads and writes, 8/128 records |
| Content.Bmff | 5 | Recursive header cursor with lazy leaf skipping; saving 1/16 containers with zero/nonzero prefixes |
| Content.Ebml | 2 | Schema-aware typed element walking and document loading |
| Content.Exe | 3 | Detect and inspect PE64, ELF64 and Mach-O64, including native tables |
| Content.Heif | 2 | HEIC/AVIF item locations and image-property associations |
| Content.Markdown | 8 | Inline/block parsing and rendering pre-parsed trees |
| Content.Media | 3 | Repeated sample, timestamp and safe seek-range queries through MP4 |
| Content.Mkv | 5 | Xiph/fixed/EBML lacing, track loading and cluster sample scanning |
| Content.Mpeg | 1 | Progressive MP4 track metadata and lazy index creation |
| Content.Pdf | 4 | Classic-xref loading and inherited page geometry, 1/8 pages |
| Content.Text | 3 | Encoding detection plus decoding, mixed-ending lines and delimiter tokens |
| Content.Yaml | 4 | Materialized syntax events for flow collections and block scalars |

Inputs are deterministic and generated in global setup. Media payloads are tiny placeholder bytes,
not playable bitstreams. No downloaded corpus, checked-in binary fixtures or temporary files are needed.
The existing BMFF loading benchmark retains its original generated 1 MiB lazy payload.

Measurements are published through the nightly workflow artifacts and dashboard; see the
[repository guide](../../../README.md).

The existing Markdig and YamlDotNet dependencies also baseline the added Markdown and YAML cases.
Markdown setup verifies identical HTML; rendering reuses trees parsed in setup. The YAML baseline
collects its streaming events into a list, matching Cohesion's materialized event API, and setup checks
equal event counts. These cases compare syntax/document processing, not object deserialization.

PdfPig 0.1.16 (Apache-2.0) is the only new counterpart dependency. Both readers open the same valid
classic-xref PDF, resolve inherited geometry for all pages, and return the same dimension sum, checked
in setup. PdfPig uses strict parsing. Empty pages avoid content operators and text extraction, which
Cohesion does not implement. This is a structural/page-tree comparison on these inputs, not a claim
that the libraries support identical PDF features or reject every malformed input identically.

MetadataExtractor was considered but not added: EXIF/tag extraction does different work from an
editable BMFF/HEIF item tree or a native media sample index. Generic unchecked binary primitives,
string splitting and format-specific executable libraries also do not provide equivalent contracts
for the standalone cases. No artificial EBML/MKV/MPEG/Exe baseline is used. Content.Media's shared
contracts are exercised through real MP4 queries and MKV enumeration rather than a synthetic index.

Coverage still excludes fragmented MP4/CMAF, MP4 fast-start relocation, MKV cue-based seeking and
metadata edits, HEIF derived images and EXIF/XMP edits, EBML CRC verification, executable universal
Mach-O files and large native tables, PDF xref/object streams, stream decoding and incremental saves,
segmented text sequences, and Markdown/YAML emission. There is no image/media decoding, PDF rendering
or process invocation benchmark. The fixtures focus on successful parsing; malformed-input/resource-limit
paths and broad real-world format corpora remain unmeasured.

Verification:

```powershell
dotnet build benchmarks/libraries/Content/benchmarks/Assimalign.Cohesion.Content.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area Content -Profile dry
dotnet build Assimalign.Cohesion.Benchmarks.slnx -c Release -nr:false
```

Both Release builds completed with zero warnings/errors; the dry profile executed all 48 cases.
