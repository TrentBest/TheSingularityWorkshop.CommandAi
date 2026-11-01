<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposalBuilder — UML neighborhood and use cases

~~~mermaid
classDiagram
  class CommandProposalBuilder {
    +AddCommand(Guid, ulong, CommandValue, CommandValue, string, CommandOutcomeContract, Action)
    +Build() CommandProposal
  }
  class CommandInstructionBuilder {
    +WithIdentifier(ulong, ulong)
    +WithText(ulong, string)
    +WithValue(ulong, CommandValue)
    +WithPrecondition(string, string)
    +Build() CommandInstruction
  }
  CommandProposalBuilder --> CommandInstructionBuilder : scopes authoring
  CommandProposalBuilder --> CommandProposal : snapshots
  CommandInstructionBuilder --> CommandInstruction : builds complete command
~~~

## Use cases

- Host extensions compose named application operations using the generic builder.
- Each command must declare identity, action, target, scope, idempotency key, and outcome contract.
- Parameters and optional preconditions are added through the scoped instruction builder.
- Build snapshots the proposal; it does not validate domain meaning or grant execution authority.

Source: [CommandProposalBuilder.cs](../../CommandProposalBuilder.cs). Tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
