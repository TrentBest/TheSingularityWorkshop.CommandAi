<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposal

## Purpose

A versioned ordered collection of one or more complete atomic command instructions.

## Public contract

- CurrentProtocolVersion is CommandAi/2.
- Commands must be non-empty, bounded, and non-null.
- Command identities must be unique within the proposal.
- Idempotency keys must be unique within the proposal.
- Aggregate literal/precondition/idempotency text is bounded.
- A proposal is an ordered batch, not a transaction; it does not imply that all commands execute or succeed together.

## Ownership

- **Declaration:** public sealed record.
- **Source:** [CommandProposal.cs](../../CommandProposal.cs).
- **Related types:** CommandInstruction, CommandLimits, CommandAiTextCodec.
- **Non-goals:** resolving identifiers, evaluating preconditions, authorizing, executing, or enforcing idempotency across proposals or processes.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs). These pages are human-authored and source-checked.

## Related documentation

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Architecture](../ARCHITECTURE.md)
- [Theory and wire format](../THEORY.md)
- [Companion UML](../UML/CommandProposalUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
