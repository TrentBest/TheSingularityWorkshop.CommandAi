namespace TheSingularityWorkshop.CommandAi;

/// <summary>Declares whether an address may be supplied to or returned from an exchange.</summary>
[Flags]
public enum CommandAddressAccess
{
    None = 0,
    Read = 1,
    Write = 2,
    ReadWrite = Read | Write
}

/// <summary>
/// Describes an application-owned value slot. The address is a contract identifier, never a
/// pointer or permission to access application memory.
/// </summary>
public sealed record CommandAddressDescriptor
{
    public CommandAddressDescriptor(
        ulong addressId,
        string name,
        CommandAddressAccess access,
        CommandValueKind valueKind,
        bool requiredInput = false,
        bool requiredOutput = false,
        string? description = null,
        ulong? grammarDefinitionId = null,
        uint? grammarSchemaVersion = null)
    {
        if (addressId == 0) throw new ArgumentOutOfRangeException(nameof(addressId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Address name is required.", nameof(name));
        if (name.Length > 256) throw new ArgumentException("Address name cannot exceed 256 characters.", nameof(name));
        if (description?.Length > 4096) throw new ArgumentException("Description cannot exceed 4096 characters.", nameof(description));
        if (access is CommandAddressAccess.None || (access & ~CommandAddressAccess.ReadWrite) != 0)
            throw new ArgumentOutOfRangeException(nameof(access));
        if (valueKind is not (CommandValueKind.IntegerIdentifier or CommandValueKind.LiteralText))
            throw new ArgumentOutOfRangeException(nameof(valueKind));
        if (requiredInput && (access & CommandAddressAccess.Read) == 0)
            throw new ArgumentException("A required input address must be readable.", nameof(requiredInput));
        if (requiredOutput && (access & CommandAddressAccess.Write) == 0)
            throw new ArgumentException("A required output address must be writable.", nameof(requiredOutput));
        if (grammarDefinitionId.HasValue != grammarSchemaVersion.HasValue)
            throw new ArgumentException("Grammar definition ID and schema version must be supplied together.");
        if (grammarDefinitionId == 0 || grammarSchemaVersion == 0)
            throw new ArgumentOutOfRangeException(nameof(grammarDefinitionId), "Grammar identifiers and schema versions must be positive.");

        AddressId = addressId;
        Name = name;
        Access = access;
        ValueKind = valueKind;
        RequiredInput = requiredInput;
        RequiredOutput = requiredOutput;
        Description = description;
        GrammarDefinitionId = grammarDefinitionId;
        GrammarSchemaVersion = grammarSchemaVersion;
    }

    public ulong AddressId { get; }
    public string Name { get; }
    public CommandAddressAccess Access { get; }
    public CommandValueKind ValueKind { get; }
    public bool RequiredInput { get; }
    public bool RequiredOutput { get; }
    public string? Description { get; }
    /// <summary>Opaque GrammarAi definition identity; CommandAi does not resolve or validate it.</summary>
    public ulong? GrammarDefinitionId { get; }
    /// <summary>Opaque GrammarAi schema version paired with <see cref="GrammarDefinitionId"/>.</summary>
    public uint? GrammarSchemaVersion { get; }
}

/// <summary>An immutable, application-defined collection of address descriptors.</summary>
public sealed class CommandAddressSpace
{
    public const int MaxAddresses = 1024;
    private readonly IReadOnlyDictionary<ulong, CommandAddressDescriptor> _byId;

    public CommandAddressSpace(IEnumerable<CommandAddressDescriptor> addresses)
    {
        ArgumentNullException.ThrowIfNull(addresses);
        var snapshot = addresses.ToArray();
        if (snapshot.Length == 0) throw new ArgumentException("An address space cannot be empty.", nameof(addresses));
        if (snapshot.Length > MaxAddresses) throw new ArgumentException($"An address space cannot exceed {MaxAddresses} descriptors.", nameof(addresses));
        if (snapshot.Any(x => x is null)) throw new ArgumentException("Address descriptors cannot contain null.", nameof(addresses));
        var map = new Dictionary<ulong, CommandAddressDescriptor>();
        foreach (var address in snapshot)
            if (!map.TryAdd(address.AddressId, address))
                throw new ArgumentException($"Duplicate address ID {address.AddressId}.", nameof(addresses));
        Addresses = Array.AsReadOnly(snapshot);
        _byId = map;
    }

    public IReadOnlyList<CommandAddressDescriptor> Addresses { get; }

    internal CommandAddressDescriptor Get(ulong addressId) =>
        _byId.TryGetValue(addressId, out var descriptor)
            ? descriptor
            : throw new ArgumentException($"Address {addressId} is not declared in this address space.");
}

/// <summary>Binds a declared address identifier to a typed value; it does not read or write host memory.</summary>
public sealed record CommandAddressValue
{
    public CommandAddressValue(ulong addressId, CommandValue value)
    {
        if (addressId == 0) throw new ArgumentOutOfRangeException(nameof(addressId));
        ArgumentNullException.ThrowIfNull(value);
        AddressId = addressId;
        Value = value;
    }

    public ulong AddressId { get; }
    public CommandValue Value { get; }
}

/// <summary>A correlated request/context exchange against an application-defined address space.</summary>
public sealed class CommandExchange
{
    public const int MaxValues = 1024;

    public CommandExchange(Guid exchangeId, CommandAddressSpace addressSpace, IEnumerable<CommandAddressValue> inputs)
    {
        if (exchangeId == Guid.Empty) throw new ArgumentException("Exchange ID cannot be empty.", nameof(exchangeId));
        ArgumentNullException.ThrowIfNull(addressSpace);
        ArgumentNullException.ThrowIfNull(inputs);
        var snapshot = inputs.ToArray();
        ValidateValues(addressSpace, snapshot, CommandAddressAccess.Read, true);
        ExchangeId = exchangeId;
        AddressSpace = addressSpace;
        Inputs = Array.AsReadOnly(snapshot);
    }

    public Guid ExchangeId { get; }
    public CommandAddressSpace AddressSpace { get; }
    public IReadOnlyList<CommandAddressValue> Inputs { get; }

    internal static void ValidateValues(
        CommandAddressSpace addressSpace,
        CommandAddressValue[] values,
        CommandAddressAccess requiredAccess,
        bool validateRequired)
    {
        if (values.Length > MaxValues) throw new ArgumentException($"An exchange cannot contain more than {MaxValues} values.", nameof(values));
        if (values.Any(x => x is null)) throw new ArgumentException("Address values cannot contain null.", nameof(values));
        var seen = new HashSet<ulong>();
        foreach (var item in values)
        {
            if (!seen.Add(item.AddressId)) throw new ArgumentException($"Duplicate address value {item.AddressId}.", nameof(values));
            var descriptor = addressSpace.Get(item.AddressId);
            if ((descriptor.Access & requiredAccess) == 0)
                throw new ArgumentException($"Address {item.AddressId} is not declared for {requiredAccess.ToString().ToLowerInvariant()} access.", nameof(values));
            if (item.Value.Kind != descriptor.ValueKind)
                throw new ArgumentException($"Value kind does not match address {item.AddressId}.", nameof(values));
        }

        if (validateRequired)
        {
            foreach (var descriptor in addressSpace.Addresses)
            {
                var required = requiredAccess == CommandAddressAccess.Read ? descriptor.RequiredInput : descriptor.RequiredOutput;
                if (required && !seen.Contains(descriptor.AddressId))
                    throw new ArgumentException($"Required address {descriptor.AddressId} was not supplied.", nameof(values));
            }
        }
    }
}

/// <summary>A correlated response containing values for writable addresses in the originating exchange.</summary>
public sealed class CommandResponse
{
    public CommandResponse(Guid exchangeId, CommandAddressSpace addressSpace, IEnumerable<CommandAddressValue> outputs)
    {
        if (exchangeId == Guid.Empty) throw new ArgumentException("Exchange ID cannot be empty.", nameof(exchangeId));
        ArgumentNullException.ThrowIfNull(addressSpace);
        ArgumentNullException.ThrowIfNull(outputs);
        var snapshot = outputs.ToArray();
        CommandExchange.ValidateValues(addressSpace, snapshot, CommandAddressAccess.Write, true);
        ExchangeId = exchangeId;
        AddressSpace = addressSpace;
        Outputs = Array.AsReadOnly(snapshot);
    }

    public Guid ExchangeId { get; }
    public CommandAddressSpace AddressSpace { get; }
    public IReadOnlyList<CommandAddressValue> Outputs { get; }

    /// <summary>Ensures a response belongs to the request it is being paired with.</summary>
    public void ValidateFor(CommandExchange exchange)
    {
        ArgumentNullException.ThrowIfNull(exchange);
        if (ExchangeId != exchange.ExchangeId)
            throw new ArgumentException("Response exchange ID does not match the request.", nameof(exchange));
        if (!AddressSpace.Addresses.SequenceEqual(exchange.AddressSpace.Addresses))
            throw new ArgumentException("Response address-space descriptors do not match the originating exchange.", nameof(exchange));
    }
}
