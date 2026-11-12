<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder

Fluent construction API for one immutable compact or enriched CommandInstruction.

## Compact mode

The legacy constructor accepts a positive action identity. WithIdentifier, WithText, and WithValue append ordered typed arguments. Build returns a compact instruction without inventing enriched metadata.

## Enriched mode

The enriched constructor requires command GUID, positive action identity, target, scope, idempotency identity, and expected-outcome contract. It supports the same typed argument methods plus WithPrecondition(grammarVersion, expression).

WithPrecondition is available only in enriched mode. The builder validates bounds and duplicate parameter identities; it does not resolve meanings, parse or evaluate grammar, authorize, or execute.

The builder is mutable, not thread-safe, and Build creates an immutable snapshot.

Source: [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs). Related: CommandInstruction, CommandPrecondition, CommandOutcomeContract.
---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
