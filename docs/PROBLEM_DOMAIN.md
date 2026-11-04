<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi problem domain

## The problem

A model or agent can propose an action, but free-form text is not a reliable interchange contract. Consuming applications need a bounded, provider-neutral way to represent an action identity and its typed parameters while retaining ownership of meaning and execution.

CommandAi supplies that compact contract. It does not attempt to become an application runtime or workflow engine.

## Domain concepts

- **Proposal:** an immutable, ordered, non-empty collection of command instructions.
- **Command:** an atomic action declaration identified by a positive opaque action ID.
- **Parameter:** an ordered pair of positive parameter ID and typed value.
- **Value:** either a positive integer identifier or bounded literal text.
- **Protocol:** the explicit CommandAi/1 wire contract.

A proposal is not a transaction. Ordering is represented, but all-or-nothing execution, dependencies, retries, rollback, cancellation, and compensation are not.

## Responsibility boundary

CommandAi owns the model, builders, representation limits, and CommandAi/1 codec. The host owns identity meaning, domain validation, live-state checks, authorization, approval, dispatch, execution, and actual outcomes. ProtocolAi, GrammarAi, HeadlessAi, CiAi, and FSM_Foundry retain their respective vocabulary, grammar, transport, CI evidence, and runtime responsibilities.

## Acceptance criteria

1. Proposals contain at least one command and remain ordered.
2. Action and parameter identifiers are positive.
3. Arguments retain their declared order and typed values.
4. Literal values and aggregate representations are bounded.
5. The codec round-trips the approved /1 representation and rejects malformed or unsupported envelopes.
6. Invalid wire data is not silently reinterpreted as another protocol.
7. No API grants authority, resolves application meaning, or executes commands.
8. The package remains independent of UI and sibling runtime packages.

## Current verification

The source, tests, quickstart, package pack, and packed-package consumer audit passed exact-head [CommandAi CI run 41753414951](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41753414951) on `development` commit `d66db6bf9b53a893f8c83bca03efaf525b3ce07b`. The run also verified Workshop Markdown branding and coverage upload; `publish_nuget` was skipped. See [REFLECTION](REFLECTION.md) for remaining domain and integration gaps. Package publication is not authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
