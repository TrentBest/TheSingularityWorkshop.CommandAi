<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# ✳️ 00 TheSingularityWorkshop.CommandAi

[![Build](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml/badge.svg?branch=development)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/LICENSE.txt)

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.CommandAi/development/docs/images/command-ai-boundary.svg" alt="CommandAi represents a command proposal; a trusted host resolves, validates, authorizes, and executes separately." width="100%"></p>
<p align="center"><em>A well-formed proposal is still only a proposal.</em></p>

## 01 What is CommandAi?

**CommandAi defines a provider-neutral, immutable representation for proposed atomic actions.** The approved CommandAi/1 model contains an ordered proposal, action identities, and ordered typed arguments. Each value is either a positive integer identifier or bounded literal text.

CommandAi represents intent. It does not resolve application meaning, validate live state, authorize, or execute commands.

## 02 Why does it exist?

AI-generated and agent-generated content is untrusted. A host needs a small, bounded interchange contract instead of relying on ad-hoc prose or application-specific parsing. CommandAi provides that shared representation while leaving domain meaning and authority with the consuming application.

## 03 How does it work?

The line-oriented CommandAi/1 envelope:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=300 201="diagnostic stream"
[/CommandAi]
~~~

The first token is a positive action identifier. Each following token is a positive parameter identifier and a typed value. Integer identifiers remain opaque; literal strings are JSON-quoted. A proposal preserves command order but is not a transaction or workflow.

## 04 See it in a minute

~~~bash
dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj
~~~

The quickstart builds a proposal with the generic fluent builder, encodes it, and decodes it. Its sample identifiers are illustrative; a consuming host owns the trusted vocabulary and named operations.

## 05 Documentation and theory

- [Documentation index](DOCUMENTATION_INDEX.md)
- [Problem domain](docs/PROBLEM_DOMAIN.md)
- [Domain completeness and ownership](docs/DOMAIN_COMPLETENESS.md)
- [Theory and wire contract](docs/THEORY.md)
- [Architecture](docs/ARCHITECTURE.md)
- [Usage guide](docs/USAGE.md)
- [Builder integration](docs/BUILDER_INTEGRATION.md)
- [Type guides](docs/types/)
- [UML and use cases](docs/UML/)
- [Status and verification](docs/REFLECTION.md)
- [Historical reference](docs/LEGACY_COMPATIBILITY.md)

## Package boundaries

| Package | Responsibility |
|---|---|
| ProtocolAi | Protocol vocabulary, identity meaning, correlated events and actual outcomes |
| GrammarAi | Grammar definitions and structural validation |
| CommandAi | Immutable action/argument proposals, bounds, builders, CommandAi/1 codec |
| HeadlessAi | Configured endpoint communication |
| CiAi | CI evidence and verdicts |
| FSM_Foundry / host | Live-state checks, policy, authorization, approval, scheduling, dispatch, execution |

CommandAi has no runtime dependency on sibling packages. Presentation, accessibility, narration, haptics, and pedagogy belong to observer/application layers.

## Verification and release status

The repository workflow restores, builds, tests, runs the quickstart, packs, and audits the package artifact. Package version remains `0.1.0-alpha.2`. Internal consumers adapt to the approved /1 contract; no version bump is required for this coordination. **NuGet publication is not authorized unless explicitly approved by the owner.**

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
