<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder — UML neighborhood and use cases

~~~mermaid
classDiagram
  class CommandInstructionBuilder {
    -Guid commandId
    -ulong actionId
    -CommandValue target
    -CommandValue scope
    -string idempotencyKey
    -CommandOutcomeContract outcomeContract
    -List~CommandArgument~ arguments
    -List~CommandPrecondition~ preconditions
    +WithIdentifier(ulong, ulong)
    +WithText(ulong, string)
    +WithValue(ulong, CommandValue)
    +WithPrecondition(string, string)
    +Build() CommandInstruction
  }
  CommandInstructionBuilder --> CommandInstruction : immutable snapshot
~~~

## Use cases

- Construct a command with all required identity and contract metadata.
- Add ordered typed parameters.
- Attach opaque grammar-versioned preconditions without evaluating them.
- Build a validated immutable command snapshot.

Source: [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs). Tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
