<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposal — UML neighborhood and use cases

> Relationships are authored from the approved source model; this is not a runtime trace or generated UML.

## Neighborhood

~~~mermaid
classDiagram
  class CommandProposal {
    +string ProtocolVersion
    +IReadOnlyList~CommandInstruction~ Commands
    +CurrentProtocolVersion = CommandAi/1
  }
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
  CommandProposal --> CommandInstruction : ordered commands
  CommandInstruction --> CommandArgument : ordered parameters
  CommandArgument --> CommandValue : typed value
~~~

## Source-supported relationships

- **Target source:** [CommandProposal.cs](../../CommandProposal.cs).
- **Related source:** [CommandInstruction.cs](../../CommandInstruction.cs), [CommandArgument.cs](../../CommandArgument.cs), [CommandValue.cs](../../CommandValue.cs), and [CommandLimits.cs](../../CommandLimits.cs).
- **Codec:** [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs) reads and writes the line-oriented CommandAi/1 envelope.
- **Invariants:** proposals are non-empty, ordered, bounded; action and parameter identities are positive; duplicate parameter identities are rejected within a command.

## Use cases

### UC-1 — Decode a proposed batch
- **Actor:** consuming host.
- **Trigger:** receive a candidate CommandAi/1 envelope.
- **Outcome:** a structurally valid proposal is available for host-owned semantic validation.
- **Failure:** malformed content, unsupported protocol, invalid identifiers, duplicates, or resource-limit violations are rejected.

### UC-2 — Compose atomic actions
- **Actor:** application or host.
- **Goal:** represent several ordered action proposals.
- **Outcome:** the host receives an ordered list. The list does not imply transaction, dependency, retry, or rollback semantics.

## Related pages

- [Type page](../types/CommandProposal.md)
- [Theory](../THEORY.md)
- [Problem domain](../PROBLEM_DOMAIN.md)


---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
