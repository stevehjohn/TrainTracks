using TrainTracks.Engine.Models;

namespace TrainTracks.Engine.Board;

public class Grid
{
    private Piece[,] _pieces;

    public int Width { get; private set; }
    
    public int Height { get; private set; }
    
    public Grid(Puzzle puzzle)
    {
        Initialise(puzzle);

        Width = puzzle.GridWidth;

        Height = puzzle.GridHeight;

        _pieces = new Piece[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
            }
        }
    }

    private void Initialise(Puzzle puzzle)
    {
    }
}