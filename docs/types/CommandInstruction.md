<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstruction

An immutable atomic action declaration consisting of a positive action identity and an ordered collection of typed arguments.

## Contract

- `ActionId` is a positive unsigned integer.
- `Arguments` preserves insertion order.
- Parameter identities are positive and unique within the instruction.
- Argument count and literal values are bounded by CommandLimits.

The instruction does not carry command GUID, target/scope, preconditions, idempotency, or expected-outcome metadata in the approved CommandAi/1 model.

Source: [CommandInstruction.cs](../../CommandInstruction.cs). Related: CommandArgument, CommandValue, CommandProposal.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
