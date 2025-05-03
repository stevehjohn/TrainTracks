using TrainTracks.Engine;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Infrastructure;
using static System.Console;

namespace TrainTracks.Console;

public static class EntryPoint
{
    private static int _count = -1;
    
    public static void Main()
    {
        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        PuzzleManager.Path = "Data/puzzles.json";
        
        var puzzle = PuzzleManager.Instance.Puzzles[8];

        Clear();
        
        WriteLine(puzzle.ToString());

        // ReadKey();
        
        var result = solver.Solve(puzzle);
        
        WriteLine(puzzle.ToString());
        
        WriteLine($"Solve state: {result}");
        
        WriteLine($"Steps: {_count + 1}");
    }

    private static void VisualiseStep(Grid grid)
    {
        _count++;

        if (_count % 100_000 != 0)
        {
            return;
        }

        CursorTop = 1;
        
        WriteLine(grid.ToString());
        
        WriteLine(_count);
        
        // ReadKey();
    }
}