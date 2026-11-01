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

Each CommandInstruction represents one proposed action with an ordered set of parameter/value pairs. A CommandProposal is an ordered batch of those instructions; the model does not claim that the batch is transactional or that the host must execute every instruction.

The representation deliberately does not define a universal target, scope, precondition language, idempotency key, observer lifecycle, execution status, or outcome. Such fields only become meaningful when a host/domain defines their vocabulary and semantics. They may be represented as host-owned parameters where appropriate; durable event identity, evidence, gate transitions, execution results, retries, and authorization belong to the relevant host and protocol contracts. Do not infer any of those guarantees from a successful parse.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
