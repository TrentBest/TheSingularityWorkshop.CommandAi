<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstruction

An immutable command proposal with an action identity and ordered typed arguments. It can use either the original compact /1 shape or the enriched atomic-command shape.

## Compact contract

The legacy constructor accepts a positive ActionId and optional ordered arguments. CommandId, Target, Scope, IdempotencyKey, and OutcomeContract remain absent. Preconditions is empty. HasAtomicContract is false. The model does not invent defaults for fields that were not present in a legacy proposal.

## Enriched atomic-command contract

The enriched constructor additionally requires:

- CommandId: non-empty GUID identifying this proposed command, distinct from action identity.
- ActionId: positive application-owned action identity.
- Target and Scope: non-null typed opaque values.
- IdempotencyKey: non-empty bounded caller-supplied identity; it is not a replay guarantee.
- OutcomeContract: a non-null declaration of the expected outcome shape.
- Arguments: ordered typed values with unique positive parameter identities.
- Preconditions: optional bounded expressions paired with grammar-version identifiers.

HasAtomicContract is true only for a command created with the complete enriched constructor. A proposal may contain compact and enriched instructions together.

Source: [CommandInstruction.cs](../../CommandInstruction.cs). Related: CommandArgument, CommandValue, CommandPrecondition, CommandOutcomeContract, CommandProposal.
---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
