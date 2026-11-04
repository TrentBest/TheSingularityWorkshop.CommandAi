<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec — UML neighborhood and use cases

~~~mermaid
flowchart TD
  I[Untrusted CommandAi/1 envelope] --> C[CommandAiTextCodec]
  C --> P[CommandProposal]
  P --> L[Legacy Command action and arguments]
  P --> A[Enriched AtomicCommand JSON]
  L --> M[Action identity and ordered typed arguments]
  A --> X[Command identity, target, scope, preconditions, idempotency, outcome contract]
  P -.-> H[Host validation and authorization remain external]
  X -.-> G[GrammarAi validates precondition structure]
  G -.-> H
~~~

## Contract

The codec reads and writes compact and enriched entries within the same CommandAi/1 envelope. Compact Command entries preserve their original serialized form. Enriched AtomicCommand entries use bounded JSON with depth 16, strict unknown-member rejection, and duplicate-property rejection. Proposals may mix both forms while preserving command order.

Encoding uses LF line separators. Decoding accepts LF and CRLF, removes empty lines, and trims remaining lines. Whitespace-only lines are ignored as entries but still count toward the maximum-line guard.

Older compact-only readers reject AtomicCommand as an unknown envelope entry. Consumers must upgrade before accepting enriched proposals. Parsing does not authorize, evaluate preconditions, guarantee replay, or execute.

## Use cases

- Decode an untrusted compact or enriched envelope into bounded immutable data for host validation.
- Encode a constructed compact, enriched, or mixed proposal.
- Reject malformed envelopes, unsupported versions, duplicate declarations/identities/properties, invalid values, and resource-limit violations.

Source: [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs). Tests: [CommandAiAtomicCommandTests.cs](../../tests/CommandAi.Tests/CommandAiAtomicCommandTests.cs) and [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
