<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandValueKind

## Purpose

Discriminator for the two value representations supported by CommandValue.

## Why this type exists

CommandAi separates a machine-readable proposal from the application policy that may eventually accept it. This type is a source-level part of that representation; it grants no authority by itself.

## Declaration and ownership

- **Declaration:** public enum.
- **Source:** [CommandValue.cs](../../CommandValue.cs).
- **Source-supported consumers/related types:** CommandValue.cs; CommandAiTextCodec.cs.
- **Responsibilities:** Defines IntegerIdentifier and LiteralText.
- **Non-goals:** It does not define vocabulary semantics or additional value kinds.

## Contract notes

The current package uses opaque unsigned 64-bit action, parameter, and value identifiers. Literal strings remain explicitly typed and are JSON-quoted by the text codec. Identifiers are not globally meaningful unless the consuming host resolves them against the intended trusted vocabulary.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The current quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs). These pages are human-authored and source-checked; they are not generated from reflection or a compiler AST.

## Related documentation

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Architecture](../ARCHITECTURE.md)
- [Theory and wire format](../THEORY.md)
- [Companion UML and use cases](../UML/CommandValueKindUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
