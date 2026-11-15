<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi/1 consumer alignment — no /2 migration

## Owner-selected contract

The Workshop has selected the enriched **CommandAi/1** contract. This page is retained at its historical path so existing links do not break; it is not a proposal to migrate to CommandAi/2.

## Supported proposal shapes

- The original compact `Command=` entry remains supported with its original encoding.
- Enriched `AtomicCommand=` JSON entries add command identity, typed target and scope, idempotency identity, expected-outcome shape, ordered typed arguments, optional GrammarAi-versioned preconditions, and an optional opaque GrammarAi body.
- Compact and enriched entries may coexist in one proposal, preserving their declared order.
- Compact instructions retain absent enriched metadata. Consumers must not infer defaults.

## Compatibility

Older compact-only readers reject `AtomicCommand=` entries. Upgrade consumers before sending enriched proposals; the unchanged `CommandAi/1` identifier does not make older readers forward-compatible. Enriched readers must also support the optional `body` member before consuming body-bearing commands.

## Ownership

CommandAi validates representation and bounds only. GrammarAi owns grammar definitions and structural validation. The trusted host owns live-state evaluation, replay/deduplication enforcement, policy, authorization, approval, dispatch, execution, and actual outcomes.

The NuGet package version remains `0.1.0-alpha.2`. Package publication, master changes, merges, releases, and deployments require explicit owner authorization.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
