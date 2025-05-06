using CommandLine;
using JetBrains.Annotations;

namespace TrainTracks.Console.Infrastructure;

[UsedImplicitly]
[Verb("local", HelpText = "Run a puzzle from the local file system.")]
public class LocalOptions
{
    public int PuzzleNumber { get; set; }
}