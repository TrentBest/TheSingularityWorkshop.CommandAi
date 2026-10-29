using System.Collections.ObjectModel;
namespace TheSingularityWorkshop.CommandAi;

/// <summary>A proposed action and its arguments. It describes intent; it does not execute it.</summary>
public sealed record CommandInstruction
{
    public CommandInstruction(int actionId, IEnumerable<CommandArgument>? arguments = null)
    {
        if (actionId <= 0) throw new ArgumentOutOfRangeException(nameof(actionId), "Action identifiers must be positive.");
        ActionId = actionId;
        var items = (arguments ?? []).ToArray();
        if (items.Any(item => item is null)) throw new ArgumentException("Arguments cannot contain null items.", nameof(arguments));
        if (items.Select(item => item.ParameterId).Distinct().Count() != items.Length)
            throw new ArgumentException("A command cannot repeat a parameter identifier.", nameof(arguments));
        Arguments = new ReadOnlyCollection<CommandArgument>(items);
    }
    public int ActionId { get; }
    public IReadOnlyList<CommandArgument> Arguments { get; }
}