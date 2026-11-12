<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandOutcomeContract

An immutable declaration of the expected shape of a command outcome.

## Contract

- OutcomeId is a positive application-owned identity.
- Fields is an ordered immutable collection of outcome-field declarations.
- Field IDs are positive and unique within the contract.
- Field count is bounded by CommandLimits.MaxOutcomeFields.
- Each field declares a supported CommandValueKind and whether it is required.

This is not an actual result, success signal, CI verdict, or proof of execution. ProtocolAi/runtime carries correlated actual outcomes; the host interprets them.

Source: [CommandOutcomeContract.cs](../../CommandOutcomeContract.cs). Related: [CommandOutcomeField](CommandOutcomeField.md), [CommandInstruction](CommandInstruction.md).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
