<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Legacy CommandAi — historical reference

## Status

**Historical reference only.** The old CommandAi implementation is not a compatibility target. The repository owner has decided that preserving or recreating the legacy implementation is not worth the effort beyond the source-level observations already recorded here.

Do not add a legacy parser, compatibility adapter, captured fixture corpus, or differential test project. Do not delay current package development to reproduce the old behavior.

## What the source review established

The legacy implementation reviewed at [The / Architect/Workshop/Ai/CommandAi.cs](https://github.com/TrentBest/The/blob/6ad1971f3932654ea58f8dd8dbc0dde530e4e4c6/Architect/Workshop/Ai/CommandAi.cs) and its adjacent ProtocolAi/GrammarAi sources:

- used a CommandAi/1 envelope marker;
- accepted a Protocol header case-insensitively and permitted it to be absent;
- parsed human-readable and compact integer command forms;
- mapped integer action IDs through a fixed legacy ProtocolAi table;
- supported legacy Surface and Repository parameters;
- resolved Surface IDs through a fixed table and unquoted Repository strings;
- mixed generic parsing with application-specific command meanings and approval/execution concerns.

These are source-reviewed observations, not a verified interoperability contract. No captured historical input corpus or runtime compatibility claim is retained.

## Current package boundary

The current CommandAi package intentionally provides a narrower, generic representation:

- exact `Protocol=CommandAi/2` envelope identity;
- unique command identity, positive opaque action identity, typed target and scope;
- typed parameters, grammar-versioned opaque preconditions, and a required idempotency key;
- an explicit expected outcome contract;
- ordered immutable command proposals and generic fluent builders;
- strict JSON command encoding/decoding with explicit resource limits.

The previous standalone package alpha shape (action plus arguments only) was also replaced. See the [alpha consumer migration guide](MIGRATION_COMMAND_AI_2.md).

CommandAi does not resolve identifier meanings, authorize, approve, or execute commands. Application/domain extensions own named operations and semantic validation; the host owns trust, current-state checks, authorization, approval, and execution.

## Decision

The former compatibility-corpus work item, [Issue #2](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/issues/2), was closed as **not planned** by repository-owner direction. The source observations above are retained only to prevent accidental claims that the modern package is a drop-in replacement.

For current implementation guidance, use the [builder integration guide](BUILDER_INTEGRATION.md), [usage guide](USAGE.md), and [architecture](ARCHITECTURE.md).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
