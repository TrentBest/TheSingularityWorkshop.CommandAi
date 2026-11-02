<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder

Fluent construction API for one immutable `CommandInstruction`.

## Contract

- The builder is initialized with a positive action identity.
- `WithIdentifier(parameterId, valueId)` adds a positive integer identifier value.
- `WithText(parameterId, value)` adds bounded literal text.
- `WithValue(parameterId, value)` adds a typed CommandValue.
- `Build()` validates and snapshots the ordered arguments.

The builder does not accept command GUID, target/scope, preconditions, idempotency, or expected-outcome metadata in CommandAi/1.

Source: [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs). Related: CommandInstruction, CommandArgument, CommandValue.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
