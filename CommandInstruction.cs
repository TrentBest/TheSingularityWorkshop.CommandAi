using System.Collections.ObjectModel;
namespace TheSingularityWorkshop.CommandAi;

/// <summary>A proposed action and its arguments. It describes intent; it does not execute it.</summary>
public sealed record CommandInstruction
{
    public CommandInstruction(ulong actionId, IEnumerable<CommandArgument>? arguments = null)
    {
        if (actionId <= 0) throw new ArgumentOutOfRangeException(nameof(actionId), "Action identifiers must be positive.");
        ActionId = actionId;
        var items = (arguments ?? []).Take(CommandLimits.MaxArgumentsPerCommand + 1).ToArray();
        if (items.Length > CommandLimits.MaxArgumentsPerCommand)
            throw new ArgumentException($"A command cannot contain more than {CommandLimits.MaxArgumentsPerCommand} arguments.", nameof(arguments));
        if (items.Any(item => item is null)) throw new ArgumentException("Arguments cannot contain null items.", nameof(arguments));
        if (items.Select(item => item.ParameterId).Distinct().Count() != items.Length)
            throw new ArgumentException("A command cannot repeat a parameter identifier.", nameof(arguments));
        Arguments = new ReadOnlyCollection<CommandArgument>(items);
    }
    public ulong ActionId { get; }
    public IReadOnlyList<CommandArgument> Arguments { get; }
}