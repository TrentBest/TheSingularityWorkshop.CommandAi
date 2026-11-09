using System.Globalization;
using System.Text;
using System.Text.Json;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>
/// Canonical bounded JSON representation for CommandExchange and CommandResponse.
/// This codec validates structure only; it never resolves addresses or authorizes values.
/// </summary>
public static class CommandExchangeTextCodec
{
    public const int MaxEnvelopeCharacters = 262144;
    private const string Protocol = "CommandAi/1";

    public static string Encode(CommandExchange exchange)
    {
        ArgumentNullException.ThrowIfNull(exchange);
        return EncodeCore("exchange", exchange.ExchangeId, exchange.AddressSpace, "inputs", exchange.Inputs);
    }

    public static string Encode(CommandResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return EncodeCore("response", response.ExchangeId, response.AddressSpace, "outputs", response.Outputs);
    }

    public static CommandExchange DecodeExchange(string text)
    {
        using var document = Parse(text);
        var root = Object(document.RootElement, "envelope", "protocol", "kind", "exchangeId", "addresses", "inputs");
        RequireString(root, "protocol", Protocol);
        RequireString(root, "kind", "exchange");
        var id = ReadGuid(root, "exchangeId");
        var space = ReadAddressSpace(root["addresses"]);
        var values = ReadValues(root["inputs"]);
        try { return new CommandExchange(id, space, values); }
        catch (ArgumentException ex) { throw new FormatException("The exchange contains invalid address values.", ex); }
    }

    public static CommandResponse DecodeResponse(string text)
    {
        using var document = Parse(text);
        var root = Object(document.RootElement, "envelope", "protocol", "kind", "exchangeId", "addresses", "outputs");
        RequireString(root, "protocol", Protocol);
        RequireString(root, "kind", "response");
        var id = ReadGuid(root, "exchangeId");
        var space = ReadAddressSpace(root["addresses"]);
        var values = ReadValues(root["outputs"]);
        try { return new CommandResponse(id, space, values); }
        catch (ArgumentException ex) { throw new FormatException("The response contains invalid address values.", ex); }
    }

    private static string EncodeCore(string kind, Guid id, CommandAddressSpace space, string valueName, IReadOnlyList<CommandAddressValue> values)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false }))
        {
            writer.WriteStartObject();
            writer.WriteString("protocol", Protocol);
            writer.WriteString("kind", kind);
            writer.WriteString("exchangeId", id.ToString("D", CultureInfo.InvariantCulture));
            writer.WritePropertyName("addresses");
            writer.WriteStartArray();
            foreach (var address in space.Addresses)
            {
                writer.WriteStartObject();
                writer.WriteNumber("addressId", address.AddressId);
                writer.WriteString("name", address.Name);
                writer.WriteString("access", AccessName(address.Access));
                writer.WriteString("valueKind", KindName(address.ValueKind));
                writer.WriteBoolean("requiredInput", address.RequiredInput);
                writer.WriteBoolean("requiredOutput", address.RequiredOutput);
                if (address.Description is not null) writer.WriteString("description", address.Description);
                if (address.GrammarDefinitionId.HasValue)
                {
                    writer.WriteNumber("grammarDefinitionId", address.GrammarDefinitionId.Value);
                    writer.WriteNumber("grammarSchemaVersion", address.GrammarSchemaVersion!.Value);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WritePropertyName(valueName);
            writer.WriteStartArray();
            foreach (var item in values)
            {
                writer.WriteStartObject();
                writer.WriteNumber("addressId", item.AddressId);
                writer.WriteString("kind", KindName(item.Value.Kind));
                if (item.Value.Kind == CommandValueKind.IntegerIdentifier)
                    writer.WriteNumber("identifier", item.Value.Identifier!.Value);
                else
                    writer.WriteString("text", item.Value.Text);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        var encoded = Encoding.UTF8.GetString(stream.ToArray());
        if (encoded.Length > MaxEnvelopeCharacters)
            throw new ArgumentException($"Encoded exchange envelope cannot exceed {MaxEnvelopeCharacters} characters.", nameof(values));
        return encoded;
    }

    private static JsonDocument Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("CommandAi exchange text cannot be empty.", nameof(text));
        if (text.Length > MaxEnvelopeCharacters) throw new FormatException($"CommandAi exchange envelope exceeds {MaxEnvelopeCharacters} characters.");
        try
        {
            var document = JsonDocument.Parse(text, new JsonDocumentOptions { MaxDepth = 16, CommentHandling = JsonCommentHandling.Disallow, AllowTrailingCommas = false });
            try
            {
                ValidateUniqueProperties(document.RootElement);
                return document;
            }
            catch { document.Dispose(); throw; }
        }
        catch (JsonException ex) { throw new FormatException("CommandAi exchange envelope is not valid JSON.", ex); }
    }

    private static void ValidateUniqueProperties(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var property in element.EnumerateObject())
            {
                if (!names.Add(property.Name)) throw new FormatException($"Duplicate JSON property '{property.Name}'.");
                ValidateUniqueProperties(property.Value);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
            foreach (var item in element.EnumerateArray()) ValidateUniqueProperties(item);
    }

    private static Dictionary<string, JsonElement> Object(JsonElement element, string label, params string[] allowed)
    {
        if (element.ValueKind != JsonValueKind.Object) throw new FormatException($"{label} must be a JSON object.");
        var result = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        foreach (var property in element.EnumerateObject())
        {
            if (!allowed.Contains(property.Name, StringComparer.Ordinal))
                throw new FormatException($"Unknown {label} property '{property.Name}'.");
            result.Add(property.Name, property.Value);
        }
        foreach (var name in allowed)
            if (!result.ContainsKey(name) && !((label == "address" && name is "description" or "grammarDefinitionId" or "grammarSchemaVersion")))
                throw new FormatException($"Required {label} property '{name}' is missing.");
        return result;
    }

    private static void RequireString(Dictionary<string, JsonElement> obj, string name, string expected)
    {
        if (obj[name].ValueKind != JsonValueKind.String || obj[name].GetString() != expected)
            throw new FormatException($"'{name}' must be '{expected}'.");
    }

    private static Guid ReadGuid(Dictionary<string, JsonElement> obj, string name)
    {
        var value = obj[name].ValueKind == JsonValueKind.String ? obj[name].GetString() : null;
        if (!Guid.TryParseExact(value, "D", out var id) || id == Guid.Empty)
            throw new FormatException($"'{name}' must be a non-empty D-format GUID.");
        return id;
    }

    private static CommandAddressSpace ReadAddressSpace(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Array) throw new FormatException("'addresses' must be an array.");
        var list = new List<CommandAddressDescriptor>();
        foreach (var item in element.EnumerateArray())
        {
            var obj = Object(item, "address", "addressId", "name", "access", "valueKind", "requiredInput", "requiredOutput", "description", "grammarDefinitionId", "grammarSchemaVersion");
            var id = PositiveUInt64(obj["addressId"], "addressId");
            var name = String(obj["name"], "name");
            var access = ParseAccess(String(obj["access"], "access"));
            var kind = ParseKind(String(obj["valueKind"], "valueKind"));
            var requiredInput = Boolean(obj["requiredInput"], "requiredInput");
            var requiredOutput = Boolean(obj["requiredOutput"], "requiredOutput");
            var description = obj.TryGetValue("description", out var d) ? NullableString(d, "description") : null;
            ulong? grammarId = obj.TryGetValue("grammarDefinitionId", out var g) ? PositiveUInt64(g, "grammarDefinitionId") : null;
            uint? grammarVersion = obj.TryGetValue("grammarSchemaVersion", out var v) ? PositiveUInt32(v, "grammarSchemaVersion") : null;
            if (grammarId.HasValue != grammarVersion.HasValue) throw new FormatException("Grammar definition ID and schema version must be supplied together.");
            try { list.Add(new CommandAddressDescriptor(id, name, access, kind, requiredInput, requiredOutput, description, grammarId, grammarVersion)); }
            catch (ArgumentException ex) { throw new FormatException("An address descriptor is invalid.", ex); }
            if (list.Count > CommandAddressSpace.MaxAddresses) throw new FormatException("Address count exceeds the configured limit.");
        }
        try { return new CommandAddressSpace(list); }
        catch (ArgumentException ex) { throw new FormatException("The address space is invalid.", ex); }
    }

    private static CommandAddressValue[] ReadValues(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Array) throw new FormatException("Address values must be an array.");
        var list = new List<CommandAddressValue>();
        foreach (var item in element.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object) throw new FormatException("An address value must be an object.");
            var props = item.EnumerateObject().ToArray();
            var names = props.Select(p => p.Name).ToArray();
            if (names.Distinct(StringComparer.Ordinal).Count() != names.Length) throw new FormatException("An address value contains duplicate properties.");
            var kindProperty = props.FirstOrDefault(p => p.Name == "kind");
            if (kindProperty.Value.ValueKind != JsonValueKind.String) throw new FormatException("Address value kind is required.");
            var kind = ParseKind(kindProperty.Value.GetString()!);
            var expected = kind == CommandValueKind.IntegerIdentifier
                ? new[] { "addressId", "kind", "identifier" }
                : new[] { "addressId", "kind", "text" };
            if (names.Length != expected.Length || names.Any(n => !expected.Contains(n, StringComparer.Ordinal)))
                throw new FormatException("An address value has missing or unknown properties.");
            var id = PositiveUInt64(props.Single(p => p.Name == "addressId").Value, "addressId");
            var raw = props.Single(p => p.Name == (kind == CommandValueKind.IntegerIdentifier ? "identifier" : "text")).Value;
            CommandValue value;
            try
            {
                value = kind == CommandValueKind.IntegerIdentifier
                    ? CommandValue.FromIdentifier(PositiveUInt64(raw, "identifier"))
                    : CommandValue.FromText(String(raw, "text"));
            }
            catch (ArgumentException ex) { throw new FormatException("An address value is invalid.", ex); }
            list.Add(new CommandAddressValue(id, value));
            if (list.Count > CommandExchange.MaxValues) throw new FormatException("Address value count exceeds the configured limit.");
        }
        return list.ToArray();
    }

    private static string String(JsonElement value, string name) =>
        value.ValueKind == JsonValueKind.String ? value.GetString()! : throw new FormatException($"'{name}' must be a string.");

    private static string? NullableString(JsonElement value, string name) =>
        value.ValueKind == JsonValueKind.Null ? null : String(value, name);

    private static bool Boolean(JsonElement value, string name) =>
        value.ValueKind is JsonValueKind.True or JsonValueKind.False ? value.GetBoolean() : throw new FormatException($"'{name}' must be a boolean.");

    private static ulong PositiveUInt64(JsonElement value, string name) =>
        value.ValueKind == JsonValueKind.Number && value.TryGetUInt64(out var number) && number > 0 ? number : throw new FormatException($"'{name}' must be a positive unsigned integer.");

    private static uint PositiveUInt32(JsonElement value, string name) =>
        value.ValueKind == JsonValueKind.Number && value.TryGetUInt32(out var number) && number > 0 ? number : throw new FormatException($"'{name}' must be a positive unsigned integer.");

    private static string AccessName(CommandAddressAccess value) => value switch
    {
        CommandAddressAccess.Read => "read",
        CommandAddressAccess.Write => "write",
        CommandAddressAccess.ReadWrite => "readWrite",
        _ => throw new FormatException("Unsupported address access.")
    };

    private static CommandAddressAccess ParseAccess(string value) => value switch
    {
        "read" => CommandAddressAccess.Read,
        "write" => CommandAddressAccess.Write,
        "readWrite" => CommandAddressAccess.ReadWrite,
        _ => throw new FormatException($"Unknown address access '{value}'.")
    };

    private static string KindName(CommandValueKind value) => value switch
    {
        CommandValueKind.IntegerIdentifier => "integerIdentifier",
        CommandValueKind.LiteralText => "literalText",
        _ => throw new FormatException("Unsupported value kind.")
    };

    private static CommandValueKind ParseKind(string value) => value switch
    {
        "integerIdentifier" => CommandValueKind.IntegerIdentifier,
        "literalText" => CommandValueKind.LiteralText,
        _ => throw new FormatException($"Unknown value kind '{value}'.")
    };
}
