using System.Diagnostics;
using CommandLine;
using TrainTracks.Console.Infrastructure;
using TrainTracks.Console.Runners;

namespace TrainTracks.Console;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        Parser.Default.ParseArguments<LocalOptions, RemoteOptions>(arguments)
            .WithParsed<LocalOptions>(options => new Local().Run(options.PuzzleNumber))
            .WithParsed<RemoteOptions>(_ => Debug.Fail("Remote not yet implemented."));
    }
}