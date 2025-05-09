using System.Diagnostics;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Infrastructure;
using static System.Console;

namespace TrainTracks.Console.Runners;

public class Local
{
    private long _count;

    private Stopwatch _stopwatch;
    
    public void Run(int puzzleNumber)
    {
        Clear();
        
        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        PuzzleManager.Path = "Data/puzzles.json";
        
        var puzzle = PuzzleManager.Instance.GetPuzzle(puzzleNumber);
        
        WriteLine($"Puzzle number: {puzzleNumber} ({puzzle.Width}x{puzzle.Height})");
        
        WriteLine();

        WriteLine(puzzle.ToString());
        
        _stopwatch = Stopwatch.StartNew();
        
        var result = solver.Solve(puzzle);

        CursorTop = puzzle.Height + 3;
        
        WriteLine(puzzle.ToString());
        
        WriteLine($"Solve state: {result}");
        
        WriteLine($"Steps:       {_count:N0}");
                
        WriteLine($"Elapsed:     {_stopwatch.Elapsed:h\\:mm\\:ss\\.fff}");

        WriteLine();
    }

    private void VisualiseStep(Grid grid)
    {
        _count++;

        if (_count % 100_000 != 0)
        {
            return;
        }

        CursorTop = grid.Height + 3;
        
        WriteLine(grid.ToString());
        
        WriteLine($"Steps:       {_count:N0}");
        
        WriteLine($"Elapsed:     {_stopwatch.Elapsed:h\\:mm\\:ss\\.fff}");
    }
}