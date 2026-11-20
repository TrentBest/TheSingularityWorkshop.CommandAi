using System.Globalization;
using TheSingularityWorkshop.CommandAi;

namespace TheSingularityWorkshop.CommandAi.Samples.SingularityBot;

/// <summary>
/// Demonstrates the consumer-owned vocabulary check for the first SingularityBot
/// CommandAi example. This is not the bot's production adapter or authorization gate.
/// </summary>
public static class BotCommandProposalReader
{
    public const ulong SetProductionTargetActionId = 1001;
    public const ulong ProductionTargetParameterId = 2001;
    public const ulong ProductionDesiredCountParameterId = 2002;
    public const ulong MarineTargetId = 1;

    /// <summary>
    /// Reads one Marine production-target request from a decoded CommandAi proposal.
    /// No game state is read or changed, and no command is authorized or executed.
    /// </summary>
    public static bool TryReadMarineProductionTarget(
        CommandProposal proposal,
        out int desiredCount,
        out string error)
    {
        ArgumentNullException.ThrowIfNull(proposal);
        desiredCount = 0;
        error = string.Empty;

        if (proposal.Commands.Count != 1)
            return Reject("Exactly one command is supported by this sample.", out error);

        var command = proposal.Commands[0];
        if (command.ActionId != SetProductionTargetActionId)
            return Reject("The action is not SetProductionTarget.", out error);

        if (command.Arguments.Count != 2)
            return Reject("SetProductionTarget requires exactly two parameters.", out error);

        var target = command.Arguments.SingleOrDefault(
            argument => argument.ParameterId == ProductionTargetParameterId);
        var count = command.Arguments.SingleOrDefault(
            argument => argument.ParameterId == ProductionDesiredCountParameterId);

        if (target is null || count is null)
            return Reject("A required production parameter is missing or unknown.", out error);

        if (target.Value.Kind != CommandValueKind.IntegerIdentifier ||
            target.Value.Identifier != MarineTargetId)
            return Reject("The target is not the sample's known Marine identity.", out error);

        if (count.Value.Kind != CommandValueKind.LiteralText ||
            !int.TryParse(count.Value.Text, NumberStyles.None, CultureInfo.InvariantCulture, out desiredCount) ||
            desiredCount <= 0)
        {
            desiredCount = 0;
            return Reject("The desired count must be a positive integer encoded as literal text.", out error);
        }

        return true;
    }

    private static bool Reject(string message, out string error)
    {
        error = message;
        return false;
    }
}
