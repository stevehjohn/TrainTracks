using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;

using static System.Console;

namespace TrainTracks.Console;

public static class EntryPoint
{
    public static void Main()
    {
        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        var puzzle = PuzzleManager.Instance.Puzzles[0];

        Clear();
        
        VisualiseStep(puzzle);
        
        solver.Solve(puzzle);
    }

    private static void VisualiseStep(Grid grid)
    {
        CursorTop = 1;
        
        WriteLine(grid.ToString());

        ReadKey();
    }
}