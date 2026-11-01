<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec

## Purpose

Encoder and decoder for the line-oriented CommandAi/1 envelope.

## Why this type exists

CommandAi separates a machine-readable proposal from the application policy that may eventually accept it. This type is a source-level part of that representation; it grants no authority by itself.

## Declaration and ownership

- **Declaration:** public static class.
- **Source:** [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs).
- **Source-supported consumers/related types:** CommandProposal.cs; examples/CommandAi.QuickStart/Program.cs; tests/CommandAi.Tests/CommandAiTextCodecTests.cs.
- **Responsibilities:** Encodes proposals and rejects malformed envelopes, unsupported/missing versions, malformed IDs, duplicate parameters, invalid values, and unknown envelope entries. Non-empty malformed wire input is normalized to FormatException, including constructor-level proposal/argument validation failures. Null, empty, or whitespace-only caller input throws ArgumentException.
- **Non-goals:** It does not parse arbitrary natural language, authenticate a sender, authorize a proposal, or execute commands.

## Contract notes

The current package uses opaque unsigned 64-bit action, parameter, and value identifiers. Literal strings remain explicitly typed and are JSON-quoted by the text codec. Identifiers are not globally meaningful unless the consuming host resolves them against the intended trusted vocabulary.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The current quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs). These pages are human-authored and source-checked; they are not generated from reflection or a compiler AST.

## Related documentation

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Architecture](../ARCHITECTURE.md)
- [Theory and wire format](../THEORY.md)
- [Companion UML and use cases](../UML/CommandAiTextCodecUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
