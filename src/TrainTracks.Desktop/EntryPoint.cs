using JetBrains.Annotations;
using TrainTracks.Desktop.Presentation;
using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Desktop;

[UsedImplicitly]
public class EntryPoint
{
    public static void Main(string[] arguments)
    {
        PuzzleManager.Path = "Data/puzzles.json";
        
        using var renderer = new PuzzleRenderer();

        renderer.Grid = PuzzleManager.Instance.GetPuzzle(int.Parse(arguments[0]));
        
        renderer.Run();
    }
}