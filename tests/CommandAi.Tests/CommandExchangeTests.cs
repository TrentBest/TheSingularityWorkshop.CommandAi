using TheSingularityWorkshop.CommandAi;

namespace CommandAi.Tests;

public sealed class CommandExchangeTests
{
    private static CommandAddressSpace Space() => new([
        new CommandAddressDescriptor(1, "context", CommandAddressAccess.Read, CommandValueKind.LiteralText, requiredInput: true),
        new CommandAddressDescriptor(2, "intent", CommandAddressAccess.Write, CommandValueKind.IntegerIdentifier, requiredOutput: true),
        new CommandAddressDescriptor(3, "grammar-body", CommandAddressAccess.Write, CommandValueKind.LiteralText,
            grammarDefinitionId: 42, grammarSchemaVersion: 3)
    ]);

    [Fact]
    public void ExchangeAndResponse_PreserveCorrelationAndTypedValues()
    {
        var space = Space();
        var id = Guid.NewGuid();
        var request = new CommandExchange(id, space, [new CommandAddressValue(1, CommandValue.FromText("current state"))]);
        var response = new CommandResponse(id, space, [
            new CommandAddressValue(2, CommandValue.FromIdentifier(900)),
            new CommandAddressValue(3, CommandValue.FromText("{opaque grammar payload}"))
        ]);

        response.ValidateFor(request);

        Assert.Equal("current state", request.Inputs[0].Value.Text);
        Assert.Equal(900UL, response.Outputs[0].Value.Identifier);
    }

    [Fact]
    public void AddressSpace_RejectsDuplicateIds()
    {
        var address = new CommandAddressDescriptor(1, "a", CommandAddressAccess.Read, CommandValueKind.LiteralText);
        Assert.Throws<ArgumentException>(() => new CommandAddressSpace([address, address]));
    }

    [Fact]
    public void Descriptor_RequiresGrammarDefinitionAndVersionTogether()
    {
        Assert.Throws<ArgumentException>(() => new CommandAddressDescriptor(
            1, "body", CommandAddressAccess.Write, CommandValueKind.LiteralText, grammarDefinitionId: 5));
    }

    [Fact]
    public void Descriptor_RejectsRequiredInputOnWriteOnlyAddress() =>
        Assert.Throws<ArgumentException>(() => new CommandAddressDescriptor(
            1, "output", CommandAddressAccess.Write, CommandValueKind.LiteralText, requiredInput: true));

    [Fact]
    public void Exchange_RequiresRequiredInputs()
    {
        var space = Space();
        Assert.Throws<ArgumentException>(() => new CommandExchange(Guid.NewGuid(), space, []));
    }

    [Fact]
    public void Response_RequiresRequiredOutputs()
    {
        var space = Space();
        Assert.Throws<ArgumentException>(() => new CommandResponse(Guid.NewGuid(), space, []));
    }

    [Fact]
    public void Exchange_RejectsWriteOnlyAddressInInputs()
    {
        var space = Space();
        Assert.Throws<ArgumentException>(() => new CommandExchange(Guid.NewGuid(), space, [
            new CommandAddressValue(1, CommandValue.FromText("context")),
            new CommandAddressValue(2, CommandValue.FromIdentifier(10))
        ]));
    }

    [Fact]
    public void Response_RejectsReadOnlyAddressInOutputs()
    {
        var space = Space();
        Assert.Throws<ArgumentException>(() => new CommandResponse(Guid.NewGuid(), space, [
            new CommandAddressValue(1, CommandValue.FromText("not writable")),
            new CommandAddressValue(2, CommandValue.FromIdentifier(10))
        ]));
    }

    [Fact]
    public void Exchange_RejectsDuplicateAndUnknownAddresses()
    {
        var space = Space();
        var value = new CommandAddressValue(1, CommandValue.FromText("context"));
        Assert.Throws<ArgumentException>(() => new CommandExchange(Guid.NewGuid(), space, [value, value]));
        Assert.Throws<ArgumentException>(() => new CommandExchange(Guid.NewGuid(), space, [
            value, new CommandAddressValue(99, CommandValue.FromText("unknown"))
        ]));
    }

    [Fact]
    public void Exchange_RejectsMismatchedValueKind()
    {
        var space = Space();
        Assert.Throws<ArgumentException>(() => new CommandExchange(Guid.NewGuid(), space, [
            new CommandAddressValue(1, CommandValue.FromIdentifier(4))
        ]));
    }

    [Fact]
    public void Response_RejectsMismatchedCorrelation()
    {
        var space = Space();
        var request = new CommandExchange(Guid.NewGuid(), space, [new CommandAddressValue(1, CommandValue.FromText("context"))]);
        var response = new CommandResponse(Guid.NewGuid(), space, [new CommandAddressValue(2, CommandValue.FromIdentifier(10))]);
        Assert.Throws<ArgumentException>(() => response.ValidateFor(request));
    }

    [Fact]
    public void ExchangeAndResponse_TakeImmutableSnapshots()
    {
        var space = Space();
        var inputs = new List<CommandAddressValue> { new(1, CommandValue.FromText("context")) };
        var request = new CommandExchange(Guid.NewGuid(), space, inputs);
        inputs.Clear();
        Assert.Single(request.Inputs);
    }

    [Fact]
    public void AddressSpace_RejectsExcessiveDescriptors()
    {
        var descriptors = Enumerable.Range(1, CommandAddressSpace.MaxAddresses + 1)
            .Select(i => new CommandAddressDescriptor((ulong)i, $"address-{i}", CommandAddressAccess.Read, CommandValueKind.IntegerIdentifier));
        Assert.Throws<ArgumentException>(() => new CommandAddressSpace(descriptors));
    }
}
