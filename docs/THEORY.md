<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi theory: proposals and atomic commands

## 1. Describe intent without granting permission

A command proposal is like a work ticket: it communicates an intended action, but it does not prove that the requester owns the resource, establish that the resource still exists, authorize access, or perform the work.

CommandAi provides a bounded representation for proposed actions and correlated application exchanges. Its model is provider-neutral. It does not know application-specific action meanings, parse GrammarAi definitions, evaluate live state, authorize a caller, or execute a command.

## 2. Two proposal forms, one protocol identity

CommandAi/1 retains the compact Command entry:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=1001 2001=42 2002="resource"
[/CommandAi]
~~~

The enriched AtomicCommand entry is additive. It is a JSON record that explicitly carries command identity, action identity, typed target and scope, idempotency identity, expected-outcome shape, optional grammar-versioned preconditions, and an optional opaque GrammarAi body.

Compact and enriched instructions may coexist in the same ordered proposal. No defaults are inferred for compact instructions. A compact-only reader does not understand AtomicCommand and must reject that entry; readers must be upgraded before consuming enriched proposals. The shared CommandAi/1 label does not make the enriched entry readable by older compact-only implementations.

## 3. Model and invariants

| Type | Responsibility | Key invariant |
|---|---|---|
| CommandProposal | Ordered proposal envelope | Non-empty, bounded, immutable snapshot |
| CommandInstruction | Compact or enriched instruction | Positive action ID; enriched form is complete |
| CommandArgument | Typed parameter | Positive, unique parameter ID per instruction |
| CommandValue | Integer identity or literal text | Valid typed representation and bounds |
| CommandPrecondition | Opaque grammar-versioned condition | Non-empty bounded version token and expression |
| CommandOutcomeContract | Expected result shape | Positive outcome identity and unique bounded fields |
| CommandOutcomeField | Expected field declaration | Positive field identity and supported value kind |
| CommandGrammarBody | Opaque grammar wire bytes | Positive version, canonical Base64, decoded size bounded |
| CommandLimits | Resource safeguards | Independent envelope, count, literal, and body bounds |
| CommandAiTextCodec | Wire boundary | Parses structure only; never authorizes or executes |

CommandProposal rejects duplicate enriched command identities and duplicate enriched idempotency identities within a proposal. Compact instructions remain on their original shape.

## 4. Builders and snapshots

CommandProposalBuilder and CommandInstructionBuilder are mutable, non-thread-safe authoring helpers. Build creates immutable model snapshots. The compact builder accepts an action ID and optional arguments. The enriched builder requires all core enriched declarations up front; it does not synthesize target, scope, idempotency, or outcome values.

The host can wrap these generic builders with named application operations. Those names and mappings belong to the consuming application, not the shared package.

## 5. Grammar boundary

CommandPrecondition preserves the grammar-version identity token and expression as opaque strings. CommandAi does not parse or evaluate either value. GrammarAi owns grammar interpretation and structural validation; the trusted host evaluates validated conditions against current trusted state.

CommandGrammarBody carries opaque bytes for a GrammarAi-defined wire body. CommandAi checks that the grammar wire version is positive, the payload is non-empty, Base64 is valid and canonical, and decoded bytes stay within configured limits. It does not inspect the body schema or infer grammar semantics.

## 6. Expected outcomes are not actual outcomes

CommandOutcomeContract describes an expected outcome identity and its declared fields. Each field has a positive ID, a supported CommandValueKind, and a required/optional flag.

This contract is not an execution result, success signal, CI verdict, or proof of execution. Actual correlated outcomes and broader provenance belong to ProtocolAi and the trusted host. CommandAi does not synthesize an outcome when a command is proposed or decoded.

## 7. Trust boundary and resource limits

| Limit | Value |
|---|---:|
| Encoded envelope | 262,144 UTF-16 characters |
| Commands per proposal | 256 |
| Arguments per command | 64 |
| Preconditions per command | 32 |
| Outcome fields per contract | 64 |
| One literal value | 16,384 UTF-16 characters |
| Aggregate literal content | 1,048,576 UTF-16 characters |
| Grammar-version token | 128 characters |
| One precondition expression | 16,384 characters |
| Decoded GrammarAi body per command | 32 KiB |
| Decoded GrammarAi bodies per proposal | 128 KiB |
| Idempotency identity | 256 characters |

The aggregate in-memory literal allowance is independent of the smaller encoded envelope limit. A model can be constructible but not encodable. Grammar-body bounds are measured on decoded bytes; the complete text envelope is bounded separately.

A proposal is untrusted input. Successful decoding proves only structural representability within configured limits. It does not prove that an action exists, an identifier is meaningful, a target is accessible, a precondition is true, or a caller is authorized.

## 8. Host ownership and execution lifecycle

| Layer | Responsibility |
|---|---|
| ProtocolAi | Shared vocabulary identity, correlation/provenance, actual event/outcome records |
| GrammarAi | Grammar definitions, parsing, and structural validation |
| CommandAi | Immutable compact/enriched proposals, typed values, bounds, builders, /1 representation |
| HeadlessAi | Configured endpoint communication |
| CiAi | CI evidence and verdicts |
| FSM_Foundry / trusted host | Live-state evaluation, policy, authorization, approval, replay/deduplication enforcement, dispatch, execution |

The idempotency identity is only a key supplied in the proposal. The host must define retention and deduplication policy and enforce it against trusted state. Target and scope values remain opaque until the host resolves them. Preconditions are not authorization. An expected-outcome declaration is not an outcome.

Proposal ordering does not imply transactionality, rollback, retries, cancellation, deadlines, or all-or-nothing execution. Those are orchestration and host responsibilities.

## 9. Verification and release boundary

The earlier exact-head [CommandAi CI run 43683852447](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/43683852447) verified the compact development package, both package boundaries, quickstarts, and package-only consumers. It predates the enriched contract and is not evidence for the enriched implementation.

The enriched source, adversarial tests, documentation, both package audits, and exact-head workflow must be verified together on the final candidate commit. NuGet publication remains disabled and requires separate explicit owner authorization.

## 10. Practice

1. Which compatibility guarantee does the compact Command entry preserve, and why must an old reader reject AtomicCommand?
2. Why is an idempotency key not an exactly-once guarantee?
3. Which layer parses a grammar precondition, and which layer evaluates it against live state?
4. Why is an expected-outcome contract not evidence that an action succeeded?
5. Construct a proposal whose decoded GrammarAi body is within the per-command bound but causes the aggregate proposal bound to be exceeded. Which layer should reject it?

## 11. Further reading

- [Problem domain](PROBLEM_DOMAIN.md)
- [Architecture](ARCHITECTURE.md)
- [Usage](USAGE.md)
- [Domain completeness](DOMAIN_COMPLETENESS.md)
- [Governed command lifecycle](GOVERNED_COMMAND_LIFECYCLE.md)
- [Builder integration](BUILDER_INTEGRATION.md)
- [Source and tests](../CommandAiTextCodec.cs)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>