<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi problem domain

## The problem

A model response that proposes work must carry enough information for a host to identify the requested atomic operation, its target and scope, the parameters it needs, the conditions under which it is valid, the identity used to recognize duplicate delivery, and the shape of the expected result. If these are implicit, each consuming application invents a different contract and critical execution context is lost at the boundary.

CommandAi defines a provider-neutral, immutable, bounded representation for that complete command contract. It carries identities and typed values without assigning them application-specific meaning.

## Domain concepts

- **Proposal** — a versioned ordered batch of one or more atomic command instructions. It is not a transaction.
- **Command identity** — unique identity for one proposed instruction; distinct from the idempotency key.
- **Action identity** — application-owned opaque identity for the operation being proposed.
- **Target** — required opaque typed identity of the object or resource to which the command applies.
- **Scope** — required opaque typed identity delimiting the command's intended scope.
- **Parameter** — an ordered parameter identity paired with an integer identifier or literal text value.
- **Precondition** — an opaque expression tagged with its grammar version. CommandAi stores it; GrammarAi defines structural grammar; the host evaluates it against live state.
- **Idempotency key** — required caller-supplied key used by the host to recognize duplicate delivery. Merely carrying the key does not enforce idempotency.
- **Outcome contract** — expected outcome identity and field declarations. It is a declaration, not an actual execution outcome.
- **Typed value** — either a positive opaque integer identifier or explicitly typed literal text.

## Responsibility boundary

CommandAi owns the complete atomic representation, immutable model, resource limits, and deterministic CommandAi/2 codec. It does not own application vocabulary (ProtocolAi), grammar definitions or structural validation of precondition expressions (GrammarAi), endpoint communication (HeadlessAi), CI evidence/verdicts (CiAi), or durable observer lifecycle, live-state evaluation, policy, authorization, approval, scheduling, dispatch, execution, and actual outcome lifecycle (FSM_Foundry/host and ProtocolAi).

A successful parse is never proof of truth, permission, safety, execution, or success. A declared outcome contract is not a CI verdict. CommandAi has no runtime dependency on its sibling packages.

## Resource boundaries

The codec bounds envelope characters, command count, arguments per command, preconditions per command, outcome fields, individual literal values, grammar-version identifiers, idempotency keys, precondition expressions, and aggregate literal characters. See CommandLimits for the exact source constants. Limits protect representation handling; they do not establish semantic safety.

## Acceptance criteria

1. Each command requires a non-empty unique command identity and a positive action identity.
2. Each command requires explicit target, scope, idempotency key, and expected outcome contract.
3. Parameters are ordered, typed, bounded, and unique by parameter identity.
4. Preconditions are bounded and retain the grammar version without being evaluated by CommandAi.
5. Outcome contracts declare an opaque outcome identity and unique typed fields.
6. Proposal command identities and idempotency keys are unique within a proposal.
7. The CommandAi/2 codec round-trips every field and rejects unsupported versions, malformed JSON, unknown properties, missing fields, and invalid values.
8. No API in this package resolves domain meaning, grants authority, evaluates live conditions, or executes commands.
9. The package remains provider-neutral and has no runtime dependency on ProtocolAi, GrammarAi, HeadlessAi, CiAi, or host frameworks.

## Evidence status

The complete model, codec, builders, focused tests, and quickstart are implemented on development. CI verification of the current head is pending. Alpha consumers should follow the [migration guide](MIGRATION_COMMAND_AI_2.md). NuGet publication is not authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
