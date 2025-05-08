// ReSharper disable UnusedAutoPropertyAccessor.Global

using CommandLine;
using JetBrains.Annotations;
using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Console.Infrastructure;

[UsedImplicitly]
[Verb("remote", HelpText = "Run puzzles from Puzzle Madness.")]
public class RemoteOptions
{
    [Option('d', "difficulty", Required = true, HelpText = "The class of puzzles to solve.")]
    public Difficulty Difficulty { get; set; }
    
    [Option('q', "quantity", Required = true, HelpText = "The number of puzzles to solve.")]
    public int Quantity { get; set; }
}