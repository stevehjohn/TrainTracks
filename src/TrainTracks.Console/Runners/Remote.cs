using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Infrastructure;
using static System.Console;

namespace TrainTracks.Console.Runners;

public class Remote
{
    public void Run(RemoteOptions options)
    {
        var client = new PuzzleClient();

        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        WriteLine();
        
        for (var i = 0; i < options.Quantity; i++)
        {
            var puzzle = client.GetNextPuzzle(options.Difficulty);

            if (puzzle == null)
            {
                WriteLine("No puzzles available.");
                
                break;
            }
            
            WriteLine($"Solving puzzle for {puzzle.Value.Date:d} ({puzzle.Value.Grid.Width}x{puzzle.Value.Grid.Height})");
            
            WriteLine();

            var result = solver.Solve(puzzle.Value.Grid);

            if (! result)
            {
                WriteLine("Unable to solve the puzzle.");
                
                WriteLine();
            }
            
            WriteLine("Solved in ");

            Thread.Sleep(1_000);
        }
        
        WriteLine();
    }

    private void VisualiseStep(Grid grid)
    {
    }
}