<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi problem domain

## The problem

A model or agent can propose a side effect, but free-form text is not a reliable interchange contract. Consuming applications need a bounded, provider-neutral representation that distinguishes which command is proposed, which action vocabulary entry it names, what target and scope it refers to, what preconditions are declared, what arguments are supplied, and what outcome shape is expected.

CommandAi supplies that generic data contract without becoming an application runtime, grammar engine, policy engine, or workflow orchestrator.

## Domain concepts

- **Proposal:** an immutable, ordered, non-empty collection of command instructions.
- **Command identity:** a GUID identifying one proposed command; distinct from action identity and idempotency identity.
- **Action identity:** a positive opaque integer identifying an action in a consuming application's vocabulary.
- **Target and scope:** typed opaque values identifying the intended object/resource and the boundary within which the action is proposed.
- **Parameter:** an ordered pair of positive parameter identity and typed value.
- **Precondition:** a grammar-versioned opaque expression. GrammarAi owns its grammar; the host evaluates it against current trusted state.
- **Idempotency identity:** a caller-supplied bounded key the host may use to recognize repeated handling. The key alone does not implement replay protection.
- **Expected-outcome contract:** an opaque declaration of expected outcome shape; not an actual result, verdict, or proof of execution.
- **Protocol:** the CommandAi/1 wire contract, supporting both legacy compact and enriched atomic-command entries.

A proposal is not a transaction. Order is represented, but dependencies, all-or-nothing execution, rollback, retries, cancellation, compensation, and partial-failure policy remain orchestration concerns.

## Responsibility boundary

CommandAi owns immutable proposal/command data, builders, bounds, and the CommandAi/1 codec. It validates bounded representation only. It does not interpret domain meaning, parse or evaluate preconditions, guarantee idempotent handling, authorize, approve, dispatch, execute, or report actual outcomes.

ProtocolAi owns event/outcome envelopes and correlation/provenance. GrammarAi owns grammar definitions and structural validation. CiAi owns CI evidence and verdicts. Foundry/host owns live-state evaluation, policy, authorization, approval, replay/delivery, scheduling, dispatch, execution, and actual outcome handling.

## Acceptance criteria

1. Proposals contain at least one command and preserve command order.
2. Compact commands retain positive action and parameter identities with ordered typed arguments.
3. Enriched commands require distinct command identity, action identity, target, scope, idempotency identity, and expected-outcome contract.
4. Preconditions carry a non-empty grammar version and bounded opaque expression; CommandAi does not evaluate them.
5. Command and idempotency identities are unique among enriched commands in one proposal.
6. The codec round-trips compact and enriched /1 forms, including mixed proposals.
7. Legacy compact encoding remains byte-for-byte stable.
8. Unknown/duplicate JSON properties, malformed data, unsupported versions, and over-limit input are rejected.
9. Old compact-only readers may reject enriched AtomicCommand= entries; this compatibility limitation is explicit.
10. No API grants authority, resolves application meaning, or executes commands.
11. The package remains independent of UI and sibling runtime packages.

## Verification

Implementation and exact-head CI evidence are tracked in [REFLECTION](REFLECTION.md). Package publication is not authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
