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

- Empty input: argument exception.
- Missing/malformed envelope, unsupported version, invalid identifiers, malformed values, duplicate parameters, or unknown entries: format exception.
- Empty proposal, invalid identifiers, duplicate parameters, and empty literal text: argument exception at construction boundaries.

Callers should treat any parse failure as a rejected proposal and avoid fallback paths that silently reinterpret malformed data.
