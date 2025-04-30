using TrainTracks.Engine.Models;

namespace TrainTracks.Engine.Board;

public class Grid
{
    private Piece[,] _pieces;

    public Piece this[int x, int y]
    {
        get => _pieces[x, y];
        set => _pieces[x, y] = value;
    }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Bottom => Height - 1;

    public int Right => Width - 1;

    public int[] RowConstraints { get; private set; }

    public int[] ColumnConstraints { get; private set; }

    public Point Entry { get; private set; }

    public Point Exit { get; private set; }

    public bool IsComplete => ConstraintsAreMet() && IsContinuous();

    // TODO: Do.
    public bool IsValid => false;

    public Grid(Puzzle puzzle)
    {
        Initialise(puzzle);
    }

    private void Initialise(Puzzle puzzle)
    {
        Width = puzzle.GridWidth;

        Height = puzzle.GridHeight;

        _pieces = new Piece[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _pieces[x, y] = puzzle.Data.StartingGrid[y * Width + x];
            }
        }

        RowConstraints = puzzle.Data.HorizontalClues;

        ColumnConstraints = puzzle.Data.VerticalClues;

        FindEntryAndExit();
    }

    private void FindEntryAndExit()
    {
        for (var x = 0; x < Width; x++)
        {
            CheckForEndpoint(x, 0);

            CheckForEndpoint(x, Bottom);
        }

        for (var y = 0; y < Height; y++)
        {
            CheckForEndpoint(0, y);

            CheckForEndpoint(Right, y);
        }
    }

    private void CheckForEndpoint(int x, int y)
    {
        var point = new Point(x, y);

        if (IsEndpoint(point))
        {
            if (Entry == null)
            {
                Entry = point;
            }
            else
            {
                Exit = point;
            }
        }
    }

    private bool IsEndpoint(Point point)
    {
        var x = point.X;

        var y = point.Y;

        var piece = this[x, y];

        if (y == 0 && (piece == Piece.NorthEast || piece == Piece.NorthWest || piece == Piece.Vertical))
        {
            return true;
        }

        if (y == Bottom && (piece == Piece.SouthEast || piece == Piece.SouthWest || piece == Piece.Vertical))
        {
            return true;
        }

        if (x == 0 && (piece == Piece.NorthWest || piece == Piece.SouthWest || piece == Piece.Horizontal))
        {
            return true;
        }

        if (x == Right && (piece == Piece.NorthEast || piece == Piece.SouthEast || piece == Piece.Horizontal))
        {
            return true;
        }

        return false;
    }

    private bool ConstraintsAreMet()
    {
        for (var x = 0; x < Width; x++)
        {
            var sum = 0;
        
            for (var y = 0; y < Height; y++)
            {
                if (this[x, y] != Piece.Empty)
                {
                    sum++;
                }
            }

            if (sum != ColumnConstraints[x])
            {
                return false;
            }
        }

        for (var y = 0; y < Height; y++)
        {
            var sum = 0;
        
            for (var x = 0; x < Width; x++)
            {
                if (this[x, y] != Piece.Empty)
                {
                    sum++;
                }
            }

            if (sum != RowConstraints[y])
            {
                return false;
            }
        }

        return true;
    }

    private bool IsContinuous()
    {
        // TODO: Do.
        
        return true;
    }
}