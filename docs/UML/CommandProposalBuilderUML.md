<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposalBuilder — UML and use cases

## Class neighborhood

```mermaid
classDiagram
    class CommandProposalBuilder {
      -List~CommandInstruction~ commands
      +AddCommand(actionId, configure) CommandProposalBuilder
      +Build() CommandProposal
    }
    class CommandInstructionBuilder {
      +WithIdentifier(parameterId, valueId) CommandInstructionBuilder
      +WithText(parameterId, value) CommandInstructionBuilder
      +WithValue(parameterId, value) CommandInstructionBuilder
      +Build() CommandInstruction
    }
    class CommandProposal {
      +ProtocolVersion string
      +Commands IReadOnlyList
    }
    CommandProposalBuilder --> CommandInstructionBuilder : scopes configuration
    CommandProposalBuilder --> CommandProposal : snapshots
    CommandInstructionBuilder --> CommandInstruction : snapshots
```

## Use case: construct a proposal

1. Create a `CommandProposalBuilder`.
2. Add each opaque action identity in order.
3. Configure parameters using the scoped instruction builder.
4. Call `Build` to obtain a validated immutable proposal.
5. Encode with `CommandAiTextCodec` if a text representation is required.

## Boundary

The builder only constructs representation. The host still resolves IDs, validates current domain state, applies policy and authorization, obtains approval where required, and chooses whether to execute.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
