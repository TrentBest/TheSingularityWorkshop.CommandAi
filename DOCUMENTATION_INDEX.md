<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi — Documentation index

CommandAi/2 represents complete atomic command proposals as inspectable data. The host remains responsible for vocabulary resolution, precondition evaluation, idempotency enforcement, authorization, approval, and execution.

## Start here

| Goal | Read |
|---|---|
| Understand the problem and boundary | [README](README.md) · [Problem domain](docs/PROBLEM_DOMAIN.md) |
| Learn the versioned wire contract | [Theory](docs/THEORY.md) |
| Use the current API | [Usage guide](docs/USAGE.md) |
| Understand package ownership and handoffs | [Architecture](docs/ARCHITECTURE.md) · [AI series](docs/AI_SERIES.md) |
| Understand builders and host extensions | [Builder integration](docs/BUILDER_INTEGRATION.md) |
| Reproduce the local example | [Gallery](docs/GALLERY.md) · [Quickstart](examples/CommandAi.QuickStart/Program.cs) |
| Understand historical implementation | [Legacy historical reference](docs/LEGACY_COMPATIBILITY.md) |
| Review maturity and remaining evidence | [Status](docs/REFLECTION.md) |
| Inspect public type contracts | [Type guides](docs/types/) |
| Inspect source-backed neighborhoods | [UML and use cases](docs/UML/) |
| Read agent instructions | [AGENTS.md](AGENTS.md) |
| Check shared documentation standards | [FSM_COS Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md) |

## Public type guides

| Type | Contract |
|---|---|
| CommandProposal | [Type guide](docs/types/CommandProposal.md) |
| CommandInstruction | [Type guide](docs/types/CommandInstruction.md) |
| CommandArgument | [Type guide](docs/types/CommandArgument.md) |
| CommandValue | [Type guide](docs/types/CommandValue.md) |
| CommandValueKind | [Type guide](docs/types/CommandValueKind.md) |
| CommandPrecondition | [Type guide](docs/types/CommandPrecondition.md) |
| CommandOutcomeContract | [Type guide](docs/types/CommandOutcomeContract.md) |
| CommandOutcomeField | [Type guide](docs/types/CommandOutcomeField.md) |
| CommandLimits | [Type guide](docs/types/CommandLimits.md) |
| CommandAiTextCodec | [Type guide](docs/types/CommandAiTextCodec.md) |
| CommandProposalBuilder | [Type guide](docs/types/CommandProposalBuilder.md) |
| CommandInstructionBuilder | [Type guide](docs/types/CommandInstructionBuilder.md) |

## Evidence and limits

Type guides and diagrams are human-authored and checked against current source. They are not generated from a compiler AST or a repository-wide consumer scan. The current development head must pass the repository workflow, including tests, quickstart, package packing, packed-artifact audit, and consumer compilation, before implementation status is marked CI-verified. NuGet publication remains disabled.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
