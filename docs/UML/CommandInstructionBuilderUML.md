<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder — UML and use cases

## Class neighborhood

```mermaid
classDiagram
    class CommandInstructionBuilder {
      -ulong actionId
      -List~CommandArgument~ arguments
      +WithIdentifier(parameterId, valueId) CommandInstructionBuilder
      +WithText(parameterId, value) CommandInstructionBuilder
      +WithValue(parameterId, value) CommandInstructionBuilder
      +Build() CommandInstruction
    }
    class CommandInstruction {
      +ActionId ulong
      +Arguments IReadOnlyList
    }
    class CommandArgument {
      +ParameterId ulong
      +Value CommandValue
    }
    class CommandValue {
      +Kind CommandValueKind
      +Identifier ulong?
      +Text string?
    }
    CommandInstructionBuilder --> CommandInstruction : snapshots
    CommandInstruction --> CommandArgument : contains
    CommandArgument --> CommandValue : contains
```

## Use case: add a typed argument

- Use `WithIdentifier(parameterId, valueId)` when the value is an opaque integer identity.
- Use `WithText(parameterId, value)` when the value is literal text.
- Use `WithValue(parameterId, value)` for an already constructed CommandValue.
- Build to validate and snapshot the ordered arguments.

## Boundary

The class does not know what an action or parameter ID means. It does not link ProtocolAi definitions, load GrammarAi, or execute a command.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
