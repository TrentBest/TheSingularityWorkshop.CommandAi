<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposal

## Purpose

Versioned collection of one or more proposed instructions.

## Why this type exists

CommandAi separates a machine-readable proposal from the application policy that may eventually accept it. This type is a source-level part of that representation; it grants no authority by itself.

## Declaration and ownership

- **Declaration:** public sealed record.
- **Source:** [CommandProposal.cs](../../CommandProposal.cs).
- **Source-supported consumers/related types:** CommandAiTextCodec.cs; examples/CommandAi.QuickStart/Program.cs; tests/CommandAi.Tests/CommandAiTextCodecTests.cs.
- **Responsibilities:** Owns the protocol version and non-empty ordered command collection. Construction rejects unsupported versions, null collections, empty collections, and null entries. A proposal is a batch, not a transaction; the type does not imply that all instructions execute or succeed together.
- **Non-goals:** It does not validate the meaning of identifiers, authorize a command, or execute instructions.

## Contract notes

The current package uses opaque unsigned 64-bit action, parameter, and value identifiers. Literal strings remain explicitly typed and are JSON-quoted by the text codec. Identifiers are not globally meaningful unless the consuming host resolves them against the intended trusted vocabulary.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The current quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs). These pages are human-authored and source-checked; they are not generated from reflection or a compiler AST.

## Related documentation

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Architecture](../ARCHITECTURE.md)
- [Theory and wire format](../THEORY.md)
- [Companion UML and use cases](../UML/CommandProposalUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
