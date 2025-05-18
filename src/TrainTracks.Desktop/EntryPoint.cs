using JetBrains.Annotations;
using TrainTracks.Desktop.Presentation;

namespace TrainTracks.Desktop;

[UsedImplicitly]
public class EntryPoint
{
    public static void Main()
    {
        var renderer = new PuzzleRenderer();
        
        renderer.Run();
    }
}