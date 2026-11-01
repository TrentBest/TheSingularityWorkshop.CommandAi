<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# ✳️ 00 TheSingularityWorkshop.CommandAi

[![Build](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml/badge.svg?branch=development)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/TheSingularityWorkshop.CommandAi?style=flat-square)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commits/development)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/LICENSE.txt)

<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.CommandAi/development/docs/images/command-ai-boundary.svg" alt="CommandAi parses a proposed command into a versioned representation; the host separately resolves meaning, validates policy, obtains approval, and executes." width="100%"></p>
<p align="center"><em>A well-formed proposal is still only a proposal.</em></p>

## 🟦 01 What is CommandAi?

**CommandAi gives software a deterministic representation for commands proposed by an AI or another untrusted input source.** It models a versioned proposal as action identifiers and parameter/value pairs, with explicit support for integer identities and literal text.

CommandAi is not an autonomous agent and is not a command runner. It turns a representation into inspectable data; it does not turn that data into permission.

## 🟣 02 Why does it exist?

Free-form model output is hard for a host to validate reliably. A small, explicit envelope gives the host something it can parse, test, log, and reject deterministically. Integer identities keep known vocabulary separate from model-generated spelling; literal text remains visibly data.

The analogy is a shipping label: it helps identify what is being proposed, but it does not prove the contents are correct or authorize delivery.

## 🩵 03 How does it work?

    [CommandAi]
    Protocol=CommandAi/1
    Command=100 200=302 201="Logs.Diagnostic Stream"
    [/CommandAi]

- 100 is an opaque action identity.
- 200=302 pairs a parameter identity with an integer value identity.
- 201="Logs.Diagnostic Stream" pairs a parameter identity with literal text.

The integers are examples, not built-in meanings. The consuming host resolves identities against trusted vocabulary and applies its own domain rules.

## 🟢 04 See it in a minute

Run the local example:

    dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj

The quickstart now demonstrates a host-owned named-command extension layer in `ResourceCommandExtensions.cs`: it adds typed `InspectResource` and `OpenDiagnosticStream` methods over the generic CommandAi builder. These action/parameter IDs are sample values owned by the example host, not built-in CommandAi meanings.

Or create a generic proposal directly in C#:

    using TheSingularityWorkshop.CommandAi;
    var proposal = new CommandProposalBuilder()
        .AddCommand(100, command => command
            .WithIdentifier(200, 302)
            .WithText(201, "Logs.Diagnostic Stream"))
        .Build();
    var wireText = CommandAiTextCodec.Encode(proposal);
    var decoded = CommandAiTextCodec.Decode(wireText);
    Console.WriteLine(wireText);

**Verification:** GitHub Actions run [#89](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41283845342) verified the exact development head at commit [`96adbaa`](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commit/96adbaa023fd5a052479d809a2061801790e9f6a): Release build, all 20 tests, quickstart execution, package packing, packed-artifact audit, and compilation of the quickstart against the packed package. The NuGet publication job was skipped. This verifies the local representation path, not live-provider behavior, vocabulary resolution, authorization, or execution.

## 🟪 05 Documentation and theory

- [Documentation index](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/DOCUMENTATION_INDEX.md) — reader-oriented guide to the package, types, UML, and evidence.
- [AI series boundaries](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/AI_SERIES.md) — responsibilities and trust boundaries across ProtocolAi, GrammarAi, CommandAi, and HeadlessAi.
- [Per-type documentation](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/tree/development/docs/types) and [UML/use cases](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/tree/development/docs/UML) — source-grounded public type contracts and consumer scenarios.
- [Usage guide](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/USAGE.md) — construct, encode, decode, and verify a proposal.
- [Builder integration](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/BUILDER_INTEGRATION.md) — layer host-owned named commands over the generic builder.
- [Problem domain](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/PROBLEM_DOMAIN.md) — purpose, non-goals, and acceptance criteria.
- [Theory](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/THEORY.md) — why representation, identity, structure, and authority are separate.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/ARCHITECTURE.md) — component boundary and failure behavior.
- [Gallery](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/GALLERY.md) — the current reproducible example and what it proves.
- [Legacy historical reference](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/LEGACY_COMPATIBILITY.md) — exact differences from the implementation in The.
- [Status and open questions](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/docs/REFLECTION.md) — implementation state and remaining evidence.
- [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/DOCUMENTATION_STANDARD.md).

## Package boundary

| Package | Domain |
|---|---|
| [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | Vocabulary and symbol identity |
| [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Legal structural composition |
| **CommandAi** | Versioned command proposals and representation |
| [HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi) | Configured AI endpoint communication |
| Host application | Trust, policy, approval, and execution |

This separation is an architectural intent; cross-package compatibility must be verified before claiming a complete end-to-end protocol. CommandAi has no runtime dependency on its siblings in this initial implementation.

## Verification and release status

The repository workflow restores, builds, tests, packs, and uploads an artifact for inspection. **NuGet publication is disabled** unless the owner explicitly authorizes a release. No package release is being performed by this work.

## Resources and support

- [Source repository](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi)
- [The Singularity Workshop on GitHub](https://github.com/TrentBest)
- [CoderLegion](https://coderlegion.com/user/The+Singularity+Workshop)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
