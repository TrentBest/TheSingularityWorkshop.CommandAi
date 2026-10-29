<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# The Workshop AI package series

## One stack, separate responsibilities

The package names describe different boundaries, not interchangeable ways to “make AI work.”

| Package | Domain question | Responsibility | Explicit non-goal |
|---|---|---|---|
| [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | What does this identity mean? | Application-owned vocabulary and protocol-qualified symbol resolution | Inference, authorization, execution |
| [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Which symbol structures are valid? | Grammar definitions, productions, and structural validation | Owning vocabulary meaning, parsing arbitrary natural language, execution |
| **CommandAi** | What command is being proposed? | Versioned command proposal model and deterministic envelope codec | Granting authority or dispatching commands |
| [HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi) | How does a host exchange content with an endpoint? | Configured HTTP endpoint invocation through explicit protocol adapters | Owning task policy, memory, command meaning, or execution |

The host application surrounds these packages with trusted configuration, domain validation, authorization, approval policy, logging, and any consequential action. A response from an AI endpoint is untrusted input even when it parses correctly.

## Conceptual data flow

    Host defines task and trusted vocabulary/grammar
                         |
                         v
    HeadlessAi sends request to configured endpoint
                         |
                         v
    Model returns candidate content
                         |
                         v
    Host or adapter validates expected structure
                         |
                         v
    CommandAi represents a proposed command
                         |
                         v
    ProtocolAi resolves vocabulary identities
                         |
                         v
    GrammarAi validates composition where applicable
                         |
                         v
    Host validates current state, authorizes, and approves
                         |
                         v
    Host dispatches an accepted command

This is an authored conceptual flow, not a claim that a single turnkey integration currently wires all four packages together. The order of validation can vary by host: a consumer may validate grammar before creating a command proposal or resolve identifiers as part of domain validation. The invariant is that no package-level parse or lookup authorizes execution.

## Version and compatibility questions

Before claiming cross-package interoperability, document and test:

- which protocol and grammar identities are expected;
- whether action and parameter identifiers use the same identity domain or are qualified by a protocol;
- how literal strings are escaped and distinguished from numeric identifiers;
- how missing, unknown, duplicate, or stale identifiers are rejected;
- how versions are declared and how incompatible versions fail;
- which component owns schema generation, output validation, and host approval.

The first CommandAi implementation uses opaque unsigned 64-bit identifiers, explicit literal strings, and a CommandAi/1 envelope. It intentionally has no runtime dependency on ProtocolAi, GrammarAi, or HeadlessAi until their contracts are reconciled with the legacy implementation in The.

## Security invariant

**Parsing is not permission.** A well-formed proposal can still be malicious, incorrect, stale, or unauthorized. The host must not execute an instruction solely because CommandAi decoded it or because an identifier exists in ProtocolAi.

## Reading path

- [CommandAi problem domain](PROBLEM_DOMAIN.md)
- [CommandAi architecture](ARCHITECTURE.md)
- [CommandAi wire format and rationale](THEORY.md)
- [CommandAi usage](USAGE.md)
- [CommandAi status and open questions](REFLECTION.md)
- [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)
