<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Legacy CommandAi compatibility audit

## Scope

This page compares the current CommandAi package proposal model with the legacy implementation at [The / Architect/Workshop/Ai/CommandAi.cs](https://github.com/TrentBest/The/blob/6ad1971f3932654ea58f8dd8dbc0dde530e4e4c6/Architect/Workshop/Ai/CommandAi.cs) and its adjacent legacy GrammarAi implementation. It is a source review, not a compatibility test.

## What the legacy implementation does

- Uses the CommandAi/1 envelope marker and accepts the Protocol header case-insensitively; the legacy parser permits a missing Protocol header.
- Parses both human-readable commands and compact integer commands.
- Maps integer action IDs through a fixed legacy ProtocolAi action table.
- Supports a limited parameter set in the legacy decoder: Surface and Repository.
- Resolves Surface value IDs through a fixed legacy table and unquotes Repository strings.
- Describes explicit user approval and execution in The as the final safety boundary. The same source file mixes parsing/decoding with an application-specific Command model.

## What the current package does

- Requires an explicit exact Protocol=CommandAi/1 header.
- Encodes/decodes typed command proposals with unsigned 64-bit action and parameter identifiers.
- Represents argument values as either integer identities or JSON-quoted literal text.
- Rejects unknown envelope entries, invalid identifiers, duplicate parameters, missing/unsupported protocol versions, and malformed literals.
- Does not resolve action or parameter meanings, parse the legacy human-readable command syntax, request approval, or execute commands.

## Compatibility conclusion

**The current package is not a drop-in replacement for the legacy CommandAi implementation.** It shares the broad idea of a versioned envelope and compact action/parameter identity notation, but its parsing strictness, typed value representation, supported syntax, and ownership boundary differ.

Do not claim end-to-end interoperability or migrate The's UI to the new package until compatibility fixtures and an explicit migration decision exist. The next implementation step should be a test corpus captured from legacy examples, including optional headers, casing, human-readable commands, Surface mappings, Repository literal escaping, malformed envelopes, and unsupported parameters.

## Recommended migration sequence

1. Freeze representative legacy inputs and their expected decoded meanings in tests.
2. Decide whether legacy human-readable commands remain a compatibility feature or are retired.
3. Define how the generic unsigned identities map to the modern ProtocolAi/GrammarAi definitions; do not copy legacy static tables into CommandAi.
4. Decide whether the package wire format stays strict or offers a separately named legacy-compatible parser.
5. Keep authorization, explicit approval, and execution in the host application.
6. Only then add adapters and claim cross-package compatibility.

## Evidence labels

- **Source-reviewed:** the legacy C# file and current CommandAi source were inspected.
- **Not yet verified:** automated differential tests against a captured legacy corpus.
- **Not claimed:** runtime interoperability between the modern package and The's existing UI.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
