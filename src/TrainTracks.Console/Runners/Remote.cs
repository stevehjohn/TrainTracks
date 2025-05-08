using System.Diagnostics;
using System.Net;
using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Infrastructure;
using static System.Console;

namespace TrainTracks.Console.Runners;

public class Remote
{
    private int _top;

    private int _count;

    public void Run(RemoteOptions options)
    {
        var client = new PuzzleClient();

        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        var stopwatch = new Stopwatch();

        for (var i = 0; i < options.Quantity; i++)
        {
            Clear();

            WriteLine();

            WriteLine($"Fetching {options.Difficulty.ToString().ToLowerInvariant()} puzzle {i + 1} of {options.Quantity}...");

            WriteLine();

            var puzzle = client.GetNextPuzzle(options.Difficulty);

            if (puzzle == null)
            {
                WriteLine("No puzzles available.");

                break;
            }

            WriteLine($"Solving puzzle for {puzzle.Value.Date:R} ({puzzle.Value.Grid.Width}x{puzzle.Value.Grid.Height}).");

            WriteLine();

            WriteLine(puzzle.Value.Grid.ToString());

            WriteLine();

            _top = CursorTop;

            _count = 0;

            stopwatch.Restart();

            CursorVisible = false;

            var result = solver.Solve(puzzle.Value.Grid);

            CursorVisible = true;

            stopwatch.Stop();

            if (! result)
            {
                WriteLine("Unable to solve the puzzle.");

                WriteLine();
            }
            else
            {
                CursorTop = _top;

                WriteLine(puzzle.Value.Grid.ToString());

                WriteLine($"Solved in {stopwatch.Elapsed:g}, with {_count:N0} iterations.");

                WriteLine();

                WriteLine("Sending result...");

                WriteLine();

                var statusCode = client.SendResult(puzzle.Value.Date, puzzle.Value.Grid, puzzle.Value.Variant);

                if (statusCode != HttpStatusCode.OK)
                {
                    WriteLine($"Result not accepted. Status code: {(int) statusCode}.");
                }
                else
                {
                    WriteLine("Result accepted.");
                }
            }

            if (i < options.Quantity - 1)
            {
                //Thread.Sleep(2_000);
            }
        }

        WriteLine();
    }

    private void VisualiseStep(Grid grid)
    {
        if (_count++ % 100 != 0)
        {
            return;
        }

        CursorTop = _top;

        WriteLine(grid.ToString());
    }
}