<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi architecture

## Component map

    Untrusted response text
             |
             v
    CommandAiTextCodec -- parses/encodes --> CommandProposal
                                                |
                                                v
                                      Host-owned validation
                                                |
                                                v
                                      Host-owned authorization
                                                |
                                                v
                                      Explicit approval policy
                                                |
                                                v
                                      Host-owned dispatch/execution

This is an authored conceptual model, not a generated source diagram or runtime trace.

## Direct dependencies

The core package uses only the .NET base class library. ProtocolAi, GrammarAi, HeadlessAi, and application-specific systems are adjacent concepts, not required runtime dependencies. Integration adapters can be developed later if they preserve those ownership boundaries.

## Failure behavior

- Decode with null, empty, or whitespace-only input: argument exception because the caller did not supply an envelope.
- Non-empty input with a missing/malformed envelope, unsupported or missing version, invalid identifiers, malformed JSON literals, duplicate parameters, unknown entries, or no commands: format exception because the supplied representation is not a valid CommandAi proposal.
- Direct model construction (CommandProposal, CommandInstruction, CommandArgument, and CommandValue factories): argument exceptions for invalid caller-supplied values.
- Encoding a valid in-memory proposal whose encoded representation exceeds CommandLimits.MaxEnvelopeCharacters: argument exception because the caller supplied a proposal that cannot fit the current wire envelope.

The decoder normalizes model-construction failures caused by malformed wire content into FormatException; callers should not need to distinguish parser errors from validation errors leaked by internal model constructors. Callers should treat any parse failure as a rejected proposal and avoid fallback paths that silently reinterpret malformed data.

## Atomicity and lifecycle boundary

Each CommandInstruction is intended to represent one atomic proposed action with explicit command identity, target, scope, ordered parameters, preconditions, idempotency identity, and an outcome contract. CommandAi owns the bounded, immutable representation and its codec; it does not interpret domain-specific target/scope values or decide whether a precondition is true. The current ActionId-plus-Arguments implementation is an incomplete first slice and does not yet satisfy this full contract. A CommandProposal is an ordered batch of atomic instructions; it is not a transaction and does not require every instruction to execute.

The generic fields must preserve enough opaque, typed data for a host to make independent decisions. For example, repository, isolated branch, base/candidate SHA, exact path, operation kind, expected content digest, and bounded payload are values a repository-domain extension can express without hard-coding GitHub or CiAi semantics into CommandAi. GrammarAi validates structured condition/success-action grammar; ProtocolAi carries correlated, provenance-bearing event and outcome envelopes; CiAi remains authoritative for CI evidence and verdicts. Durable observer lifecycle, scheduling, truth evaluation against live state, authorization, approval, dispatch, and execution remain host responsibilities. A successful parse is never proof of permission or successful execution.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
