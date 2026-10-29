<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandArgument

## Purpose

One parameter identity paired with a typed value.

## Why this type exists

CommandAi separates a machine-readable proposal from the application policy that may eventually accept it. This type is a source-level part of that representation; it grants no authority by itself.

## Declaration and ownership

- **Declaration:** public sealed record.
- **Source:** [CommandArgument.cs](../../CommandArgument.cs).
- **Source-supported consumers/related types:** CommandInstruction.cs; CommandAiTextCodec.cs.
- **Responsibilities:** Owns a positive parameter identity and a non-null CommandValue.
- **Non-goals:** It does not define the parameter's meaning or application-specific value type.

## Contract notes

The current package uses opaque unsigned 64-bit action, parameter, and value identifiers. Literal strings remain explicitly typed and are JSON-quoted by the text codec. Identifiers are not globally meaningful unless the consuming host resolves them against the intended trusted vocabulary.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The current quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs). These pages are human-authored and source-checked; they are not generated from reflection or a compiler AST.

## Related documentation

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Architecture](../ARCHITECTURE.md)
- [Theory and wire format](../THEORY.md)
- [Companion UML and use cases](../UML/CommandArgumentUML.md)
