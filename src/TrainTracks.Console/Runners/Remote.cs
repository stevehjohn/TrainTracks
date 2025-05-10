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

    private readonly Stopwatch _stopwatch = new();

    public void Run(RemoteOptions options)
    {
        var client = new PuzzleClient();

        var solver = new Solver
        {
            StepCallback = VisualiseStep
        };

        Clear();

        for (var i = 0; i < options.Quantity; i++)
        {
            WriteLine();

            WriteLine($"Fetching {options.Difficulty.ToString().ToLowerInvariant()} puzzle {i + 1:N0} of {options.Quantity:N0}...");

            WriteLine();

            (DateOnly Date, Grid Grid, int Variant)? puzzle = null;

            for (var retry = 1; retry < 21; retry++)
            {
                try
                {
                    puzzle = client.GetNextPuzzle(options.Difficulty);
                }
                catch
                {
                    //
                }

                if (puzzle != null)
                {
                    break;
                }

                var sleep = (int) Math.Pow(retry, 2);

                for (var timer = 0; timer < sleep; timer++)
                {
                    if (retry > 1)
                    {
                        CursorTop -= 2;
                    }

                    WriteLine($"Waiting for {sleep - timer:N0}s before attempt {retry}.");

                    WriteLine();

                    Thread.Sleep(1_000);

                    CursorTop -= 2;
                    
                    WriteLine("Retrying...                         ");
                    
                    WriteLine();
                }
            }

            Clear();

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

            _stopwatch.Restart();

            CursorVisible = false;

            var result = solver.Solve(puzzle.Value.Grid);

            CursorVisible = true;

            _stopwatch.Stop();

            if (! result)
            {
                WriteLine("Unable to solve the puzzle.");

                WriteLine();

                break;
            }

            CursorTop = _top;

            WriteLine(puzzle.Value.Grid.ToString());

            WriteLine(@$"Solved in {_stopwatch.Elapsed:h\:mm\:ss\.fff}, with {_count:N0} iterations.");

            WriteLine();

            WriteLine("Sending result...");

            WriteLine();

            for (var retry = 1; retry < 21; retry++)
            {
                var statusCode = client.SendResult(puzzle.Value.Date, puzzle.Value.Grid, puzzle.Value.Variant);

                if (statusCode != HttpStatusCode.OK)
                {
                    WriteLine($"Result not accepted. Status code: {(int) statusCode}.");
                    
                    WriteLine();

                    var sleep = (int) Math.Pow(retry, 2);

                    for (var timer = 0; timer < sleep; timer++)
                    {
                        if (retry > 1)
                        {
                            CursorTop -= 2;
                        }

                        WriteLine($"Waiting for {sleep - timer:N0}s before attempt {retry}.");

                        WriteLine();

                        Thread.Sleep(1_000);

                        CursorTop -= 2;
                    
                        WriteLine("Retrying...                         ");
                    
                        WriteLine();
                    }
                }
                else
                {
                    WriteLine("Result accepted.");
                    
                    break;
                }
            }
        }

        WriteLine();
    }

    private void VisualiseStep(Grid grid)
    {
        if (_count++ % 10_000 != 0)
        {
            return;
        }

        CursorTop = _top;

        WriteLine(grid.ToString());

        WriteLine($@"Elapsed: {_stopwatch.Elapsed:h\:mm\:ss\.fff}, Steps: {_count:N0}.    ");
    }
}