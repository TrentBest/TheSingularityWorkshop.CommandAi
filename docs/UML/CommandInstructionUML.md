<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstruction — UML neighborhood and use cases

~~~mermaid
classDiagram
  class CommandInstruction {
    +Guid? CommandId
    +ulong ActionId
    +CommandValue? Target
    +CommandValue? Scope
    +string? IdempotencyKey
    +IReadOnlyList~CommandArgument~ Arguments
    +IReadOnlyList~CommandPrecondition~ Preconditions
    +CommandOutcomeContract? OutcomeContract
    +bool HasAtomicContract
  }
  class CommandArgument {
    +ulong ParameterId
    +CommandValue Value
  }
  class CommandValue {
    +CommandValueKind Kind
    +ulong? Identifier
    +string? Text
  }
  class CommandPrecondition {
    +string GrammarVersion
    +string Expression
  }
  class CommandOutcomeContract {
    +ulong OutcomeId
    +IReadOnlyList~CommandOutcomeField~ Fields
  }
  CommandInstruction --> CommandArgument : ordered parameters
  CommandInstruction --> CommandPrecondition : opaque conditions
  CommandInstruction --> CommandOutcomeContract : expected shape
  CommandArgument --> CommandValue : typed value
~~~

## Source-supported relationships

- Target source: [CommandInstruction.cs](../../CommandInstruction.cs).
- Related source: CommandArgument.cs, CommandValue.cs, CommandPrecondition.cs, CommandOutcomeContract.cs, CommandProposal.cs.
- Codec: CommandAiTextCodec.cs encodes compact and enriched CommandAi/1 entries.
- Invariant: action and parameter identities are positive; parameter identities are unique within a command. Enriched metadata is all-or-nothing; compact instructions retain absent metadata and HasAtomicContract is false.

A command is a representation of intent, not an authorization decision or execution record. The host owns domain validation and dispatch.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
