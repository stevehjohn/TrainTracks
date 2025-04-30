using TrainTracks.Engine;
using TrainTracks.Engine.Board;

namespace TrainTracks.Console;

public static class EntryPoint
{
    public static void Main()
    {
        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };
        
    }

    private static void VisualiseStep(Grid grid)
    {
    }
}