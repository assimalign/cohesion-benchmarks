# OpenApi benchmarks

36 cases cover all eight packages: the root document model through construction and document pipelines;
Attributes mapping; Fluent construction; Generation from schema/endpoint metadata; JSON and YAML
Serialization; default and official-schema Validation; Versioning transforms; and Integration import
and retargeted export. The suite adds 28 cases to the original four JSON cases, preserving their names.

Four further pointer-traversal cases cover nested item depths 1/16 and same-version copy controls.
Measurements are published through the nightly workflow artifacts and dashboard; see the
[repository guide](../../../README.md).

Microsoft.OpenApi is the baseline for JSON/YAML parsing, integration import, writing and common document construction.
Microsoft.OpenApi.YamlReader 3.10.2 adds YAML ingestion at the existing Microsoft.OpenApi version
([package and license](https://www.nuget.org/packages/Microsoft.OpenApi.YamlReader/3.10.2)).
Parsing comparisons disable Microsoft's semantic validation so both sides perform syntax-to-model work;
this also corrects the existing JSON baseline's settings without renaming its history series.
Construction produces the same title, version, paths, operations, parameters, integer schemas and responses.
No comparison is claimed for attribute metadata, generation, validation or diagnostic-producing transforms:
Microsoft's contracts and rule coverage differ.

The existing attributed Tic Tac Toe JSON corpus is reused throughout. YAML is derived in setup; no corpus
files were added. See [corpus attribution](benchmarks/Assimalign.Cohesion.OpenApi.Benchmarks/src/OpenApi.Serialization/NOTICE.md).

Not covered: stream parsing/writing, large or adversarial corpora, HTTP hosting, endpoint-source provider
composition, every optional specification field, and custom validation rules. The pinned Generation API
builds schemas from explicit metadata; it does not expose arbitrary CLR-type schema inference. Versioning
exposes document transformation rather than request version negotiation. Root value-node constructors
and capability lookups are intentionally not isolated as trivial benchmarks.

Run `pwsh ./scripts/Invoke-Benchmarks.ps1 -Area OpenApi -Profile dry` from the repository root.
