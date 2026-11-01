<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposalBuilder

## Purpose

Fluent construction of an immutable, versioned CommandAi proposal without assigning application-specific meaning to numeric identities.

## Declaration and ownership

- **Declaration:** public sealed class `CommandProposalBuilder`.
- **Source:** [CommandProposalBuilder.cs](../../CommandProposalBuilder.cs).
- **Related types:** CommandInstructionBuilder; CommandProposal; CommandLimits.
- **Responsibility:** Accumulates ordered command instructions and creates a validated proposal snapshot.
- **Non-goals:** It does not resolve IDs, validate domain semantics, authorize, approve, or execute commands.

## Contract and use

Call `AddCommand(actionId, configure)` for each command. The callback receives a scoped `CommandInstructionBuilder`. `Build` snapshots the accumulated commands; subsequent builder changes do not mutate an earlier proposal. The builder is bounded by `CommandLimits.MaxCommands`.

## Verification

Relevant tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs), including ordered construction, duplicate parameter rejection, command limits, immutable snapshots, and codec round-trip.

## Related documentation

- [Builder integration boundary](../BUILDER_INTEGRATION.md)
- [Problem domain](../PROBLEM_DOMAIN.md)
- [CommandProposal UML](../UML/CommandProposalBuilderUML.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
