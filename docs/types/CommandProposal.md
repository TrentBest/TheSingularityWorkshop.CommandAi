<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposal

An immutable, non-empty ordered collection of `CommandInstruction` records plus the protocol version.

## Contract

- CurrentProtocolVersion is `CommandAi/1`.
- Commands preserve insertion order.
- The proposal enforces the source-defined maximum command count.
- Empty proposals and unsupported protocol versions are rejected.

A proposal is an ordered representation, not a transaction or workflow. It does not imply dependency, rollback, retry, or all-or-nothing execution.

Source: [CommandProposal.cs](../../CommandProposal.cs). Related: CommandInstruction, CommandLimits, CommandAiTextCodec.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
