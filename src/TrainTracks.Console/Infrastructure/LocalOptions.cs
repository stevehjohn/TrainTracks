using CommandLine;

namespace TrainTracks.Console.Infrastructure;

[Verb("local", HelpText = "Run a puzzle from the local file system.")]
public class LocalOptions
{
    public int PuzzleNumber { get; set; }
}