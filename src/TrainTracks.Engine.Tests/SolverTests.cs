using TrainTracks.Engine.Infrastructure;
using static System.Console;

namespace TrainTracks.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void CanSolvePuzzle()
    {
        PuzzleManager.Path = "Test Data/puzzles.json";

        var puzzle = PuzzleManager.Instance.GetPuzzle(4);

        var solver = new Solver();

        var result = solver.Solve(puzzle);
        
        // ReSharper disable Xunit.XunitTestWithConsoleOutput
        // Will fix later.
        WriteLine();
        
        WriteLine($"Solved: {result}.");

        WriteLine();
        
        WriteLine(solver.Grid.ToString());

        WriteLine();
    }
}