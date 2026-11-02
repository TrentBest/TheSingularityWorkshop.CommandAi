<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Command domain completeness and ownership

> Status: this inventory describes the approved CommandAi/1 source contract. Candidate features are not treated as implemented merely because they appeared in an earlier design.

## 1. What a command is

A command is a bounded declaration of an intended atomic action: an action identity plus ordered, typed parameters. A proposal is a non-empty ordered collection of such commands.

A command is not authorization, proof of live-state truth, an execution result, a transaction, a workflow, a retry policy, or a user-interface instruction. A proposal is untrusted input.

## 2. Semantic neutrality

The same action meaning can be presented through different application experiences: visual, textual, narrated, haptic, simplified, or expert-oriented. Those are observer-layer projections. CommandAi must not prescribe age, reading level, modality, teaching strategy, screen, profile, or accessibility behavior. It has no UI or presentation dependencies.

## 3. Source-backed ownership matrix

| Concern | Current /1 evidence | Owner and disposition |
|---|---|---|
| Protocol identity | `CommandProposal.CurrentProtocolVersion` is `CommandAi/1`; decoder rejects unsupported versions | CommandAi — implemented |
| Proposal structure | Non-empty ordered collection of instructions | CommandAi — implemented |
| Action identity | Positive opaque integer | CommandAi — implemented |
| Parameter identity | Positive opaque integer; duplicate parameter IDs rejected within a command | CommandAi — implemented |
| Parameter value | Integer identifier or literal text | CommandAi — implemented |
| Parameter schema, optionality, defaults, constraints | Not represented in /1 | Gap — add only with concrete generic consumer need |
| Target and scope | Not represented in /1 | Host/domain context; not inferred |
| Command/proposal GUID, causal/correlation identity | Not represented in /1 | Protocol/runtime coordination gap |
| Preconditions | Not represented in /1 | Grammar/host concern; no hidden expression field |
| Idempotency and replay | Not represented in /1 | Host/runtime concern |
| Expected outcome contract | Not represented in /1 | Protocol/runtime outcome contract, not CommandAi |
| Actual result and errors | Not represented in /1 | ProtocolAi and runtime |
| Ordering | Proposal preserves insertion order | CommandAi — representation only; no dependency semantics |
| Transaction, branching, workflow, retry, cancellation, compensation | Not represented or executed | Orchestrator / host |
| Resource bounds | Envelope, command, argument, and literal bounds are source-defined | CommandAi — implemented representation safeguards |
| Encoding and decoding | Strict line-oriented /1 envelope and typed values | CommandAi — implemented |
| Canonical bytes, signatures, hashes | No canonical signing/digest contract | Not implemented |
| Unknown extensions | No extension bag; malformed/unknown entries rejected | Strict /1 contract |
| Secret classification and redaction | No secret metadata or redaction API | Host/security layer; opaque does not mean secret-safe |
| Authorization and execution | No policy, credential, dispatch, or execution API | Foundry / host — intentionally excluded |
| Presentation, narration, haptics, pedagogy | No presentation model | Application/observer layers — intentionally excluded |

## 4. Implemented guarantees

The model and tests establish positive identifiers, typed values, ordered arguments and commands, non-empty proposals, immutable snapshots from builders, and configured resource bounds. The codec rejects malformed envelopes, unsupported versions, duplicate protocol declarations, duplicate parameters, invalid values, and oversized input.

These are representation guarantees. They do not establish that an ID exists in a host vocabulary, that a command is semantically correct, or that execution is safe or authorized.

## 5. Composition boundary

Several ordered commands may be transported together, but order alone does not imply dependency, conditional execution, atomic commit, rollback, or success propagation. A higher-level workflow system owns those semantics. The host validates and authorizes each action against trusted definitions and live state.

## 6. Extension policy

Retain CommandAi/1 for internal Workshop coordination. Do not add /2-only fields or increment package/protocol versions merely to align internal consumers. Before adding schema metadata, correlation identity, extensions, or other cross-cutting fields, identify the semantic owner, validation rules, resource bounds, compatibility behavior, and source-backed tests. No publication or release is implied.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
