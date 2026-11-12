<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder — UML neighborhood and use cases

~~~mermaid
classDiagram
 class CommandInstructionBuilder {
  -ulong actionId
  -Guid? commandId
  -CommandValue? target
  -CommandValue? scope
  -string? idempotencyKey
  -CommandOutcomeContract? outcomeContract
  -List~CommandArgument~ arguments
  -List~CommandPrecondition~ preconditions
  +WithIdentifier(ulong, ulong)
  +WithText(ulong, string)
  +WithValue(ulong, CommandValue)
  +WithPrecondition(string, string)
  +Build() CommandInstruction
 }
 CommandInstructionBuilder --> CommandInstruction : immutable compact/enriched snapshot
~~~

## Use cases

- Build a compact command from action identity and typed arguments.
- Build an enriched command with command identity, target, scope, idempotency identity, and outcome contract.
- Add grammar-versioned opaque preconditions only in enriched mode.
- Build a validated immutable instruction.

The builder does not resolve meanings, parse or evaluate grammar, authorize, or execute. It is mutable and not thread-safe; Build creates an immutable snapshot.

Source: [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs). Tests: [CommandAiAtomicCommandTests.cs](../../tests/CommandAi.Tests/CommandAiAtomicCommandTests.cs).
---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
