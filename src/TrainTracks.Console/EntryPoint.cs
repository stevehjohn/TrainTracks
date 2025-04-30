using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;

using static System.Console;

namespace TrainTracks.Console;

public static class EntryPoint
{
    private static int _count = 0;
    
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
        
        VisualiseStep(solver.Grid);
    }

    private static void VisualiseStep(Grid grid)
    {
        _count++;

        if (_count == 100)
        {
            _count = 0;
        }
        else
        {
            return;
        }

        CursorTop = 1;
        
        WriteLine(grid.ToString());
        
        //ReadKey();
    }
}