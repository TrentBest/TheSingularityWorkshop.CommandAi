<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec — UML neighborhood and use cases

~~~mermaid
flowchart TD
  I[Untrusted envelope] --> C[CommandAiTextCodec]
  C --> P[CommandProposal]
  P --> A[Complete CommandInstruction]
  A --> M[Command identity, target, scope, parameters]
  A --> R[Preconditions, idempotency, outcome contract]
  P -.-> H[Host validation and authorization remain external]
~~~

## Contract

CommandAiTextCodec serializes CommandAi/2 as a strict line-oriented envelope containing JSON command entries. The decoder rejects unsupported protocol versions, malformed JSON, unknown properties, missing fields, invalid typed-value combinations, duplicate identities, and resource-limit violations.

## Use cases

### Decode an untrusted proposal
The host receives an envelope, decodes it into immutable typed data, then performs domain validation and policy checks. Decoding alone never authorizes or executes.

### Encode a proposal for transport or inspection
The consumer builds a complete command model and encodes it deterministically. The codec does not execute or mutate anything.

## Source and evidence

Source: [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs). Focused tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). CI run [#41353191987](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41353191987) passed at commit cb17cc3; NuGet publication was skipped.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
