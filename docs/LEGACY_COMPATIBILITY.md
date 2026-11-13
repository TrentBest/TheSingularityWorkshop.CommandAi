<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Legacy CommandAi — historical reference

## Status

This page records observations about earlier implementations and the compact CommandAi/1 wire form. It is not a claim of drop-in interoperability with the old Architect-specific parser.

## Historical source observations

The earlier implementation reviewed at [The / Architect/Workshop/Ai/CommandAi.cs](https://github.com/TrentBest/The/blob/6ad1971f3932654ea58f8dd8dbc0dde530e4e4c6/Architect/Workshop/Ai/CommandAi.cs) mixed generic parsing with application-specific vocabulary and approval/execution concerns. These observations are historical source notes, not a normative wire specification.

## Compact CommandAi/1 compatibility

The original compact Command= line remains supported without changing its serialized form:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=42 201="diagnostic stream"
[/CommandAi]
~~~

Existing compact proposals continue to decode into action identity plus ordered typed arguments. The model does not invent command identity, target, scope, preconditions, idempotency, or outcome defaults for compact instructions. Those fields remain absent and HasAtomicContract is false.

## Enriched CommandAi/1 extension

The enriched atomic-command form uses an explicit AtomicCommand= JSON entry under the same protocol identifier. It carries command identity, action identity, target, scope, idempotency identity, expected-outcome contract, ordered arguments, and optional grammar-versioned preconditions.

A new decoder accepts compact and enriched entries, including mixed proposals. Older compact-only readers reject the unknown AtomicCommand= entry; they must be upgraded before consuming enriched commands. Retaining the /1 identifier does not imply that old readers understand new entry types.

Enriched atomic commands may include an optional `body` object containing a positive `grammarWireVersion` and canonical Base64 payload. The Base64 value transports exact opaque GrammarAi wire bytes; it is not a second grammar representation. CommandAi enforces a non-empty payload, 32 KiB decoded bytes per command, and 128 KiB decoded bytes per proposal, but deliberately does not reject unknown positive GrammarAi wire versions or inspect the payload. The trusted consumer resolves the action to its exact GrammarAi definition/schema version and delegates structural validation to GrammarAi. Older enriched readers that do not recognize this optional member may reject body-bearing records and must be upgraded before consuming them.

## Ownership

CommandAi validates bounded representation only. GrammarAi owns precondition grammar structure; the host evaluates conditions against current state. ProtocolAi owns correlated message/event outcomes. The host owns authorization, approval, replay/delivery, and execution. Neither a parsed command nor a declared idempotency key grants authority or guarantees execution semantics.

For current implementation guidance, see the [theory chapter](THEORY.md), [builder integration](BUILDER_INTEGRATION.md), and [usage guide](USAGE.md).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
