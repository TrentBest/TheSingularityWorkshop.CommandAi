<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# ✳️ 00 TheSingularityWorkshop.CommandAi

[![Build](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml/badge.svg?branch=development)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/LICENSE.txt)

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.CommandAi/development/docs/images/command-ai-boundary.svg" alt="CommandAi represents a command proposal; a trusted host resolves, validates, authorizes, and executes separately." width="100%"></p>
<p align="center"><em>A well-formed proposal is still only a proposal.</em></p>

## 01 What is CommandAi?

**CommandAi defines a provider-neutral, immutable representation for proposed atomic actions.** Its CommandAi/1 model supports both the original compact action-plus-arguments form and an enriched atomic-command form with distinct command identity, target, scope, structured preconditions, idempotency identity, expected-outcome contract, and ordered typed arguments.

The compact form remains readable and writable. The enriched form is an additive /1 extension. It uses an explicit AtomicCommand entry so compact-only readers reject it rather than silently misinterpreting it. Consumers must upgrade before accepting enriched commands.

CommandAi represents intent. It does not resolve application meaning, parse or evaluate precondition grammar, validate live state, authorize, guarantee idempotent delivery, or execute commands.

## 02 Why does it exist?

AI-generated and agent-generated content is untrusted. A host needs a bounded, provider-neutral contract for command identity, target/scope, conditions, arguments, and expected outcomes without confusing a proposal with permission or execution. CommandAi supplies the generic data shape while leaving semantics and authority with the owning layers.

## 03 How does it work?

A legacy compact CommandAi/1 entry remains unchanged:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=300 201="diagnostic stream"
[/CommandAi]
~~~

An enriched atomic command uses a distinct JSON entry within the same protocol version:

~~~text
[CommandAi]
Protocol=CommandAi/1
AtomicCommand={"commandId":"11111111-2222-3333-4444-555555555555","actionId":100,"target":{"kind":"LiteralText","identifier":null,"text":"repository"},"scope":{"kind":"LiteralText","identifier":null,"text":"feature/ref"},"idempotencyKey":"operation-001","outcomeContract":{"outcomeId":700,"fields":[]},"arguments":[],"preconditions":[]}
[/CommandAi]
~~~

The codec accepts compact and enriched entries in one ordered proposal. The enriched JSON is bounded and rejects unknown or duplicate properties. Older compact-only readers do not understand AtomicCommand entries and must be upgraded before consuming them.

## 04 See it in a minute

~~~bash
dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj
~~~

The quickstart demonstrates the legacy compact form. See the [usage guide](docs/USAGE.md) and [builder integration](docs/BUILDER_INTEGRATION.md) for enriched atomic commands.

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
| ProtocolAi | Vocabulary identity, correlated event and outcome envelopes |
| GrammarAi | Grammar definitions and structural validation of grammar-versioned conditions |
| CommandAi | Atomic-command proposal data, compact/enriched /1 codec, builders and representation bounds |
| HeadlessAi | Configured endpoint communication |
| CiAi | CI evidence and verdicts; may compose repository-specific CommandAi proposals |
| FSM_Foundry / host | Live-state checks, policy, authorization, approval, scheduling, dispatch, execution |

CommandAi has no runtime dependency on sibling packages. It carries precondition grammar identity and opaque expression data but does not parse or evaluate it. It carries an idempotency identity but does not implement replay or exactly-once delivery. It declares expected outcomes but does not produce actual outcomes.

## Verification and release status

The repository workflow restores, builds, tests, runs the quickstart, packs, and audits the package artifact. Package version remains `0.1.0-alpha.2`. The protocol identifier remains `CommandAi/1`; this additive contract work does not authorize a package version change or publication. **NuGet publication is not authorized unless explicitly approved by the owner.**

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
