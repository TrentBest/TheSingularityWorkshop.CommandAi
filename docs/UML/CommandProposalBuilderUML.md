<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposalBuilder — UML neighborhood and use cases

~~~mermaid
classDiagram
 class CommandProposalBuilder {
  +AddCommand(ulong, Action)
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
 CommandInstructionBuilder --> CommandInstruction : builds
~~~

## Use cases

- Add compact commands to a proposal.
- Add enriched atomic commands with explicit generic fields.
- Configure ordered typed parameters and grammar-versioned preconditions.
- Build a non-empty immutable proposal and encode it with CommandAi/1.

The builder creates representation only. Host-owned extensions supply named operations and trusted identity mappings; the host retains semantic validation, policy, authorization, and execution.

Source: [CommandProposalBuilder.cs](../../CommandProposalBuilder.cs). Tests: [CommandAiAtomicCommandTests.cs](../../tests/CommandAi.Tests/CommandAiAtomicCommandTests.cs).
---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
