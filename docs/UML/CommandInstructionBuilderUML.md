<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder — UML neighborhood and use cases

~~~mermaid
classDiagram
  class CommandInstructionBuilder {
    -ulong actionId
    -List~CommandArgument~ arguments
    +WithIdentifier(ulong, ulong)
    +WithText(ulong, string)
    +WithValue(ulong, CommandValue)
    +Build() CommandInstruction
  }
  CommandInstructionBuilder --> CommandInstruction : immutable snapshot
~~~

## Use cases

- Add a positive action identity.
- Add ordered parameters as integer identifiers or literal text.
- Build a validated immutable command instruction.

The builder does not accept command GUID, target/scope, idempotency, preconditions, or expected-outcome metadata in the approved /1 model.

Source: [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs). Tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
