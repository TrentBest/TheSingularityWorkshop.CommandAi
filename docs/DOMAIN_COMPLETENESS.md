<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Command domain completeness and ownership

> Status: this inventory describes the enriched CommandAi/1 source contract. Compact legacy commands remain supported without invented metadata.

## 1. What a command is

A command is a bounded declaration of intended atomic action: a positive application-owned action identity and ordered typed arguments. An enriched command additionally declares a distinct command identity, target, scope, grammar-versioned preconditions, idempotency identity, and expected-outcome contract. A proposal is a non-empty ordered collection.

A command is not authorization, proof of live-state truth, an execution result, a transaction, a workflow, a retry policy, or a user-interface instruction. A proposal is untrusted input.

## 2. Semantic neutrality

The same action meaning can be presented through different application experiences: visual, textual, narrated, haptic, simplified, or expert-oriented. Those are observer-layer projections. CommandAi must not prescribe age, reading level, modality, teaching strategy, screen, profile, or accessibility behavior. It has no UI or presentation dependencies.

## 3. Source-backed ownership matrix

| Concern | Current /1 evidence | Owner and disposition |
|---|---|---|
| Protocol identity | CommandProposal.CurrentProtocolVersion is CommandAi/1 | CommandAi — implemented |
| Proposal structure | Non-empty ordered collection; compact and enriched entries may coexist | CommandAi — implemented |
| Command identity | Non-empty GUID, distinct from action identity; unique among enriched commands in one proposal | CommandAi — data shape and uniqueness implemented; host owns lifecycle/correlation |
| Action identity | Positive opaque integer | CommandAi — implemented |
| Target and scope | Required typed opaque values for enriched commands; absent for compact legacy commands | CommandAi — bounded representation; consuming domain/host defines meaning and validates current state |
| Parameter identity | Positive opaque integer; duplicate IDs rejected within a command | CommandAi — implemented |
| Parameter value | Positive integer identifier or bounded literal text | CommandAi — implemented |
| Parameter schema/defaults | Not represented | Consuming domain/host — intentionally outside generic command model |
| Preconditions | Bounded expression paired with grammar-version identifier; optional on enriched command | CommandAi carries opaque data; GrammarAi owns grammar and structural validation; host evaluates against live state |
| Idempotency identity | Required bounded string for enriched commands; unique among enriched commands in a proposal | CommandAi carries identity only; host owns replay/deduplication behavior. No exactly-once guarantee |
| Expected outcome | Required outcome identity and bounded unique field declarations for enriched commands | CommandAi declares shape only; ProtocolAi/runtime carries actual correlated outcomes |
| Actual result and errors | Not represented as command intent | ProtocolAi and runtime/host |
| Ordering | Proposal preserves insertion order | CommandAi — representation only; no dependency semantics |
| Transaction, branching, workflow, retry, cancellation, compensation | Not represented or executed | Orchestrator / host |
| Resource bounds | Envelope 262,144; commands 256; arguments/command 64; preconditions/command 32; outcome fields 64; literal 16,384; grammar version 128; precondition expression 16,384; idempotency identity 256; aggregate in-memory literal data 1,048,576; JSON depth 16 | CommandAi — implemented representation safeguards |
| Encoding and decoding | Legacy Command line unchanged; enriched AtomicCommand JSON line; mixed proposals supported; strict duplicate/unknown JSON property rejection | CommandAi — implemented /1 codec |
| Older-reader compatibility | Older compact-only /1 readers reject AtomicCommand entries | Consumer upgrade required before enriched forms are emitted/accepted; not universal forward compatibility |
| Canonical bytes, signatures, hashes | No canonical signing/digest contract | Not implemented |
| Secret classification and redaction | No secret metadata or redaction API | Host/security layer; opaque does not mean secret-safe |
| Authorization and execution | No policy, credential, dispatch, or execution API | Foundry / host — intentionally excluded |
| Presentation, narration, haptics, pedagogy | No presentation model | Application/observer layers — intentionally excluded |

## 4. Implemented guarantees

The model and tests establish positive action/parameter/outcome identities, non-empty command GUIDs, bounded typed values, ordered immutable arguments and commands, unique parameter IDs, unique enriched command and idempotency identities within a proposal, optional grammar-versioned preconditions, and configured resource bounds.

The codec preserves the original compact /1 line, round-trips enriched AtomicCommand entries, supports mixed proposals, and rejects malformed envelopes, unsupported versions, duplicate protocol declarations, duplicate JSON properties, unknown JSON members, invalid typed values, and oversized input.

These are representation guarantees. They do not establish that an ID exists in a host vocabulary, that a precondition is true, that an idempotency key has been enforced, or that execution is safe or authorized.

## 5. Composition boundary

Several ordered commands may be transported together, but order alone does not imply dependency, conditional execution, atomic commit, rollback, or success propagation. A higher-level workflow system owns those semantics. The host validates and authorizes each action against trusted definitions and live state.

## 6. Extension policy

The wire identifier remains CommandAi/1. The compact legacy Command entry remains supported. The enriched AtomicCommand entry is a bounded additive extension. Older readers may reject the enriched entry and must be upgraded before consuming it. Future fields require an identified semantic owner, validation rules, resource bounds, compatibility behavior, and source-backed tests. No publication or release is implied.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
