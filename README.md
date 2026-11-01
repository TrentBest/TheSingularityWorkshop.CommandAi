<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# ✳️ 00 TheSingularityWorkshop.CommandAi

[![Build](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml/badge.svg?branch=development)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/LICENSE.txt)

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.CommandAi/development/docs/images/command-ai-boundary.svg" alt="CommandAi represents a complete atomic command proposal; trusted host layers resolve, validate, authorize, and execute separately." width="100%"></p>
<p align="center"><em>A well-formed proposal is still only a proposal.</em></p>

## 01 What is CommandAi?

**CommandAi defines a provider-neutral, versioned representation for complete atomic commands proposed by an AI or another untrusted source.** A command explicitly carries its identity, action, target, scope, ordered parameters, preconditions, idempotency key, and expected outcome contract.

The package stores opaque identities and typed values. It does not resolve application meaning, evaluate conditions, authorize actions, or execute them.

## 02 Why does it exist?

A command proposal must carry enough information for a host to reason about what is requested, where it applies, under which conditions, how duplicate delivery is recognized, and what shape of result is expected. Leaving those fields implicit forces every consumer to invent a different envelope.

CommandAi provides one bounded immutable contract. It deliberately keeps the meaning of identifiers and expressions outside the generic package.

## 03 How does it work?

A compact envelope contains one JSON command object per line:

~~~text
[CommandAi]
Protocol=CommandAi/2
Command={"commandId":"...","actionId":100,"target":{"kind":"IntegerIdentifier","identifier":300,"text":null},"scope":{"kind":"LiteralText","identifier":null,"text":"workspace"},"idempotencyKey":"inspect:resource:42","outcomeContract":{"outcomeId":400,"fields":[{"fieldId":401,"valueKind":"LiteralText","required":true}]},"arguments":[],"preconditions":[]}
[/CommandAi]
~~~

Each command declares identity, target, scope, parameters, grammar-versioned preconditions, an idempotency key, and an expected outcome shape. These values are opaque to CommandAi. A precondition is stored but not evaluated; an idempotency key is carried but not enforced; an outcome contract is a declaration, not an execution result or CI verdict.

## 04 See it in a minute

Run the local example:

~~~bash
dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj
~~~

The quickstart builds InspectResource and OpenDiagnosticStream as host-owned extensions over the generic builder. Their action, target, scope, idempotency, and outcome identities are examples, not universal meanings.

A direct consumer can compose a command with Guid identity, opaque target/scope values, a required idempotency key, an outcome contract, parameters, and preconditions. See the usage guide for the complete C# example.

## 05 Documentation and theory

- [Documentation index](DOCUMENTATION_INDEX.md)
- [AI package boundaries](docs/AI_SERIES.md)
- [Problem domain](docs/PROBLEM_DOMAIN.md)
- [Command domain completeness and ownership](docs/DOMAIN_COMPLETENESS.md)
- [Architecture](docs/ARCHITECTURE.md)
- [Theory and CommandAi/2 wire contract](docs/THEORY.md)
- [Alpha consumer migration to CommandAi/2](docs/MIGRATION_COMMAND_AI_2.md)
- [Usage guide](docs/USAGE.md)
- [Builder integration](docs/BUILDER_INTEGRATION.md)
- [Type guides](docs/types/)
- [UML and use cases](docs/UML/)
- [Legacy implementation history](docs/LEGACY_COMPATIBILITY.md)

## Package boundaries

| Package | Owns |
|---|---|
| [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | Application-owned vocabulary and stable protocol-qualified symbol identity |
| [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Structural grammar definitions and composition rules |
| **CommandAi** | Complete atomic command proposal representation and codec |
| [HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi) | Configured AI endpoint request/response communication |
| [CiAi](https://github.com/TrentBest/TheSingularityWorkshop.CiAi) | CI evidence and verdicts |
| [FSM_Foundry](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Foundry) / host | Durable observer lifecycle, policy, authorization, approval, scheduling, dispatch, and execution |

CommandAi has no runtime dependency on these sibling packages. Their contracts are connected by explicit host or integration adapters, not hidden package coupling.

## Verification and release status

The repository workflow restores, builds, tests, runs the quickstart, packs, and audits the package artifact. **NuGet publication remains disabled** unless the owner explicitly authorizes a specific release. The CommandAi/1 wire shape is intentionally not supported by this breaking CommandAi/2 contract; there is no external compatibility promise.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
