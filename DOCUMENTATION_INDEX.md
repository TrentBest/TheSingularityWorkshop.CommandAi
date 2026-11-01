<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi — Documentation index

CommandAi represents versioned command proposals as inspectable data. The host remains responsible for vocabulary resolution, validation, authorization, approval, and execution.

## Start here

| Your goal | Read |
|---|---|
| Understand the problem and the boundary | [README](README.md) · [Problem domain](docs/PROBLEM_DOMAIN.md) |
| Learn the versioned wire format | [Theory](docs/THEORY.md) |
| Use the current API | [Usage guide](docs/USAGE.md) |
| Understand package ownership and handoffs | [Architecture](docs/ARCHITECTURE.md) · [AI series boundaries](docs/AI_SERIES.md) |
| Understand the generic proposal layer and higher-level builders | [Builder integration](docs/BUILDER_INTEGRATION.md) |
| Reproduce the local example | [Gallery](docs/GALLERY.md) · [Quickstart source](examples/CommandAi.QuickStart/Program.cs) |
| Review legacy migration risks | [Legacy compatibility audit](docs/LEGACY_COMPATIBILITY.md) |
| Review maturity and open questions | [Status and open questions](docs/REFLECTION.md) |
| Inspect public type contracts | [Type guides](docs/types/) |
| Inspect source-backed neighborhoods and use cases | [UML and use cases](docs/UML/) |
| Read the repository's AI-agent instructions | [AGENTS.md](AGENTS.md) |
| Check the shared documentation standard | [FSM_COS Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md) |

## Public type guides

| Type | Contract and purpose | UML and use cases |
|---|---|---|
| CommandProposal | [Type guide](docs/types/CommandProposal.md) | [UML](docs/UML/CommandProposalUML.md) |
| CommandInstruction | [Type guide](docs/types/CommandInstruction.md) | [UML](docs/UML/CommandInstructionUML.md) |
| CommandArgument | [Type guide](docs/types/CommandArgument.md) | [UML](docs/UML/CommandArgumentUML.md) |
| CommandValue | [Type guide](docs/types/CommandValue.md) | [UML](docs/UML/CommandValueUML.md) |
| CommandValueKind | [Type guide](docs/types/CommandValueKind.md) | [UML](docs/UML/CommandValueKindUML.md) |
| CommandLimits | [Type guide](docs/types/CommandLimits.md) | [UML](docs/UML/CommandLimitsUML.md) |
| CommandAiTextCodec | [Type guide](docs/types/CommandAiTextCodec.md) | [UML](docs/UML/CommandAiTextCodecUML.md) |
| CommandProposalBuilder | [Type guide](docs/types/CommandProposalBuilder.md) | [UML](docs/UML/CommandProposalBuilderUML.md) |
| CommandInstructionBuilder | [Type guide](docs/types/CommandInstructionBuilder.md) | [UML](docs/UML/CommandInstructionBuilderUML.md) |

## Evidence and limits

The type guides and diagrams are human-authored and checked against current source references. They are not generated from a compiler AST or a repository-wide consumer scan. GitHub Actions run [#63](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41280904172) verified the source at commit [e998450](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commit/e99845013edf285ce0d86ecf667db77277035432): Release build, all 20 tests, quickstart, and package artifact. Subsequent changes on development in this sequence were documentation-only. This does not establish live-provider behavior, host authorization, or end-to-end interoperability with the other AI packages.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
