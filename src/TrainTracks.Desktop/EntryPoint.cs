using JetBrains.Annotations;
using TrainTracks.Desktop.Presentation;
using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Desktop;

[UsedImplicitly]
public class EntryPoint
{
    public static void Main()
    {
        PuzzleManager.Path = "Data/puzzles.json";
        
        var renderer = new PuzzleRenderer();

        renderer.Grid = PuzzleManager.Instance.GetPuzzle(14);
        
        renderer.Run();
    }
}