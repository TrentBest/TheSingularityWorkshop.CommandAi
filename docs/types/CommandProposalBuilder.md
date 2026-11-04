<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposalBuilder

Fluent construction API for an immutable CommandProposal containing compact or enriched instructions.

## Contract

- AddCommand(actionId, configure) appends a compact legacy instruction.
- AddCommand(commandId, actionId, target, scope, idempotencyKey, outcomeContract, configure) appends a fully specified enriched atomic command.
- Both overloads preserve insertion order.
- Build returns a proposal snapshot and enforces non-empty and configured proposal limits.
- The proposal model rejects duplicate enriched command IDs and idempotency identities.

The builder does not resolve identifiers, validate host-domain meaning, authorize, or execute.

Source: [CommandProposalBuilder.cs](../../CommandProposalBuilder.cs). Related: CommandInstructionBuilder, CommandProposal.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
