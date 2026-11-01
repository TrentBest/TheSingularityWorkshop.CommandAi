<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi builder integration

> **Status: complete generic command representation and fluent construction are implemented.** Domain-specific names, mappings, policy, and execution remain host-owned.

## The layering decision

CommandAi provides the immutable contract and a fluent construction surface. A consuming host adds named operations and validates their domain-specific meaning.

    Application / host
      policy, state, trust, authorization, approval, execution
              |
              v
    Host-owned named command extensions
      operation names, mappings, typed validation, intent
              |
              v
    CommandAi generic builder
      identity, action, target, scope, arguments,
      preconditions, idempotency, expected outcome
              |
              v
    Immutable CommandProposal
              |
              v
    CommandAiTextCodec
      deterministic CommandAi/2 envelope

## Required atomic metadata

The generic builder requires a command identity, action identity, target, scope, idempotency key, and outcome contract. Its optional configure callback adds ordered parameters and grammar-versioned preconditions.

The command identity identifies the proposed instruction. The idempotency key is separately supplied so a host can recognize repeated delivery. The host must persist and enforce the key; CommandAi does not provide exactly-once semantics.

Target and scope use opaque typed CommandValue instances. Preconditions retain a grammar version and expression string. The generic package does not parse or evaluate the expression. Outcome contracts declare an outcome identity and typed fields; they do not contain an actual execution result.

## Example: host-owned named command

The quickstart defines InspectResource and OpenDiagnosticStream as example-only host extensions. Those extensions choose action identities, target/scope representations, idempotency keys, and expected outcome shapes. They are not built-in CommandAi operations.

~~~csharp
var proposal = new CommandProposalBuilder()
    .AddCommand(
        Guid.NewGuid(),
        actionId: 100,
        target: CommandValue.FromIdentifier(300),
        scope: CommandValue.FromText("workspace"),
        idempotencyKey: "inspect:resource:42",
        outcomeContract: new CommandOutcomeContract(400, [
            new CommandOutcomeField(401, CommandValueKind.LiteralText)
        ]),
        configure: command => command
            .WithIdentifier(200, 42)
            .WithPrecondition("GrammarAi/1", "resource.exists"))
    .Build();
~~~

## Extension boundary

A host extension may provide:
1. trusted mappings from named operations to action, target, scope, parameter, and outcome identities;
2. typed convenience methods and domain-specific input validation;
3. grammar-aware authoring and validation where the host explicitly links GrammarAi;
4. diagnostics explaining why a command cannot be represented;
5. host-side idempotency persistence and policy;
6. conversion to CommandProposal for deterministic serialization.

A builder must not treat successful construction or encoding as authorization. The host still resolves identities against trusted definitions, checks current state, evaluates preconditions, applies authorization and approval policy, and decides whether to execute.

## Current implementation versus host responsibilities

| Capability | CommandAi source | Host/domain |
|---|---|---|
| Immutable complete atomic command | Implemented | Consumed |
| CommandAi/2 encode/decode | Implemented | Consumed |
| Generic fluent builder | Implemented | Wrapped as desired |
| Named domain operations | Not built in | Owned |
| Vocabulary and grammar resolution | Not built in | ProtocolAi / GrammarAi integration |
| Live precondition evaluation | Not built in | Owned |
| Idempotency persistence/enforcement | Not built in | Owned |
| Actual outcomes and CI verdicts | Not built in | ProtocolAi / CiAi / host |
| Authorization, approval, execution | Explicitly outside scope | FSM_Foundry / host |

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
