using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void CanSolvePuzzle()
    {
        PuzzleManager.Path = "Test Data/puzzles.json";
        
        var puzzle = PuzzleManager.Instance.Puzzles[4];

        var solver = new Solver();

        var result = solver.Solve(puzzle);
        
        // ReSharper disable Xunit.XunitTestWithConsoleOutput
        // Will fix later.
        Console.WriteLine();
        
        Console.WriteLine($"Solved: {result}.");

        Console.WriteLine();
        
        Console.WriteLine(solver.Grid.ToString());

        Console.WriteLine();
    }
}