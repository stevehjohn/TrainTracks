using TrainTracks.Engine.Infrastructure;

namespace TrainTracks.Engine.Tests.Board;

public class GridTests
{
    [Theory]
    [InlineData(0, 5, 0, 11, 10)]
    [InlineData(1, 6, 10, 0, 9)]
    [InlineData(2, 9, 0, 10, 8)]
    [InlineData(3, 4, 11, 0, 10)]
    public void IdentifiesEntryAndExitPoints(int puzzleIndex, int entryX, int entryY, int exitX, int exitY)
    {
        PuzzleManager.Path = "Test Data/puzzles.json";
        
        var puzzle = PuzzleManager.Instance.GetPuzzle(puzzleIndex);
        
        Assert.Equal(entryX, puzzle.Entry.X);
        
        Assert.Equal(entryY, puzzle.Entry.Y);
        
        Assert.Equal(exitX, puzzle.Exit.X);
        
        Assert.Equal(exitY, puzzle.Exit.Y);
    }
}