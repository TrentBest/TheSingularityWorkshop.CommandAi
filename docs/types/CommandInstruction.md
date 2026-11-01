<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstruction

## Purpose

An immutable complete atomic command proposal. It carries identity, action, target, scope, ordered parameters, optional preconditions, idempotency identity, and an expected outcome contract.

## Public contract

- CommandId: Guid identity for this proposed instruction.
- ActionId: positive application-owned opaque action identity.
- Target and Scope: required opaque typed CommandValue instances.
- Arguments: ordered, bounded, unique parameter identities paired with typed values.
- Preconditions: bounded grammar-versioned expressions. CommandAi stores but does not parse or evaluate them.
- IdempotencyKey: required bounded key. The host persists and enforces duplicate-delivery behavior.
- OutcomeContract: required expected outcome identity and declared fields. It is not an actual execution result.

## Ownership

- **Declaration:** public sealed record.
- **Source:** [CommandInstruction.cs](../../CommandInstruction.cs).
- **Related types:** CommandArgument, CommandValue, CommandPrecondition, CommandOutcomeContract, CommandProposal.
- **Consumers:** CommandAiTextCodec, CommandInstructionBuilder, CommandProposalBuilder, quickstart, and tests.
- **Non-goals:** resolving action meaning, evaluating conditions, enforcing idempotency, granting authority, or executing.

## Verification

The focused test suite is [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). The quickstart is [Program.cs](../../examples/CommandAi.QuickStart/Program.cs). These pages are human-authored and source-checked, not generated from a compiler AST.

## Related documentation

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Architecture](../ARCHITECTURE.md)
- [Theory and wire format](../THEORY.md)
- [Companion UML](../UML/CommandInstructionUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
