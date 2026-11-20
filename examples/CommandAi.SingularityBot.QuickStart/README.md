<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi + SingularityBot: LLM proposal quickstart

This sample teaches the **proposal-producing side** of the SingularityBot integration. It uses the real CommandAi/1 codec and a small, sample-only consumer vocabulary check for the existing `SetProductionTarget(Marine, N)` example.

It is deliberately not a second production adapter. The live SingularityBot implementation remains authoritative for BotDefinition resolution, match/race validation, authorization, replay protection, production objectives, jobs, and SC2 dispatch.

## Run it

```bash
dotnet run --project examples/CommandAi.SingularityBot.QuickStart/CommandAi.SingularityBot.QuickStart.csproj
```

The program decodes representative LLM output, validates its shape against the sample vocabulary, and prints the interpreted request. It does not connect to an LLM, GitHub, StarCraft II, or the live bot.

## What the LLM should do

The model may use its available read-only GitHub tools to gather current repository context before proposing an operation. For this integration, the host should provide or let the model inspect the authoritative bot-owned vocabulary and relevant BotDefinition context. GitHub tool results are context, not permission and not proof of live game state.

A suitable system/developer instruction for the model is:

> You are a strategic proposal generator for SingularityBot. Use supplied observations and, when available, read-only GitHub tools to understand the current bot-owned command vocabulary and authored strategy definitions. Do not invent action IDs, parameter IDs, target IDs, race facts, or game-state facts. For a requested production target, emit exactly one bounded CommandAi/1 proposal using the documented action and parameter identities. Use integer identifiers only for catalog identities and literal text for the desired count. If required context is absent, contradictory, stale, or outside the known vocabulary, ask for clarification or decline to propose. Never claim that a proposal was authorized, scheduled, dispatched, or completed. Never call SC2 actions or mutate the repository as a substitute for producing a proposal.

For the current example vocabulary:

| Identity | Value | Meaning |
|---|---:|---|
| Action | 1001 | `SetProductionTarget` |
| Parameter | 2001 | Production target identity |
| Parameter | 2002 | Desired count as positive integer text |
| Target | 1 | Marine in this sample vocabulary |

Example model response:

```text
[CommandAi]
Protocol=CommandAi/1
Command=1001 2001=1 2002="4"
[/CommandAi]
```

## Trust boundary

1. The LLM gathers context and proposes; it does not authorize.
2. `CommandAiTextCodec.Decode` validates the wire representation and generic structural bounds.
3. The consuming bot validates action/parameter/target identities against its active BotDefinition and authoritative match data.
4. The host supplies authorization and enforces replay/idempotency policy.
5. The existing production planner and job lifecycle remain the only production execution path.
6. SC2 controllers remain the only path to protocol actions; observed game state, not model text, establishes outcomes.

The sample reader checks only one known action, its exact two parameters, the Marine target identity, and a positive desired count. It intentionally does not duplicate the live bot's deeper validation or mutate a production objective.

## Tests

The CommandAi test project references this sample and covers valid decoding plus unknown actions, targets, parameters, malformed counts, missing parameters, and multi-command proposals. These tests prove the sample contract only; they are not a substitute for SingularityBot's consumer tests.

## Integration reference

See [SingularityBot's CommandAi integration guide](https://github.com/TrentBest/TheSingularityWorkshop.SingularityBot/blob/development/docs/commandai-integration-v1.md) and [the proposal-to-production runtime proof issue](https://github.com/TrentBest/TheSingularityWorkshop.SingularityBot/issues/16).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
