<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi architecture

## Component flow

    Untrusted model / agent response
                 |
                 v
        CommandAiTextCodec
          decode / validate
                 |
                 v
          CommandProposal
                 |
                 v
       Host-owned semantic checks
                 |
                 v
      Host-owned policy / approval
                 |
                 v
       Host-owned dispatch / execute
                 |
                 v
       Runtime-owned actual outcome

The diagram describes responsibility boundaries, not an automatic runtime pipeline. CommandAi implements only the proposal model, its generic builders, resource bounds, and text codec.

## Public model

- `CommandProposal` is an immutable, non-empty ordered collection of commands.
- `CommandInstruction` identifies an action with a positive integer identity and carries ordered arguments.
- `CommandArgument` pairs a positive parameter identity with a `CommandValue`.
- `CommandValue` represents either a positive integer identifier or literal text.
- `CommandLimits` defines representation bounds.
- `CommandAiTextCodec` encodes and decodes the CommandAi/1 line-oriented envelope.

## Wire contract

CommandAi/1 is the approved and only supported wire contract. A command line has an action identifier followed by zero or more `parameterId=value` tokens. Values are positive integer identifiers or JSON-quoted strings. The encoder emits LF line endings. The decoder accepts LF and CRLF, removes empty lines and trims surrounding whitespace from remaining lines. Whitespace-only lines are ignored as entries but still count toward the parser's maximum-line guard; the opening and closing markers must bound the first and last non-empty entries. It rejects malformed envelopes, unsupported protocol versions, duplicate protocol declarations, duplicate parameters, invalid identifiers, and limit violations. It maps malformed model construction from wire input to `FormatException`; empty caller input remains an argument error.

## Trust boundary

A successful decode means only that the proposal is structurally representable. It is not validation of domain meaning, live-state truth, authorization, safety, or execution. The consuming host owns those decisions and must not use parsing success as an authorization shortcut.

## Package boundaries

- ProtocolAi supplies/owns vocabulary meaning and protocol-level event/outcome contracts.
- GrammarAi owns grammar definitions and parsing/validation for grammar-bearing content when integrated.
- HeadlessAi owns endpoint transport.
- CiAi owns CI evidence and verdicts.
- FSM_Foundry and the host own runtime lifecycle, policy, approval, scheduling, dispatch, execution, and actual outcome handling.

CommandAi has no runtime dependency on these packages. Presentation, accessibility, narration, haptics, and pedagogy belong to observer/application layers, not this package.

## Non-goals

CommandAi does not own application screens, user profiles, command authorization, execution, transactions, retries, cancellation, compensation, persistence, exactly-once delivery, or actual results. Those capabilities must not be inferred from the compact /1 model.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
