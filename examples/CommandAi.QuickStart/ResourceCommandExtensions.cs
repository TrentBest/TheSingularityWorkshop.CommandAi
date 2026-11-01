using TheSingularityWorkshop.CommandAi;

/// <summary>
/// Example-only host extensions. These names, IDs, and semantic checks belong to the
/// consuming application; they are not built-in CommandAi commands.
/// </summary>
internal static class ResourceCommandExtensions
{
    private const ulong InspectResourceAction = 1001;
    private const ulong ResourceIdParameter = 2001;
    private const ulong OpenDiagnosticStreamAction = 1002;
    private const ulong StreamNameParameter = 2002;

    public static CommandProposalBuilder InspectResource(
        this CommandProposalBuilder builder, ulong resourceId)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentOutOfRangeException.ThrowIfZero(resourceId);

        return builder.AddCommand(
            InspectResourceAction,
            command => command.WithIdentifier(ResourceIdParameter, resourceId));
    }

    public static CommandProposalBuilder OpenDiagnosticStream(
        this CommandProposalBuilder builder, string streamName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(streamName);

        return builder.AddCommand(
            OpenDiagnosticStreamAction,
            command => command.WithText(StreamNameParameter, streamName));
    }
}
