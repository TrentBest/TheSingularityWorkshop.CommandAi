<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi theory: proposals and atomic commands

## 1. The problem of expressing intent

A model, agent, or application may propose an action, but a proposal is not permission to perform it. The receiver needs a stable, bounded representation of the requested action and its parameters, without confusing the request with the receiver's interpretation, policy, or execution.

CommandAi is the provider-neutral data contract at that boundary. It represents an ordered proposal containing one or more atomic command instructions. The model is immutable after construction and values are deliberately opaque: an integer identity or literal text carries data, not trusted domain meaning.

## 2. What is a command?

A command is an atomic declaration of intended action: an action identity plus ordered, typed arguments. It does not prescribe how the action is implemented, displayed, authorized, scheduled, or executed.

The current CommandAi/1 model intentionally remains compact:
- `CommandProposal`: a non-empty ordered collection of instructions and its protocol version;
- `CommandInstruction`: a positive action identity and ordered arguments;
- `CommandArgument`: a positive parameter identity paired with a typed value;
- `CommandValue`: either a positive integer identifier or bounded literal text.

Target, scope, command GUID, idempotency, precondition expressions, and expected-outcome fields are not part of this approved /1 contract. Do not silently infer them from the action or parameter data.

## 3. Proposal is not execution

A proposal is untrusted input. Decoding proves only that the bytes/text conform to the supported representation and resource limits. It does not prove that an action exists, that an identifier is meaningful, that a request is safe, or that the sender is authorized.

The consuming host resolves identifiers against trusted definitions, validates domain rules and current state, applies authorization and approval policy, and owns dispatch and execution. Actual outcomes and errors are separate records owned by the runtime/protocol layer.

## 4. Atomicity and composition

Each instruction describes one atomic action. A proposal preserves instruction order, but it is not a transaction: CommandAi does not promise all-or-nothing behavior, rollback, dependency scheduling, retries, cancellation, deadlines, or compensation. Those are orchestration concerns and belong to the host or a workflow-specific package.

## 5. Wire representation

The codec reads and writes the line-oriented CommandAi/1 envelope. Encoding uses LF line separators on every platform; decoding accepts LF and CRLF input, trims surrounding whitespace on each line, and ignores blank or whitespace-only lines. The first and last non-empty entries must still be the opening and closing envelope markers. This is bounded by the overall envelope character limit. This stabilizes the text envelope's line endings, but does not define a canonical byte, signature, or digest contract:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=300 201="diagnostic stream"
[/CommandAi]
~~~

The first token is a positive action identifier. Each following token is a positive parameter identifier, an equals sign, and either a positive integer identifier or a JSON-quoted literal. The codec rejects malformed envelopes, unsupported versions, duplicate declarations/parameters, invalid identifiers, malformed quoted literals, and resource-limit violations.

## 6. Ownership across the Workshop

| Layer | Responsibility |
|---|---|
| ProtocolAi | Protocol vocabulary, identity meaning, correlated events and actual outcomes |
| GrammarAi | Grammar definitions and structural validation where a grammar is used |
| CommandAi | Immutable action/argument proposal model, bounds, builders, /1 codec |
| HeadlessAi | Endpoint communication; not command authority |
| CiAi | CI evidence and verdicts; not general command execution |
| FSM_Foundry / host | Live-state checks, policy, authorization, approval, scheduling, dispatch, execution |

The package remains independent of sibling AI packages. A host may add named operations and typed convenience APIs around the generic builder, but those extensions remain host-owned.

## 7. Presentation neutrality

A command's meaning must not prescribe a fixed screen, reading level, age, narration, haptic response, or teaching strategy. Future application layers can project the same semantic intent through different interfaces and accessibility modalities. CommandAi must not depend on UI, rendering, speech, haptics, user profiles, or pedagogy.

## 8. Security and limits

Opaque identifiers are not secrets, credentials, or authorization tokens. Literal text is untrusted and may contain sensitive data; callers must apply their own redaction and storage policy. CommandAi bounds input size and collection counts to limit resource consumption, but these limits do not establish semantic safety.

## 9. Evidence and change policy

The approved contract is CommandAi/1. Internal Workshop consumers are to evolve together; do not introduce /2 or bump package/protocol versions merely to coordinate internal code. Additive or breaking model changes require a concrete domain need, source-backed tests, and an explicit owner decision. No NuGet publication is implied.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
