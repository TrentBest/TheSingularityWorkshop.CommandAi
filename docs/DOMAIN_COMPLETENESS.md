<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi domain completeness and ownership

> Status: the owner-selected target is an additive enriched CommandAi/1 proposal contract. The compact Command entry remains supported; enriched AtomicCommand entries are explicit and must be understood by the reader. This page describes the intended combined contract, not an assertion that the current permanent development head has passed its final exact-head verification.

## 01 — What does CommandAi own?

CommandAi owns provider-neutral, immutable representations and structural validation:

- CommandProposal — a non-empty, ordered proposal containing compact and/or enriched instructions.
- CommandInstruction — either the original compact action-plus-arguments instruction or a complete enriched atomic-command declaration.
- CommandArgument, CommandValue, and CommandValueKind — ordered typed parameter values.
- CommandOutcomeContract and CommandOutcomeField — the expected shape of a result, never an actual result.
- CommandPrecondition — an opaque grammar-version token and expression; no parsing or evaluation occurs here.
- CommandGrammarBody — bounded, canonical Base64 transport for opaque GrammarAi wire bytes.
- CommandLimits — resource bounds for proposal, command, literal, precondition, outcome, and grammar-body data.
- CommandAiTextCodec — the CommandAi/1 envelope encoder and decoder.
- CommandProposalBuilder and CommandInstructionBuilder — mutable authoring helpers that produce immutable snapshots.

The existing address-space exchange and response model remains a separate representation surface. Its correlation and structural checks do not turn a proposal or response into an authorization or execution receipt.

## 02 — Why is the boundary deliberately narrow?

A model can describe an action without knowing what it means or whether it is allowed. A host needs a predictable representation but must retain trusted vocabulary mappings, current-state checks, policy, authorization, approval, replay/deduplication enforcement, dispatch, execution, and actual outcome recording.

CommandAi therefore owns representation and structural validation, not application semantics or authority. Positive integer identities are opaque identifiers, not pointers, capabilities, or authorization tokens. A successfully decoded proposal remains untrusted intent.

## 03 — Compact and enriched forms

Compact commands retain the established action-and-arguments representation:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=42 201="diagnostic stream"
[/CommandAi]
~~~

Enriched commands are represented by an explicit AtomicCommand entry containing a bounded JSON record. The enriched record adds:

- A unique command identity and the existing application-owned action identity.
- Typed target and scope values.
- A caller-supplied idempotency identity.
- An expected-outcome identity and typed field declarations.
- Zero or more opaque grammar-versioned preconditions.
- An optional opaque GrammarAi body with a positive grammar wire version and canonical Base64 payload.

Both forms may coexist in one proposal and preserve their declared order. Compact commands do not receive inferred defaults for enriched fields. A compact-only reader does not understand AtomicCommand and must reject that entry; it must be upgraded before it consumes an enriched proposal. This is explicit compatibility behavior, not silent downgrade.

## 04 — How are responsibilities divided?

| Concern | Owner |
|---|---|
| Shared vocabulary identity and broader causal/event/outcome provenance | ProtocolAi |
| Grammar definitions, grammar parsing, and structural validation | GrammarAi |
| Immutable proposal model, compact/enriched representations, builders, bounds, CommandAi/1 codec | CommandAi core |
| Endpoint communication | HeadlessAi |
| CI evidence and verdicts | CiAi |
| Trusted mappings, live-state evaluation, policy, authorization, approval, replay/deduplication enforcement, dispatch, execution | FSM_Foundry / consuming host |
| MicroBundle identity, dependencies, load lifecycle, composition participation | MicroBundleDomain |
| Artifact discovery, storage, retrieval, verification, materialization | MicroBundleRepository |
| Dependency resolution and composition orchestration | FSM_COS or another composition host |

CommandAi has no runtime dependency on GrammarAi, ProtocolAi, HeadlessAi, FSM_COS, or MicroBundleRepository. Grammar version tokens and body bytes remain opaque here. GrammarAi owns grammar meaning and structural validation; the trusted host owns evaluation against live state.

### Optional MicroBundle distribution

The optional TheSingularityWorkshop.CommandAi.MicroBundle package implements IMicroBundle from TheSingularityWorkshop.MicroBundleDomain 1.0.1. It declares provider commandai.proposal and delegates Encode/Decode to the core codec.

The package split is intentional:

- The core TheSingularityWorkshop.CommandAi package remains dependency-free for ordinary library consumers.
- The optional adapter depends on the core package and the neutral MicroBundleDomain contract.
- Neither package depends on FSM_COS or MicroBundleRepository.
- The adapter's Load lifecycle is stateless; its Arbitrate method reports no composition mutation. Neither method executes a proposal.

## 05 — Resource bounds

The current source constants define these limits:

| Limit | Value |
|---|---:|
| Encoded envelope | 262,144 UTF-16 characters |
| Commands per proposal | 256 |
| Arguments per command | 64 |
| Preconditions per command | 32 |
| Outcome fields per contract | 64 |
| One literal value | 16,384 UTF-16 characters |
| Aggregate literal content per proposal | 1,048,576 UTF-16 characters |
| Grammar-version token | 128 characters |
| One precondition expression | 16,384 characters |
| One decoded GrammarAi body | 32 KiB |
| Aggregate decoded GrammarAi bodies per proposal | 128 KiB |
| Idempotency identity | 256 characters |

The aggregate in-memory literal allowance and encoded envelope allowance are independent safeguards. A constructible proposal can still be too large to encode. The per-command and aggregate GrammarAi body bounds are measured after Base64 decoding; the wire envelope remains bounded separately.

## 06 — What enriched declarations do not guarantee

- A command identity is correlation data, not authorization.
- An idempotency identity is a caller-supplied key, not an exactly-once or deduplication guarantee. The host must enforce its policy.
- A target and scope are opaque typed values, not resolved resources or access grants.
- A precondition is not evaluated by CommandAi. GrammarAi owns grammar structure; the host evaluates it against trusted live state.
- An expected-outcome contract describes a shape. It is not an actual result, success signal, CI verdict, or proof of execution.
- A GrammarAi body is transported without interpretation. CommandAi validates version positivity, canonical Base64, and decoded-size bounds only.
- Proposal ordering does not imply transactionality, rollback, retry, cancellation, or all-or-nothing execution.

## 07 — Verification and release boundary

The earlier exact-head [CommandAi CI run 43683852447](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/43683852447) verified the compact development package, both package boundaries, quickstarts, and package-only consumers. It predates the enriched contract and is not evidence for the enriched implementation.

The enriched source, adversarial tests, documentation, both package audits, and exact-head workflow must be verified together on the final candidate commit. Until that evidence exists, describe the integration as in progress. NuGet publication remains disabled and requires separate explicit owner authorization.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>