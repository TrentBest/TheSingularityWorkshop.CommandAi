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
      GrammarAi structural validation
                 |
                 v
      Host-owned live-state policy
                 |
                 v
        Host authorization / approval
                 |
                 v
        Host-owned dispatch / execute
                 |
                 v
        Protocol/runtime outcome

The diagram describes responsibility boundaries, not an automatic runtime pipeline. CommandAi implements only the proposal model, generic builders, representation bounds, and text codec. GrammarAi may validate the structure of grammar-versioned preconditions; the host evaluates them against trusted current state.

## Public model

- CommandProposal is an immutable, non-empty ordered collection of compact or enriched commands.
- CommandInstruction always has an action identity and ordered arguments. An enriched instruction additionally has a command GUID, typed target and scope, idempotency identity, preconditions, and expected-outcome contract.
- CommandArgument pairs a positive parameter identity with a CommandValue.
- CommandValue represents either a positive integer identifier or bounded literal text.
- CommandPrecondition carries a GrammarAi version and opaque expression. CommandAi does not parse or evaluate it.
- CommandOutcomeContract and CommandOutcomeField declare the shape expected from a future/runtime outcome. They are not an actual result or success signal.
- CommandLimits defines bounds for every public field and serialized representation.
- CommandAiTextCodec encodes and decodes compact and enriched CommandAi/1 entries.

## Wire contract

The protocol identifier remains CommandAi/1. The original compact line remains byte-for-byte unchanged:

~~~text
Command=100 200=42 201="diagnostic stream"
~~~

The enriched representation uses an explicit AtomicCommand JSON entry. Its required fields are commandId, actionId, target, scope, idempotencyKey, outcomeContract, arguments, and preconditions. The JSON object is bounded, limited to depth 16, rejects unknown members, and rejects duplicate properties. A proposal may contain compact and enriched commands in the same order-preserving envelope.

A new reader decodes both forms. A compact-only older reader rejects the unknown AtomicCommand entry; it must be upgraded before consuming enriched proposals. Retaining the /1 identifier does not promise universal forward compatibility.

The encoder emits LF line endings. The decoder accepts LF and CRLF, removes empty lines, trims remaining lines, and bounds both envelope size and command count. Malformed values, duplicate declarations/identities, unknown entries, invalid JSON, and resource-limit violations are rejected.

## Trust boundary

A successful decode proves only that the proposal is structurally representable within configured bounds. It does not prove that an action exists, that target/scope values are valid, that a precondition is true, that a resource is still available, or that the sender is authorized.

A command GUID identifies a proposed command. An idempotency key is a separate caller-supplied identity that the host may use to enforce idempotent handling. CommandAi does not guarantee deduplication or exactly-once execution. The expected-outcome contract describes a shape; it does not assert that the outcome occurred.

## Package boundaries

- ProtocolAi owns vocabulary identity and interoperable event/outcome envelopes.
- GrammarAi owns grammar definitions and structural validation of grammar-bearing content.
- CommandAi owns generic command proposal data and its codec.
- HeadlessAi owns endpoint transport.
- CiAi owns CI evidence and verdicts.
- FSM_Foundry and the host own runtime lifecycle, live-state validation, policy, authorization, approval, replay/delivery, scheduling, dispatch, execution, and actual outcome handling.

CommandAi has no runtime dependency on these packages. Application-specific action catalogs, parameter meanings, repository rules, and capabilities remain in the consuming domain/host.

## Non-goals

CommandAi does not own application screens, user profiles, command authorization, execution, transactions, retries, cancellation, compensation, persistence, replay ledgers, exactly-once delivery, or actual results. Proposal ordering is not workflow ordering semantics.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
