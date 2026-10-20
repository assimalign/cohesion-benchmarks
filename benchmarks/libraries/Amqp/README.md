# AMQP benchmarks

44 cases, all in-process with deterministic wire inputs and no broker, socket or connection:

- Existing transfer frame and sectioned message series retain their names and parameters (6 cases).
  The existing AMQPNetLite message decoder now accesses `Body` to include its deferred payload copy.
- Binary data messages at 64 and 16,384 bytes, encoding and decoding against AMQPNetLite (8 cases).
- Numeric, text/binary and nested value bodies through the public message codec, encoding and decoding
  against AMQPNetLite (12 cases). Covers primitive dispatch, short/full-width unsigned integers,
  signed integers, floats, UTF-8, binary8/binary32, lists, maps and nulls inside containers.
- Sequence bodies and delivery/message annotations plus footers, encoding and decoding against
  AMQPNetLite (8 cases).
- Flow credit, flow drain and batched accepted disposition frames, encoding and decoding (6 cases).
- Transfer fragments at 64 and 16,384 payload bytes, encoding and decoding (4 cases).

The 38 added cases measure `Assimalign.Cohesion.Amqp.Connections`. The root `Assimalign.Cohesion.Amqp`
package has no implementation at the pinned revision. The primitive codec is private; value-body
benchmarks exercise it through the public message API and include the message envelope on both sides.
They are not presented as isolated primitive codec timings.

## Comparison and validation

The existing Apache-2.0 `AMQPNetLite.Core` 2.4.11 reference is reused; no new counterpart is introduced.
Paired cases construct equivalent AMQP sections and new paired decoders consume the same wire bytes.
AMQPNetLite's `AmqpValue` and `Data` defer materialization, so value decode returns `Body` and binary
decode returns an owned payload, matching Cohesion's eager decoding. Sequence/annotation maps use
each library's corresponding collection types. Setup validates values and cross-decodes both encoders'
output, outside the measurement.

Full frame benchmarks remain unpaired: AMQPNetLite's frame codec is internal. Comparing its public
performative writer with Cohesion's full frame codec would omit frame headers and validation.

## Gaps

No connection/session/link state machine, credit scheduling, broker delivery, transport, SASL exchange,
reassembly, malformed packet rejection or maximum-frame rejection is measured. Open/attach/detach/close
frames, rejection/modified delivery states, multi-section data bodies, symbol arrays, UUIDs and timestamps
are also not covered.

Setup detected that the pinned Cohesion decoder returns compact signed int/long encodings (format
codes `0x54`/`0x55`) as `sbyte`, unlike AMQPNetLite's `int`/`long`. New paired value fixtures avoid those
encodings so the primitive CLR types agree; explicit `sbyte` and larger signed integers remain covered.
This upstream behavior is not changed here. The existing sectioned-message fixture retains its original
small application-property integer and history identity.

## Verify

```powershell
dotnet build benchmarks/libraries/Amqp/benchmarks/Assimalign.Cohesion.Amqp.Benchmarks/src -c Release -nr:false
pwsh ./scripts/Invoke-Benchmarks.ps1 -Area Amqp -Profile dry
```
