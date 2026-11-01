<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi problem domain

## The problem

An AI model can return an instruction that looks plausible to a person but is ambiguous, malformed, unsupported, or unsafe for the host application. Free-form text is convenient to read, but weak as an interchange contract when software needs to inspect a proposal deterministically.

CommandAi defines a small, versioned representation for proposed commands. It keeps action and parameter identities numeric when those identities are available, while allowing explicit literal text where the vocabulary intentionally permits it.

## Domain concepts

- **Proposal** — a versioned collection of one or more command instructions.
- **Instruction** — an action identity plus zero or more parameter arguments.
- **Argument** — a parameter identity paired with a typed value.
- **Integer identifier** — an opaque positive identity. CommandAi does not decide what it means.
- **Literal text** — a string carried explicitly as data, not interpreted as executable code.

## Responsibility boundary

CommandAi validates proposal shape. It does not own vocabulary or resolve integer identities (ProtocolAi's domain); define legal composition rules for a larger language (GrammarAi's domain); call an AI endpoint (HeadlessAi's domain); decide whether a proposed action is allowed; request user approval; access application resources; or execute commands.

The generic atomic Command contract must explicitly represent command identity, target, scope, parameters, preconditions, idempotency, and an outcome contract. CommandAi owns the immutable, bounded representation and codec, not the domain-specific interpretation of those fields. The current ActionId-plus-Arguments model is incomplete against this contract. A well-formed proposal is not proof of intent, truth, permission, or safety. A proposal is an ordered batch of atomic commands, not a transaction. Repository/branch/SHA/path/content-digest values can be carried as opaque typed data; the consuming repository-domain extension and host validate their meaning. GrammarAi owns structural grammar validation, ProtocolAi owns correlated/provenance-bearing event and outcome envelopes, CiAi owns CI evidence and verdicts, and the host owns durable lifecycle, authorization, approval, dispatch, and execution.

## Resource boundaries

A proposal is limited to 1,048,576 literal-text characters in aggregate, in addition to the per-literal, per-command, command-count, and encoded-envelope limits in `CommandLimits`. This prevents many individually valid arguments from producing an unexpectedly large in-memory proposal. Integer identifiers remain opaque and do not contribute literal text size.

## Acceptance criteria

1. Invalid or missing protocol versions are rejected.
2. Actions and parameters require positive integer identifiers.
3. Repeated parameter identifiers in one instruction are rejected.
4. Literal strings round-trip without being confused with integer identifiers.
5. The codec rejects malformed envelopes rather than silently skipping unknown entries, and reports malformed non-empty wire content as FormatException.
6. No API in this package executes commands or grants authority.
7. The package remains provider-neutral and has no runtime dependency on ProtocolAi, GrammarAi, HeadlessAi, or host application frameworks.

## Evidence status

This first implementation is source-authored and includes focused unit tests and a runnable quickstart. It must be built and tested by the repository workflow before any release claim is made. NuGet publication is not authorized by this work.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
