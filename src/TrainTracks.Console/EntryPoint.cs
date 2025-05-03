using TrainTracks.Engine;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Infrastructure;
using static System.Console;

namespace TrainTracks.Console;

public static class EntryPoint
{
    private static int _count = -1;
    
    public static void Main(string[] arguments)
    {
        Clear();
        
        var puzzleNumber = 6;

        if (arguments.Length > 0)
        {
            if (! int.TryParse(arguments[0], out puzzleNumber))
            {
                puzzleNumber = 6;
            }
        }
        
        WriteLine($"Puzzle number: {arguments.Length}");
        
        WriteLine();

        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        PuzzleManager.Path = "Data/puzzles.json";
        
        var puzzle = PuzzleManager.Instance.Puzzles[puzzleNumber];
        
        WriteLine(puzzle.ToString());

        // ReadKey();
        
        var result = solver.Solve(puzzle);

        Clear();
                
        WriteLine($"Puzzle number: {arguments.Length}");
        
        WriteLine();
        
        WriteLine(PuzzleManager.Instance.Puzzles[puzzleNumber]);
        
        WriteLine();
        
        WriteLine(puzzle.ToString());
        
        WriteLine($"Solve state: {result}");
        
        WriteLine($"Steps:       {_count + 1:N0}");
        
        WriteLine();
    }

    private static void VisualiseStep(Grid grid)
    {
        _count++;

        if (_count % 100_000 != 0)
        {
            return;
        }

        CursorTop = 2;
        
        WriteLine(grid.ToString());
        
        WriteLine($"Steps: {_count + 1:N0}");
        
        // ReadKey();
    }
}