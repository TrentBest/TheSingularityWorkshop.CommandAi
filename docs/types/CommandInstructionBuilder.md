<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder

## Purpose

Build one generic command instruction from an opaque action identity and ordered parameter/value pairs.

## Declaration and ownership

- **Declaration:** public sealed class `CommandInstructionBuilder`.
- **Source:** [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs).
- **Related types:** CommandArgument; CommandValue; CommandInstruction; CommandLimits.
- **Responsibility:** Provides `WithIdentifier`, `WithText`, and `WithValue` methods and creates an immutable instruction snapshot.
- **Non-goals:** It does not resolve action or parameter identities or assign meaning to values.

## Contract and use

Action and parameter identifiers must be positive. Parameter identities cannot repeat within one instruction. The builder enforces `CommandLimits.MaxArgumentsPerCommand`; the resulting `CommandInstruction` performs the same validation again when built.

## Verification

Relevant tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs), including value preservation and duplicate parameter rejection.

## Related documentation

- [Builder integration boundary](../BUILDER_INTEGRATION.md)
- [CommandInstruction UML](../UML/CommandInstructionBuilderUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
