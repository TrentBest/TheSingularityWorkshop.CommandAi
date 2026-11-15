using TheSingularityWorkshop.CommandAi;
using TheSingularityWorkshop.CommandAi.MicroBundle;

var capability = new CommandAiMicroBundle();
var proposal = new CommandProposal(
[
    new CommandInstruction(1001)
]);

var encoded = capability.Encode(proposal);
var decoded = capability.Decode(encoded);

Console.WriteLine($"MicroBundle ID: {capability.Descriptor.Id}");
Console.WriteLine($"Capability version: {capability.Descriptor.Version}");
Console.WriteLine($"Provider: {capability.Descriptor.Providers[0].Id}");
Console.WriteLine($"Round-tripped commands: {decoded.Commands.Count}");
Console.WriteLine("No command was authorized or executed.");