namespace TheSingularityWorkshop.CommandAi;

/// <summary>Identifies the representation used by a command argument value.</summary>
public enum CommandValueKind { IntegerIdentifier = 0, LiteralText = 1 }

/// <summary>A typed argument value. Integer identifiers are opaque; this package does not resolve their meaning.</summary>
public sealed record CommandValue
{
    private CommandValue(CommandValueKind kind, int? identifier, string? text) { Kind = kind; Identifier = identifier; Text = text; }
    public CommandValueKind Kind { get; }
    public int? Identifier { get; }
    public string? Text { get; }
    public static CommandValue FromIdentifier(int identifier)
    {
        if (identifier <= 0) throw new ArgumentOutOfRangeException(nameof(identifier), "Identifiers must be positive.");
        return new(CommandValueKind.IntegerIdentifier, identifier, null);
    }
    public static CommandValue FromText(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Literal text cannot be empty or whitespace.", nameof(text));
        return new(CommandValueKind.LiteralText, null, text);
    }
}