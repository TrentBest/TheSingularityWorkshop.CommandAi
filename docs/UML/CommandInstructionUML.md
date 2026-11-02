<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstruction — UML neighborhood and use cases

~~~mermaid
classDiagram
  class CommandInstruction {
    +ulong ActionId
    +IReadOnlyList~CommandArgument~ Arguments
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
  CommandInstruction --> CommandArgument : ordered parameters
  CommandArgument --> CommandValue : typed value
~~~

## Source-supported relationships

- **Target source:** [CommandInstruction.cs](../../CommandInstruction.cs).
- **Related source:** CommandArgument.cs, CommandValue.cs, CommandProposal.cs.
- **Codec:** CommandAiTextCodec.cs encodes and decodes the CommandAi/1 representation.
- **Invariant:** action and parameter identities are positive; parameter identities are unique within a command.

A command is a representation of intent, not an authorization decision or execution record. The host owns domain validation and dispatch.

## Use cases

- Inspect a decoded atomic action and its typed arguments.
- Construct an instruction with the fluent builder.
- Preserve its argument order when placed into a proposal.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
