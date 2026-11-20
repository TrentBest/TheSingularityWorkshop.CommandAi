using TheSingularityWorkshop.CommandAi;
using TheSingularityWorkshop.CommandAi.Samples.SingularityBot;

namespace CommandAi.Tests;

public sealed class SingularityBotProposalSampleTests
{
    private const string ValidMarineProposal = """
        [CommandAi]
        Protocol=CommandAi/1
        Command=1001 2001=1 2002="4"
        [/CommandAi]
        """;

    [Fact]
    public void ValidLlmProposal_DecodesToMarineProductionTarget()
    {
        var proposal = CommandAiTextCodec.Decode(ValidMarineProposal);

        Assert.True(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out var desiredCount, out var error));
        Assert.Equal(4, desiredCount);
        Assert.Empty(error);
    }

    [Fact]
    public void UnknownAction_IsRejected()
    {
        var proposal = CommandAiTextCodec.Decode(ValidMarineProposal.Replace("Command=1001", "Command=9999"));

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("action", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UnknownTarget_IsRejected()
    {
        var proposal = CommandAiTextCodec.Decode(ValidMarineProposal.Replace("2001=1", "2001=999"));

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("target", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UnknownParameter_IsRejected()
    {
        var proposal = CommandAiTextCodec.Decode(ValidMarineProposal.Replace("2002=", "2999="));

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("parameter", error, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("four")]
    [InlineData("")]
    public void InvalidDesiredCount_IsRejected(string count)
    {
        var proposal = CommandAiTextCodec.Decode(
            ValidMarineProposal.Replace("2002=\"4\"", $"2002=\"{count}\""));

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("count", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MissingRequiredParameter_IsRejected()
    {
        const string missingCount = """
            [CommandAi]
            Protocol=CommandAi/1
            Command=1001 2001=1
            [/CommandAi]
            """;
        var proposal = CommandAiTextCodec.Decode(missingCount);

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("parameter", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MultipleCommands_AreRejectedBySingleOperationSample()
    {
        var proposal = CommandAiTextCodec.Decode(
            ValidMarineProposal.Replace(
                "Command=1001 2001=1 2002=\"4\"",
                "Command=1001 2001=1 2002=\"4\"\nCommand=1001 2001=1 2002=\"5\""));

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("one command", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void WrongValueKind_IsRejected()
    {
        var proposal = CommandAiTextCodec.Decode(
            ValidMarineProposal.Replace("2001=1", "2001=\"Marine\""));

        Assert.False(BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out _, out var error));
        Assert.Contains("target", error, StringComparison.OrdinalIgnoreCase);
    }
}
