<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec

## Purpose

Deterministic encoder and strict decoder for the line-oriented CommandAi/2 envelope.

## Contract

- Encodes a CommandProposal header and one JSON object per Command= line.
- Carries all command identity, action, target, scope, arguments, preconditions, idempotency, and outcome-contract fields.
- Uses explicit typed-value discriminators and camel-case JSON property names.
- Rejects unsupported versions, unknown JSON properties, malformed JSON, missing required values, invalid value-kind combinations, duplicates, and resource-limit violations.
- Does not parse/evaluate preconditions, resolve domain identities, authorize, or execute.

## Ownership

Source: [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs). Related model: [CommandProposal](CommandProposal.md) and [CommandInstruction](CommandInstruction.md).

CommandAi/1 is intentionally not accepted. The package was not externally released and CommandAi/2 is the corrected breaking contract.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
