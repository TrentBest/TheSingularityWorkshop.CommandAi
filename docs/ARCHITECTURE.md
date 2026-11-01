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
                                    Complete atomic command
                                    identity / action / target
                                    scope / parameters
                                    preconditions / idempotency
                                    expected outcome contract
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

The core package uses only the .NET base class library. ProtocolAi, GrammarAi, HeadlessAi, CiAi, and application-specific systems are adjacent concepts, not required runtime dependencies. Integration adapters connect their contracts at the host boundary.

## Atomic command contract

Each CommandInstruction requires a unique command identity, positive action identity, opaque typed target and scope, ordered typed parameters, an idempotency key, and an expected outcome contract. It may also carry bounded precondition expressions tagged with a GrammarAi version. CommandAi validates and preserves these fields; it does not assign their domain meaning.

A proposal is an ordered batch of atomic instructions, not a transaction. Commands may be independently accepted, rejected, retried, or executed according to host policy.

## Responsibility boundaries

- ProtocolAi owns application vocabulary, stable protocol-qualified symbol identity, correlated/provenance-bearing events, and actual outcome envelopes.
- GrammarAi owns grammar definitions and structural validation for precondition expressions and other legal compositions.
- CommandAi owns immutable bounded command proposal data and its deterministic codec.
- CiAi owns CI evidence and verdicts. A command outcome contract is not a CI verdict.
- FSM_Foundry and the host own durable observer lifecycle, scheduling, live-state condition evaluation, authorization, approval, dispatch, execution, and outcome lifecycle.

A successful parse is never permission. Carrying an idempotency key does not enforce replay protection. Declaring an outcome contract does not prove the outcome occurred.

## Failure behavior

- Decode with null, empty, or whitespace-only input: argument exception because the caller did not supply an envelope.
- Non-empty input with a missing/malformed envelope, unsupported version, invalid JSON, unknown JSON properties, missing fields, invalid identifiers, duplicates, or malformed contract data: format exception.
- Direct model construction with invalid caller-supplied values: argument exceptions.
- Encoding a valid in-memory proposal whose encoded representation exceeds CommandLimits.MaxEnvelopeCharacters: argument exception.

The decoder normalizes model-construction failures caused by malformed wire content into FormatException. Callers should treat any parse failure as a rejected proposal and avoid fallback paths that silently reinterpret malformed data.

## Wire contract

CommandAi/2 is the current and only supported wire contract. CommandAi/1 is intentionally not accepted; the earlier package was not externally released, and the expanded model is a deliberate breaking change.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
