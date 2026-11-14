using TheSingularityWorkshop.CommandAi;

namespace CommandAi.Tests;

public sealed class CommandExchangeTextCodecTests
{
    private static CommandAddressSpace Space() => new([
        new CommandAddressDescriptor(1, "context", CommandAddressAccess.Read, CommandValueKind.LiteralText, requiredInput: true),
        new CommandAddressDescriptor(2, "result", CommandAddressAccess.Write, CommandValueKind.IntegerIdentifier, requiredOutput: true,
            grammarDefinitionId: 42, grammarSchemaVersion: 3)
    ]);

    [Fact]
    public void Exchange_EncodesAndDecodesCanonicalBoundedJson()
    {
        var exchange = new CommandExchange(Guid.Parse("6e54ec9e-7d38-4f40-b2a1-4b1770d15c02"), Space(), [
            new CommandAddressValue(1, CommandValue.FromText("hello"))
        ]);
        var encoded = CommandExchangeTextCodec.Encode(exchange);
        var decoded = CommandExchangeTextCodec.DecodeExchange(encoded);
        Assert.Equal(encoded, CommandExchangeTextCodec.Encode(decoded));
        Assert.Equal(exchange.ExchangeId, decoded.ExchangeId);
        Assert.Equal("hello", Assert.Single(decoded.Inputs).Value.Text);
        Assert.Equal(42UL, decoded.AddressSpace.Addresses[1].GrammarDefinitionId);
        Assert.Equal(3U, decoded.AddressSpace.Addresses[1].GrammarSchemaVersion);
    }

    [Fact]
    public void Response_EncodesAndDecodesAndCorrelates()
    {
        var id = Guid.NewGuid();
        var space = Space();
        var request = new CommandExchange(id, space, [new CommandAddressValue(1, CommandValue.FromText("input"))]);
        var response = new CommandResponse(id, space, [new CommandAddressValue(2, CommandValue.FromIdentifier(99))]);
        var decoded = CommandExchangeTextCodec.DecodeResponse(CommandExchangeTextCodec.Encode(response));
        decoded.ValidateFor(request);
        Assert.Equal(99UL, Assert.Single(decoded.Outputs).Value.Identifier);
    }

    [Theory]
    [InlineData("{")]
    [InlineData("{\"protocol\":\"CommandAi/1\",\"protocol\":\"CommandAi/1\"}")]
    [InlineData("{\"protocol\":\"CommandAi/1\",\"kind\":\"exchange\",\"exchangeId\":\"00000000-0000-0000-0000-000000000000\",\"addresses\":[],\"inputs\":[],\"extra\":1}")]
    public void Decode_RejectsMalformedDuplicateOrUnknownProperties(string json) =>
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));

    [Fact]
    public void Decode_RejectsUnknownAddressDescriptorFields()
    {
        var json = "{\"protocol\":\"CommandAi/1\",\"kind\":\"exchange\",\"exchangeId\":\"6e54ec9e-7d38-4f40-b2a1-4b1770d15c02\",\"addresses\":[{\"addressId\":1,\"name\":\"x\",\"access\":\"read\",\"valueKind\":\"literalText\",\"requiredInput\":false,\"requiredOutput\":false,\"surprise\":true}],\"inputs\":[]}";
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }

    [Fact]
    public void Decode_RejectsWrongEnvelopeKind()
    {
        var response = CommandExchangeTextCodec.Encode(new CommandResponse(Guid.NewGuid(), Space(), [
            new CommandAddressValue(2, CommandValue.FromIdentifier(1))
        ]));
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(response));
    }

    [Fact]
    public void Decode_RejectsEnvelopeAboveConfiguredLimit() =>
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(new string(' ', CommandExchangeTextCodec.MaxEnvelopeCharacters + 1)));

    [Fact]
    public void Encode_RejectsEnvelopeAboveConfiguredLimit()
    {
        var addresses = Enumerable.Range(1, 16)
            .Select(i => new CommandAddressDescriptor((ulong)i, $"a{i}", CommandAddressAccess.Read, CommandValueKind.LiteralText));
        var space = new CommandAddressSpace(addresses);
        var exchange = new CommandExchange(Guid.NewGuid(), space,
            Enumerable.Range(1, 16).Select(i => new CommandAddressValue((ulong)i,
                CommandValue.FromText(new string('x', CommandLimits.MaxLiteralCharacters)))));
        Assert.Throws<ArgumentException>(() => CommandExchangeTextCodec.Encode(exchange));
    }

    private static string ValidExchangeJson() => """
        {
          "protocol":"CommandAi/1",
          "kind":"exchange",
          "exchangeId":"6e54ec9e-7d38-4f40-b2a1-4b1770d15c02",
          "addresses":[
            {"addressId":1,"name":"context","access":"read","valueKind":"literalText","requiredInput":true,"requiredOutput":false},
            {"addressId":2,"name":"result","access":"write","valueKind":"integerIdentifier","requiredInput":false,"requiredOutput":true,"grammarDefinitionId":42,"grammarSchemaVersion":3}
          ],
          "inputs":[{"addressId":1,"kind":"literalText","text":"hello"}]
        }
        """;

    [Fact]
    public void Decode_RejectsNullEmptyAndWhitespaceEnvelopes()
    {
        Assert.Throws<ArgumentNullException>(() => CommandExchangeTextCodec.DecodeExchange(null!));
        Assert.Throws<ArgumentException>(() => CommandExchangeTextCodec.DecodeExchange(string.Empty));
        Assert.Throws<ArgumentException>(() => CommandExchangeTextCodec.DecodeExchange("  \t\r\n"));
    }

    [Theory]
    [InlineData("\"kind\":\"exchange\"","\"kind\":17")]
    [InlineData("\"exchangeId\":\"6e54ec9e-7d38-4f40-b2a1-4b1770d15c02\"","\"exchangeId\":\"not-a-guid\"")]
    [InlineData("\"addresses\":[","\"addresses\":null,\"unused\":[")]
    [InlineData("\"access\":\"read\"","\"access\":\"execute\"")]
    [InlineData("\"valueKind\":\"literalText\"","\"valueKind\":\"unknown\"")]
    [InlineData("\"addressId\":1","\"addressId\":0")]
    public void Decode_RejectsInvalidEnvelopeAndDescriptorFields(string original, string replacement)
    {
        var json = ValidExchangeJson().Replace(original, replacement, StringComparison.Ordinal);
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }

    [Theory]
    [InlineData("\"grammarDefinitionId\":42","")]
    [InlineData("\"grammarSchemaVersion\":3","")]
    [InlineData("\"grammarDefinitionId\":42","\"grammarDefinitionId\":0")]
    [InlineData("\"grammarSchemaVersion\":3","\"grammarSchemaVersion\":0")]
    public void Decode_RequiresPositivePairedGrammarIdentity(string original, string replacement)
    {
        var json = ValidExchangeJson().Replace(original, replacement, StringComparison.Ordinal);
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }

    [Theory]
    [InlineData("\"inputs\":[{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"}]","\"inputs\":[]")]
    [InlineData("\"inputs\":[{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"}]","\"inputs\":[{\"addressId\":99,\"kind\":\"literalText\",\"text\":\"hello\"}]")]
    [InlineData("\"inputs\":[{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"}]","\"inputs\":[{\"addressId\":1,\"kind\":\"integerIdentifier\",\"identifier\":9}]")]
    [InlineData("\"inputs\":[{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"}]","\"inputs\":[{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"},{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"again\"}]")]
    public void Decode_RejectsInvalidAddressValueBindings(string original, string replacement)
    {
        var json = ValidExchangeJson().Replace(original, replacement, StringComparison.Ordinal);
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }

    [Fact]
    public void Decode_RejectsAddressDescriptorWithInvalidAccessContract()
    {
        var json = ValidExchangeJson().Replace("\"access\":\"read\"","\"access\":\"write\"",StringComparison.Ordinal);
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }

    [Fact]
    public void Decode_RejectsDuplicateAddressIdentifiers()
    {
        var json = ValidExchangeJson().Replace(
            "{\"addressId\":2,\"name\":\"result\"",
            "{\"addressId\":1,\"name\":\"result\"",
            StringComparison.Ordinal);
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }

    [Fact]
    public void Decode_RejectsValueCountAboveConfiguredLimit()
    {
        var json = ValidExchangeJson().Replace(
            "\"inputs\":[{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"}]",
            "\"inputs\":[" + string.Join(",", Enumerable.Range(0, CommandExchange.MaxValues + 1)
                .Select(_ => "{\"addressId\":1,\"kind\":\"literalText\",\"text\":\"hello\"}")) + "]",
            StringComparison.Ordinal);
        Assert.Throws<FormatException>(() => CommandExchangeTextCodec.DecodeExchange(json));
    }
