using TheSingularityWorkshop.CommandAi;

/// <summary>
/// Example-only host extensions. Names, identities, targets, scopes, outcome contracts,
/// and semantic checks belong to the consuming application.
/// </summary>
internal static class ResourceCommandExtensions
{
    private const ulong InspectResourceAction = 1001;
    private const ulong OpenDiagnosticStreamAction = 1002;

    public static CommandProposalBuilder InspectResource(
        this CommandProposalBuilder builder, ulong resourceId)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentOutOfRangeException.ThrowIfZero(resourceId);

        return builder.AddCommand(
            commandId: Guid.NewGuid(),
            actionId: InspectResourceAction,
            target: CommandValue.FromIdentifier(resourceId),
            scope: CommandValue.FromText("resource"),
            idempotencyKey: $"inspect-resource:{resourceId}",
            outcomeContract: new CommandOutcomeContract(3001, [
                new CommandOutcomeField(3002, CommandValueKind.LiteralText)
            ]));
    }

    public static CommandProposalBuilder OpenDiagnosticStream(
        this CommandProposalBuilder builder, string streamName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(streamName);

        return builder.AddCommand(
            commandId: Guid.NewGuid(),
            actionId: OpenDiagnosticStreamAction,
            target: CommandValue.FromText(streamName),
            scope: CommandValue.FromText("diagnostic-stream"),
            idempotencyKey: $"open-diagnostic-stream:{streamName}",
            outcomeContract: new CommandOutcomeContract(3003, [
                new CommandOutcomeField(3004, CommandValueKind.LiteralText)
            ]));
    }
}
