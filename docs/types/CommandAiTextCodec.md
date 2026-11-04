<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec

Encoder and decoder for compact and enriched entries within the CommandAi/1 envelope.

## Contract

- Encode(proposal) preserves proposal order and writes the CommandAi/1 header.
- Legacy compact instructions use the original Command= action/argument line unchanged.
- Enriched atomic commands use AtomicCommand= followed by a JSON object.
- Decode(text) reads compact, enriched, or mixed proposals.
- Encoding emits LF line endings; decoding accepts LF or CRLF.
- Enriched JSON is limited to depth 16, rejects unknown members, and rejects duplicate properties.
- Unknown envelope entries, malformed values, unsupported versions, duplicate protocol declarations, duplicate parameter identities, duplicate enriched command identities/idempotency identities, and resource-limit violations are rejected.
- The decoder does not reinterpret missing legacy fields as defaults.

Older compact-only readers reject the new AtomicCommand entry. Consumers must upgrade before accepting enriched proposals; keeping the protocol identifier at /1 does not promise universal forward compatibility.

Empty caller input is an argument error. Malformed non-empty wire input is a format error. Parsing does not authorize, evaluate, or execute.

Source: [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs). Tests: [CommandAiAtomicCommandTests.cs](../../tests/CommandAi.Tests/CommandAiAtomicCommandTests.cs) and [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
