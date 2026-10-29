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

The consuming host must resolve identifiers against trusted definitions, validate business rules and current state, apply authorization and approval policy, and only then dispatch an action. A well-formed proposal is not proof of intent, truth, permission, or safety.

## Acceptance criteria

1. Invalid or missing protocol versions are rejected.
2. Actions and parameters require positive integer identifiers.
3. Repeated parameter identifiers in one instruction are rejected.
4. Literal strings round-trip without being confused with integer identifiers.
5. The codec rejects malformed envelopes rather than silently skipping unknown entries.
6. No API in this package executes commands or grants authority.
7. The package remains provider-neutral and has no runtime dependency on ProtocolAi, GrammarAi, HeadlessAi, or host application frameworks.

## Evidence status

This first implementation is source-authored and includes focused unit tests and a runnable quickstart. It must be built and tested by the repository workflow before any release claim is made. NuGet publication is not authorized by this work.
