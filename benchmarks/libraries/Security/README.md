# Security benchmarks

32 cases across both packages (24 added to the original eight). Existing benchmark names and payload sizes are preserved.

| Package | Cases | Work measured |
| --- | ---: | --- |
| Security | 12 | PKCS#12 bytes, PKCS#12 files and PEM files, each with/without a private key, against the BCL certificate loaders |
| Security.DataProtection | 20 | Protect, unprotect and round trips at 64/1024 bytes; purpose-chain construction and dynamically scoped round trips at depth 1/4 |

ASP.NET Core Data Protection remains the authenticated, purpose-isolated protection counterpart. Both use warmed in-memory key rings and their default algorithms: Cohesion AES-256-GCM with HKDF-SHA256, ASP.NET Core AES-256-CBC with HMAC-SHA256. Each unprotect reads its own stack's ciphertext; setup verifies recovery of the original bytes, including across separately constructed purpose chains.

HKDF runs on every Cohesion protect/unprotect call, so those measurements include key derivation and subkey zeroing. There is no standalone public derivation API. A raw BCL HKDF or AES-GCM call omits the key-ring lookup, purpose binding and authenticated envelope; it would not be a fair baseline for IDataProtector and is not presented as one.

The certificate counterparts are the same System.Security.Cryptography.X509Certificates loaders wrapped by the public certificate manager, using identical inputs. Both sides include certificate disposal. Certificates and keys are generated in GlobalSetup; files live only in a unique temporary directory and are cleaned up. File measurements include warm filesystem reads, not cold-storage latency. No certificate-store lookup, network retrieval, key rotation, ring persistence/reload, malformed-ciphertext failure or concurrent access is measured. No new NuGet counterpart is needed.
