// ReSharper disable Xunit.XunitTestWithConsoleOutput
using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void CanSolvePuzzle()
    {
        PuzzleManager.Path = "Test Data/puzzles.json";

        for (var i = 0; i < 19; i++)
        {
            var puzzle = PuzzleManager.Instance.GetPuzzle(i);

            var solver = new Solver();

            Console.WriteLine($"Solving puzzle number: {i} ({puzzle.Width}x{puzzle.Height})");
            
            var result = solver.Solve(puzzle);
            
            Console.WriteLine(puzzle.ToString());

            Assert.True(result);
        }
    }
}