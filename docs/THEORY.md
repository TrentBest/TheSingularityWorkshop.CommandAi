<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi theory

## A command is a complete contract

A command proposal is more than an action name plus arguments. For a host to reason about a request, the atomic instruction must explicitly identify:

1. **Identity** — which proposed command is this?
2. **Action** — which application-owned operation is requested?
3. **Target** — which object or resource is acted upon?
4. **Scope** — what boundary limits the operation?
5. **Parameters** — what typed values configure the operation?
6. **Preconditions** — which grammar-defined conditions must hold?
7. **Idempotency** — how can duplicate delivery of the same request be recognized?
8. **Outcome contract** — what shape of result is expected?

CommandAi/2 models all eight concerns. It deliberately does not interpret the values. The target and scope are CommandValue instances; preconditions are opaque expressions tagged with a GrammarAi version; the outcome contract declares an opaque outcome identity and typed fields.

## Identity is not idempotency

A command identity distinguishes one proposed instruction from other instructions. An idempotency key lets a host recognize repeated delivery or retries of the same logical request. They are separate because a newly proposed command can target the same resource and action while still representing a distinct intent, and a retried delivery can repeat the same idempotency key.

CommandAi validates that command IDs and idempotency keys are unique within a single proposal. It does not persist keys, coordinate across processes, or guarantee exactly-once effects. The host owns that lifecycle.

## Preconditions are not evaluation

A precondition expression is stored with the grammar version that defines its structure. CommandAi bounds and transports the expression but does not parse it, decide whether it is true, or inspect live application state. GrammarAi owns the grammar and structural validity; the host evaluates the accepted expression against current state at the appropriate point in the workflow.

This separation avoids coupling the generic command package to a specific grammar implementation or application domain.

## Expected outcome is not actual outcome

CommandOutcomeContract declares an outcome identity and the fields expected in a conforming result. It is attached to the proposed command so the consumer knows what kind of result the command promises to produce.

The contract is not the result itself. Actual execution events, correlation, provenance, replay metadata, and outcome records belong to ProtocolAi and the host runtime. CI evidence and pass/fail verdicts remain CiAi domain. A proposal cannot assert that its expected outcome has occurred.

## Why integer identities and literal text?

An integer identity is compact and avoids relying on model-generated spelling for known vocabulary. It is still opaque: it can be stale, unknown, or inappropriate in context. ProtocolAi and the consuming application own the mapping.

Literal text carries open-ended values explicitly as data. JSON encoding keeps quotes, whitespace, and backslashes unambiguous. Literal text is still untrusted input and remains bounded.

## Wire format: CommandAi/2

The envelope is line-oriented. Each command line contains a JSON object with explicit fields:

~~~text
[CommandAi]
Protocol=CommandAi/2
Command={"commandId":"...","actionId":100,"target":{"kind":"IntegerIdentifier","identifier":300,"text":null},"scope":{"kind":"LiteralText","identifier":null,"text":"workspace"},"idempotencyKey":"inspect:resource:42","outcomeContract":{"outcomeId":400,"fields":[{"fieldId":401,"valueKind":"LiteralText","required":true}]},"arguments":[],"preconditions":[]}
[/CommandAi]
~~~

The codec rejects unsupported versions, malformed JSON, unknown JSON properties, missing required contract fields, invalid value-kind combinations, invalid identifiers, duplicates, and resource-limit violations. The old CommandAi/1 token format is intentionally not accepted; the package has not been externally released and this is a breaking contract correction.

## Package ownership

| Package | Owns | Does not own |
|---|---|---|
| ProtocolAi | Vocabulary, protocol-qualified identity, correlated/provenance-bearing events and actual outcomes | Command representation, authorization, execution |
| GrammarAi | Grammar definitions, productions, structural validation | Vocabulary meaning, live-state truth, authorization |
| CommandAi | Complete atomic command representation and deterministic codec | Domain meaning, condition evaluation, idempotency enforcement, actual outcomes, authorization, execution |
| HeadlessAi | Configured endpoint request/response communication | Command authority or host policy |
| CiAi | CI evidence and verdicts | Command execution or general application authorization |
| FSM_Foundry / host | Durable observer lifecycle, scheduling, live-state evaluation, authorization, approval, dispatch, execution | Reusable provider-neutral command representation |

## Limits and non-goals

- CommandAi does not negotiate versions or translate CommandAi/1.
- It does not authenticate the sender or cryptographically bind the proposal.
- It does not guarantee replay protection or exactly-once execution.
- It does not parse natural language or evaluate preconditions.
- It does not execute commands, generate CI verdicts, or treat an expected outcome as a completed outcome.
- It has no runtime dependency on the other AI packages.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
