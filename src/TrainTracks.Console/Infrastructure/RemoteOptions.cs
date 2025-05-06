using CommandLine;
using JetBrains.Annotations;

namespace TrainTracks.Console.Infrastructure;

[UsedImplicitly]
[Verb("remote", HelpText = "Run puzzles from Puzzle Madness.")]
public class RemoteOptions
{
    public Difficulty Difficulty { get; set; }
}