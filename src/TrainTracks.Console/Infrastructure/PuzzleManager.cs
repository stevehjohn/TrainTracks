using System.Text.Json;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Models;

namespace TrainTracks.Console.Infrastructure;

public class PuzzleManager
{
    public IReadOnlyList<Grid> Puzzles { get; private set; }

    private static readonly Lazy<PuzzleManager> Lazy = new(GetPuzzleManager);

    public static PuzzleManager Instance => Lazy.Value;

    private PuzzleManager()
    {
    }

    private static PuzzleManager GetPuzzleManager()
    {
        var puzzleJson = File.ReadAllText("Data/Puzzles.json");

        var puzzles = JsonSerializer.Deserialize<Puzzle[]>(puzzleJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var grids = new List<Grid>();

        foreach (var puzzle in puzzles)
        {
            var grid = new Grid(puzzle);

            grids.Add(grid);
        }
        
        var instance = new PuzzleManager
        {
            Puzzles = grids
        };
        
        return instance;
    }
}