using TrainTracks.Engine.Tests.Infrastructure;

namespace TrainTracks.Engine.Tests;

public class SolverTests
{
    [Fact]
    public void CanSolvePuzzle()
    {
        var puzzle = PuzzleManager.Instance.Puzzles[0];

        var solver = new Solver();

        var result = solver.Solve(puzzle);
        
        Console.WriteLine(solver.Grid.ToString());
    }
}