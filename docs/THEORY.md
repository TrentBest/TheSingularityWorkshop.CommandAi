<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi theory

## From language to proposal

A model response is data crossing a trust boundary. A command representation should make the parts a host must inspect explicit, without pretending that syntax alone makes the response safe.

CommandAi uses three layers: the envelope (protocol version and command entries), the instruction (action identity and ordered parameter/value pairs), and the value (either a positive integer identity or literal text). The codec provides deterministic representation. Meaning is external: the integer 100 does not mean “Navigate” unless a trusted vocabulary defines it that way.

## Why integer identities?

An integer identifier is compact and avoids relying on model-generated spelling for known vocabulary. It is still only an identifier: it can be stale, unknown, incorrectly selected, or inappropriate in context. The host must resolve it against the intended protocol and version.

Literal strings are useful for values that are genuinely open-ended, such as a user-authored name. They are emitted as JSON-quoted text so whitespace, equals signs, and quote escaping do not blur the token boundary. A literal is still untrusted input.

## Why CommandAi does not execute

Parsing answers “is this representation structurally understandable?” It cannot answer whether the action exists in this application, whether the caller is permitted to request it, whether the user intended it, whether preconditions still hold, or whether side effects are acceptable. Those are host policy and application-domain decisions.

CommandAi intentionally has no execution API, application handles, network client, or provider SDK.

## Relationship to the other packages

| Package | Owns | Does not own |
|---|---|---|
| ProtocolAi | Stable vocabulary and symbol identity | Parsing command envelopes or execution |
| GrammarAi | Legal structure/composition of protocol symbols | Symbol meaning or model invocation |
| CommandAi | Versioned command proposal representation and codec | Vocabulary definitions, grammar compilation, authorization, execution |
| HeadlessAi | Configured endpoint request/response boundary | Command meaning or host authority |
| Host application | Policy, approval, dispatch, and side effects | Reusable package-level vocabulary/transport contracts |

These boundaries describe intended ownership. Actual interoperability must be verified against each package's current public API; this initial CommandAi package uses opaque identifiers and has no project references to its siblings.

## Wire format: CommandAi/1

    [CommandAi]
    Protocol=CommandAi/1
    Command=100 200=302 201="Logs.Diagnostic Stream"
    [/CommandAi]

Each Command entry contains one positive action identifier followed by zero or more parameterId=value tokens. A value is either a positive integer identifier or a JSON-quoted string. Each parameter identifier may appear at most once in a command. The codec rejects missing/unsupported versions, malformed identifiers, duplicate parameters, and unknown envelope entries.

## Current limits

- The first codec supports the line-oriented envelope shown above, not arbitrary natural-language interpretation.
- It does not resolve identifiers or validate application-specific argument types.
- It does not claim cryptographic authenticity, replay protection, or authorization.
- It does not yet define negotiation across protocol versions.
- The wire grammar is intentionally small; future changes require explicit compatibility tests and version policy.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
