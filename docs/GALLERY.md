<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi gallery

The console quickstart demonstrates a complete CommandAi/2 proposal with two host-owned operations. Each command has its own identity, action, target, scope, idempotency key, and expected outcome contract. The example prints the encoded proposal, decodes it, and explicitly states that no command was executed.

- [Quickstart source](../examples/CommandAi.QuickStart/Program.cs) — reproduce with the command in [Usage](USAGE.md).
- [Host-owned extensions](../examples/CommandAi.QuickStart/ResourceCommandExtensions.cs) — see how an application adds named operations without expanding generic CommandAi semantics.
- [Wire format and design rationale](THEORY.md) — understand the contract and its limits.

The CommandAi/2 source, tests, quickstart, package packing, packed-artifact audit, and quickstart compilation passed GitHub Actions at development commit [cb17cc3](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commit/cb17cc33a577ce2bdab3afef07cb61db31cb0c8e), run [#41353191987](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41353191987). NuGet publication was skipped. This verification does not prove host authorization, live precondition evaluation, or command execution.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
