namespace TheSingularityWorkshop.CommandAi;

/// <summary>Opaque GrammarAi wire bytes carried by an enriched atomic command.</summary>
/// <remarks>CommandAi validates transport framing and resource bounds only; GrammarAi owns decoding and structural validation.</remarks>
public sealed class CommandGrammarBody
{
    public CommandGrammarBody(int grammarWireVersion, ReadOnlySpan<byte> payload)
    {
        if (grammarWireVersion <= 0)
            throw new ArgumentOutOfRangeException(nameof(grammarWireVersion), "Grammar wire version must be positive.");
        if (payload.IsEmpty)
            throw new ArgumentException("Grammar body payload cannot be empty.", nameof(payload));
        if (payload.Length > CommandLimits.MaxGrammarBodyBytesPerCommand)
            throw new ArgumentException($"Grammar body cannot exceed {CommandLimits.MaxGrammarBodyBytesPerCommand} decoded bytes.", nameof(payload));
        GrammarWireVersion = grammarWireVersion;
        _payload = payload.ToArray();
    }

    private readonly byte[] _payload;
    public int GrammarWireVersion { get; }
    public ReadOnlyMemory<byte> Payload => _payload;
    public string PayloadBase64 => Convert.ToBase64String(_payload);

    internal static CommandGrammarBody FromBase64(int version, string payloadBase64)
    {
        if (string.IsNullOrEmpty(payloadBase64))
            throw new FormatException("Grammar body Base64 payload cannot be empty.");
        byte[] bytes;
        try { bytes = Convert.FromBase64String(payloadBase64); }
        catch (FormatException ex) { throw new FormatException("Grammar body payload is not valid Base64.", ex); }
        if (!string.Equals(Convert.ToBase64String(bytes), payloadBase64, StringComparison.Ordinal))
            throw new FormatException("Grammar body payload must use canonical Base64.");
        try { return new CommandGrammarBody(version, bytes); }
        catch (ArgumentException ex) { throw new FormatException("Grammar body version or payload is outside the allowed bounds.", ex); }
    }
}
