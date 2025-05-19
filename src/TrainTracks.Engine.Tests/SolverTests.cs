using TrainTracks.Engine.Infrastructure;

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
        
        Assert.True(result);
    }
}