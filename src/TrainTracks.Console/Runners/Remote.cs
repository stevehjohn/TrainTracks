using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Console.Runners;

public class Remote
{
    public void Run(RemoteOptions options)
    {
        var client = new PuzzleClient();
        
        var puzzle = client.GetNextPuzzle(options.Difficulty);
    }
}