<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi — documentation index

CommandAi/1 is the provider-neutral representation for immutable, ordered atomic-action proposals and application address-space exchanges. Proposals support the original compact form and an enriched atomic-command form; the host owns meaning, policy, authorization, and execution.

## Start here

| Goal | Read |
|---|---|
| Understand the problem and boundaries | [README](README.md) · [Problem domain](docs/PROBLEM_DOMAIN.md) · [Domain completeness](docs/DOMAIN_COMPLETENESS.md) |
| Understand command theory and atomicity | [Theory](docs/THEORY.md) |
| Understand policy, authority, approval, disclosure, and execution ownership | [Governed command lifecycle](docs/GOVERNED_COMMAND_LIFECYCLE.md) |
| Use the current API and wire format | [Usage guide](docs/USAGE.md) |
| Understand package responsibilities | [Architecture](docs/ARCHITECTURE.md) · [AI series](docs/AI_SERIES.md) |
| Extend the generic builder | [Builder integration](docs/BUILDER_INTEGRATION.md) |
| Review historical compatibility notes | [Legacy reference](docs/LEGACY_COMPATIBILITY.md) |
| Review current maturity and evidence | [Status](docs/REFLECTION.md) |
| Inspect source-backed types and diagrams | [Type guides](docs/types/) · [UML](docs/UML/) |
| Read agent instructions | [AGENTS.md](AGENTS.md) |
| Shared documentation rules | [FSM_COS Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md) |

## Current public types

The approved /1 model is centered on `CommandProposal`, `CommandInstruction`, `CommandArgument`, `CommandValue`, `CommandValueKind`, `CommandLimits`, `CommandAiTextCodec`, `CommandProposalBuilder`, and `CommandInstructionBuilder`.

The enriched atomic-command form adds `CommandPrecondition`, `CommandOutcomeContract`, and `CommandOutcomeField`; compact commands leave enriched metadata absent rather than inferring defaults. The separate address-space surface includes `CommandAddressDescriptor`, `CommandAddressSpace`, `CommandAddressValue`, `CommandExchange`, `CommandResponse`, and `CommandExchangeTextCodec`.

All Markdown pages must retain the animated moniker at the top and canonical Workshop PNG footer. CI validates those branding requirements. No NuGet publication is authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
