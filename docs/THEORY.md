<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi theory: proposals and atomic commands

## 1. Intuition: describe the action without granting permission

Imagine writing an instruction on a work ticket: “inspect resource 42 and return its diagnostic stream.” The ticket communicates intent. It does not prove the requester owns the resource, establish that the resource still exists, authorize access, or perform the inspection.

CommandAi is the structured work-ticket boundary for software. It turns a proposed action into bounded, typed data that a receiving application can inspect. The analogy stops at representation: CommandAi does not own the resource, know the meaning of the identifiers, or operate the machinery that performs the work.

This distinction matters whenever a model, agent, service, or application proposes side effects. A receiver needs a predictable representation, but the authority to interpret and execute that representation must remain with the trusted host.

## 2. Concepts and vocabulary

- **Command identity:** a non-empty GUID identifying one proposed command, distinct from action identity and idempotency identity.
- **Action identity:** a positive opaque integer identifying an action in a vocabulary owned by the consuming application.
- **Target and scope:** typed opaque values identifying the intended resource and the boundary in which the action is proposed.
- **Parameter identity:** a positive opaque integer identifying a parameter in that same application-owned vocabulary.
- **Argument value:** either another positive integer identifier or literal text.
- **Precondition:** an opaque expression paired with the GrammarAi version required to interpret it.
- **Idempotency identity:** a bounded caller-supplied key that the host may use to recognize repeated handling; it is not a replay guarantee.
- **Expected-outcome contract:** an opaque declaration of expected outcome fields, not an actual result or verdict.
- **Proposal:** an immutable, non-empty ordered collection of compact or enriched command instructions.
- **Protocol version:** the explicit representation contract. The approved and only supported version remains `CommandAi/1`.

An identifier is not a name. The integer `1001` has no universal meaning; the host maps it to a trusted action definition. Likewise, the order of arguments is preserved, but the generic package does not know whether a parameter is a resource, a mode, a path, or a diagnostic selector.

A proposal is not a transaction, workflow, authorization token, result, or UI instruction.

## 3. The model and its invariants

The public model is deliberately small:

| Type | Responsibility | Important invariant |
|---|---|---|
| `CommandProposal` | Proposal envelope | At least one command; at most the configured command limit; immutable collection snapshot |
| `CommandInstruction` | Compact or enriched command declaration | Positive action ID and ordered immutable arguments; enriched form additionally requires command ID, target, scope, idempotency identity, outcome contract, and optional bounded preconditions |
| `CommandArgument` | One named parameter value | Positive parameter ID and non-null typed value |
| `CommandValue` | Typed target, scope, and argument representation | Positive integer identifier or non-empty bounded literal text |
| `CommandPrecondition` | Grammar-versioned opaque condition | Required bounded grammar version and expression; no parsing/evaluation in CommandAi |
| `CommandOutcomeContract` / `CommandOutcomeField` | Expected outcome shape | Positive unique identities and bounded field count; not actual results |
| `CommandLimits` | Resource safeguards | Bounds for wire characters, commands, arguments, preconditions, outcome fields, identities, and literal text |
| `CommandAiTextCodec` | Text serialization boundary | Validates the /1 envelope; does not resolve meaning or grant authority |

Within one instruction, parameter IDs are unique. Proposal order and argument order are preserved. Builders are mutable authoring helpers; `Build()` creates immutable model snapshots. The builders are not thread-safe and do not perform application-specific semantic validation.

The proposal's ordering is only a representation guarantee. It does not imply that command two depends on command one, that all commands succeed together, or that a failure rolls back earlier side effects. If an application needs a dependency graph, conditional branches, retries, cancellation, deadlines, compensation, or partial-failure policy, that belongs in its orchestration layer.

## 4. Implementation mapping

The implementation is dependency-free and remains separate from adjacent Workshop packages.

- [`CommandProposal.cs`](../CommandProposal.cs) owns the non-empty bounded collection and protocol identity.
- [`CommandInstruction.cs`](../CommandInstruction.cs) owns command/action identity, target, scope, idempotency identity, expected-outcome contract, and ordered arguments.
- [`CommandArgument.cs`](../CommandArgument.cs) pairs parameter identity with a value.
- [`CommandValue.cs`](../CommandValue.cs) distinguishes integer identifiers from literal text.
- [`CommandPrecondition.cs`](../CommandPrecondition.cs) carries opaque grammar-versioned precondition data.
- [`CommandOutcomeContract.cs`](../CommandOutcomeContract.cs) and [`CommandOutcomeField.cs`](../CommandOutcomeField.cs) declare expected outcome shape.
- [`CommandLimits.cs`](../CommandLimits.cs) centralizes representation bounds.
- [`CommandAiTextCodec.cs`](../CommandAiTextCodec.cs) encodes and decodes the line-oriented /1 envelope.
- [`CommandAiTextCodecTests.cs`](../tests/CommandAi.Tests/CommandAiTextCodecTests.cs) exercises round trips, ordering, malformed input, whitespace handling, and resource boundaries.

The builder API is convenience, not a second semantic model. A consuming domain can wrap it with named operations such as `InspectResource(...)`, but the names, mappings, and validation remain in that consuming domain. CommandAi itself must not accumulate application-specific action catalogs.

## 5. Wire representation and parsing

The encoder emits LF line separators on every platform. The decoder accepts LF and CRLF. It removes empty lines and trims surrounding whitespace from each remaining line. Whitespace-only lines become empty entries and are ignored semantically, but still count toward the parser's maximum-line guard. The opening and closing markers must still bound the first and last non-empty entries. Non-empty unknown entries, nested envelopes, duplicate protocol declarations, duplicate parameter IDs, unsupported versions, malformed identifiers, malformed JSON literals, and oversized input are rejected.

The legacy compact wire shape remains unchanged:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=1001 2001=42 2002="resource"
Command=1002 2003="Diagnostics"
[/CommandAi]
~~~

An enriched atomic command uses an explicit JSON entry within the same protocol version:

~~~text
[CommandAi]
Protocol=CommandAi/1
AtomicCommand={"commandId":"11111111-2222-3333-4444-555555555555","actionId":1001,"target":{"kind":"LiteralText","identifier":null,"text":"repository"},"scope":{"kind":"LiteralText","identifier":null,"text":"feature/ref"},"idempotencyKey":"operation-001","outcomeContract":{"outcomeId":700,"fields":[]},"arguments":[],"preconditions":[]}
[/CommandAi]
~~~

The codec accepts compact and enriched entries in one ordered proposal. New readers decode both. Older compact-only readers reject the unknown AtomicCommand entry; they must be upgraded before consuming enriched commands. Retaining the /1 identifier does not promise universal forward compatibility. Enriched JSON rejects unknown members, duplicate properties, invalid field combinations, and nesting deeper than 16.

A compact Command line begins with a positive action identifier. Each subsequent token is a positive parameter identifier, an equals sign, and either a positive integer identifier or a JSON-quoted string.

LF output makes line endings deterministic, but this does not establish canonical bytes, canonical JSON rules beyond the current serializer behavior, signatures, hashes, or a digest contract. Those must not be inferred from deterministic line separators.

## 6. Worked example: propose, encode, inspect

The quickstart builds two proposed actions using opaque identities:

~~~csharp
var proposal = new CommandProposalBuilder()
    .AddCommand(1001, command => command
        .WithIdentifier(parameterId: 2001, valueId: 42)
        .WithText(parameterId: 2002, value: "resource"))
    .AddCommand(1002, command => command
        .WithText(parameterId: 2003, value: "Diagnostics"))
    .Build();

var encoded = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(encoded);
~~~

The resulting envelope contains the two commands in the same order. The first argument value is an integer identifier; the second is literal text. A receiver can decode the envelope and then pass the proposal to its own trusted resolver and policy checks.

The example deliberately stops before execution. It does not show a universal action mapping because none exists in this package. In a real application, the host must know what action IDs 1001 and 1002 mean, what each parameter means, whether the values are valid for those parameters, whether the target is in scope, and whether the caller is authorized.

See the [runnable quickstart](../examples/CommandAi.QuickStart/Program.cs) for the complete executable sample.

## 7. Trust boundary, resource limits, and failure behavior

A proposal is untrusted input. Successful decoding proves only structural representability within configured limits. It does not prove that an action exists, that an identifier is meaningful, that the request is safe, that a resource is still available, or that the sender is authorized.

The current limits are measured in UTF-16 characters unless otherwise stated:

| Limit | Value |
|---|---:|
| Encoded envelope | 262,144 |
| Commands per proposal | 256 |
| Arguments per command | 64 |
| Preconditions per command | 32 |
| Outcome fields per contract | 64 |
| One literal/target/scope value | 16,384 |
| Grammar-version identifier | 128 |
| Precondition expression | 16,384 |
| Idempotency identity | 256 |
| Aggregate in-memory literal data | 1,048,576 |
| Enriched JSON depth | 16 |

The model aggregate literal limit is currently independent of the smaller transport envelope limit. The recommendation to retain that separation remains open for owner review in issue #3. A proposal may be valid as in-memory data and still be too large to encode. The model bound limits aggregate literal storage; the codec bound limits the serialized transport representation. This is not a promise that every constructible model has a wire representation.

Failure categories are also distinct:
- Empty caller input is an `ArgumentException`.
- Malformed non-empty wire data and unsupported protocol declarations are `FormatException`.
- Invalid values supplied directly to model constructors/builders are argument-validation errors.
- Encoding a valid in-memory proposal whose serialized envelope exceeds the wire limit is an `ArgumentException`.

Opaque identifiers are not secrets, credentials, or authorization tokens. Literal text may contain sensitive data. Callers must apply their own redaction, logging, storage, and retention policies. CommandAi does not classify secrets or sanitize domain content.

## 8. Ownership across the Workshop

| Layer | Responsibility |
|---|---|
| ProtocolAi | Protocol vocabulary, identity meaning, correlation/provenance, event and outcome envelopes |
| GrammarAi | Grammar definitions and structural validation where a grammar is used |
| CommandAi | Immutable compact/enriched atomic-command model, representation bounds, builders, /1 codec |
| HeadlessAi | Configured endpoint request/response communication; not command authority |
| CiAi | CI evidence and verdicts; not general command execution |
| FSM_Foundry / host | Live-state checks, policy, authorization, approval, scheduling, dispatch, execution, and lifecycle |

CommandAi has no runtime dependency on these sibling packages. The host resolves IDs against trusted definitions, validates domain rules and current state, applies authorization and approval policy, and owns dispatch and execution. Actual outcomes and errors are separate runtime/protocol records.

## 9. Evidence, limits, and open questions

The exact-head [CommandAi CI run 41752954762](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41752954762) passed on commit `2b488e1d25f746322d411b16fbcf088bd88d6b38`: branding, restore/build, tests with coverage, Codecov, quickstart, package pack, packed-artifact audit, and consumer compilation. NuGet publication was skipped.

Subsequent regression tests add explicit coverage for escaped control characters and blank/whitespace-only lines. Their own exact-head CI is tracked in [REFLECTION](REFLECTION.md); do not treat the earlier run as evidence for later commits.

The enriched /1 contract now represents command identity, target/scope, grammar-versioned preconditions, idempotency identity, and expected-outcome shape. It does not interpret or enforce those values. Proposal-level correlation, parameter schemas/defaults, actual results, canonical signing/digests, workflow semantics, secret classification, authorization, and execution remain outside CommandAi. A declared idempotency identity does not implement replay protection; a declared precondition does not prove it true; an expected-outcome contract is not an actual outcome.

## 10. Practice and assessment

1. If a proposal contains two commands, what does the preserved order guarantee—and what does it not guarantee?
2. Why is a positive action ID insufficient to authorize an operation?
3. Construct a literal containing a newline, a tab, a quote, and a backslash. Predict its encoded form, then round-trip it through the codec.
4. Build a proposal that is below the aggregate in-memory literal limit but above the encoded envelope limit. Which operation should succeed, and which should fail?
5. Identify which layer carries a command idempotency identity, which layer enforces replay/deduplication, where target meaning comes from, and which layer records the actual execution result. Explain why carrying a field does not transfer semantic or runtime ownership.

For an implementation exercise, add a test for a malformed token or a boundary condition before changing the codec. State the invariant the test protects and the exact CI revision that verifies it.

## 11. Further reading

- [Problem domain](PROBLEM_DOMAIN.md) — the user problem and package scope.
- [Architecture](ARCHITECTURE.md) — component and trust boundaries.
- [Usage](USAGE.md) — API examples, failures, and verification commands.
- [Domain completeness matrix](DOMAIN_COMPLETENESS.md) — concept ownership and explicit exclusions.
- [Builder integration](BUILDER_INTEGRATION.md) — host-owned named operations.
- [CommandAiTextCodec UML](UML/CommandAiTextCodecUML.md) — parser neighborhood and use cases.
- [Source and tests](../CommandAiTextCodec.cs) — implementation details.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
