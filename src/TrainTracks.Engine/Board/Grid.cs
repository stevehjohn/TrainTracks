using System.Text;
using TrainTracks.Engine.Models;

namespace TrainTracks.Engine.Board;

public class Grid
{
    private Piece[,] _pieces;

    private int[] _rowCounts;
    
    private int[] _columnCounts;

    public Piece this[int x, int y]
    {
        get => _pieces[x, y];
        set 
        {
            if (x < 0 || x > Right || y < 0 || y > Bottom)
            {
                return;
            }

            if (value != Piece.Empty && value != Piece.Cross && _pieces[x, y] == Piece.Empty)
            {
                _columnCounts[x]++;
                
                _rowCounts[y]++;
            }
            
            if ((value == Piece.Empty || value != Piece.Cross) && _pieces[x, y] != Piece.Empty)
            {
                _columnCounts[x]--;
                
                _rowCounts[y]--;
            }
            
            _pieces [x, y] = value;
        }
    }

    public Piece this[Point position]
    {
        get => this[position.X, position.Y];
        set => this[position.X, position.Y] = value;
    }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Bottom { get; private set; }

    public int Right { get; private set; }

    public int TotalPieces { get; private set; }
    
    public int FixedPieces { get; private set; }

    public int[] RowConstraints { get; private set; }

    public int[] ColumnConstraints { get; private set; }

    public Point Entry { get; private set; } = new Point(-1, -1);

    public Point Exit { get; private set; }

    public bool IsComplete => ConstraintsAreMet() && PathIsContinuous();

    public Grid(Puzzle puzzle)
    {
        Initialise(puzzle);
    }

    public Grid Clone()
    {
        var copy = new Grid
        {
            Width = Width,
            Height = Height,
            Right = Right,
            Bottom = Bottom,
            Entry = new Point(Entry),
            Exit = new Point(Exit),
            RowConstraints = new int[Height],
            ColumnConstraints = new int[Width],
            TotalPieces = TotalPieces
        };

        Array.Copy(RowConstraints, copy.RowConstraints, Height);

        Array.Copy(ColumnConstraints, copy.ColumnConstraints, Width);

        copy._pieces = new Piece[Width, Height];

        Array.Copy(_pieces, copy._pieces, Width * Height);
        
        copy._rowCounts = new int[Height];
        
        Array.Copy(_rowCounts, copy._rowCounts, Height);
        
        copy._columnCounts = new int[Width];
        
        Array.Copy(_columnCounts, copy._columnCounts, Width);

        return copy;
    }

    private Grid()
    {
    }

    public int GetColumnCount(int x) => _columnCounts[x];
    
    public int GetRowCount(int y) => _rowCounts[y];
    
    public int GetRowRemaining(int y) => RowConstraints[y] - _rowCounts[y];
    
    public int GetColumnRemaining(int x) => ColumnConstraints[x] - _columnCounts[x];
    
    private void Initialise(Puzzle puzzle)
    {
        Width = puzzle.GridWidth;

        Height = puzzle.GridHeight;
        
        Right = Width - 1;
        
        Bottom = Height - 1;

        _pieces = new Piece[Width, Height];
        
        _rowCounts = new int[Height];

        _columnCounts = new int[Width];

        FixedPieces = 0;

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _pieces[x, y] = puzzle.Data.StartingGrid[y * Width + x];

                if (_pieces[x, y] != Piece.Empty)
                {
                    FixedPieces++;
                    
                    _columnCounts[x]++;
                    
                    _rowCounts[y]++;
                }
            }
        }

        RowConstraints = puzzle.Data.HorizontalClues;

        ColumnConstraints = puzzle.Data.VerticalClues;

        TotalPieces = 0;

        for (var x = 0; x < Width; x++)
        {
            TotalPieces += ColumnConstraints[x];
        }

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
            if (Entry.X == -1)
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
            if (_columnCounts[x] != ColumnConstraints[x])
            {
                return false;
            }
        }
        
        for (var y = 0; y < Height; y++)
        {
            if (_rowCounts[y] != RowConstraints[y])
            {
                return false;
            }
        }
        
        return true;
    }

    private bool PathIsContinuous()
    {
        var visited = new HashSet<Point>();

        Traverse(Entry, visited, null);

        return visited.Count == TotalPieces;
    }

    private void Traverse(Point position, HashSet<Point> visited, (int Dx, int Dy)? fromDirection)
    {
        if (! visited.Add(position))
        {
            return;
        }

        var directions = Connector.Directions[this[position]];

        foreach (var direction in directions)
        {
            if (fromDirection != null && direction.Dx == -fromDirection.Value.Dx && direction.Dy == -fromDirection.Value.Dy)
            {
                continue;
            }

            var nextPosition = new Point(position.X + direction.Dx, position.Y + direction.Dy);

            if (nextPosition.X < 0 || nextPosition.X > Right || nextPosition.Y < 0 || nextPosition.Y > Bottom)
            {
                continue;
            }

            var nextPiece = this[nextPosition];

            if (nextPiece == Piece.Empty)
            {
                continue;
            }

            var connections = Connector.GetConnections(this[position], direction.Dx, direction.Dy);

            if (! connections.Contains(nextPiece))
            {
                continue;
            }

            var backConnections = Connector.GetConnections(nextPiece, -direction.Dx, -direction.Dy);
            
            if (! backConnections.Contains(this[position]))
            {
                continue;
            }

            Traverse(nextPosition, visited, direction);
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                switch (_pieces[x, y])
                {
                    case Piece.Horizontal:
                        builder.Append('─');
                        break;

                    case Piece.Vertical:
                        builder.Append('│');
                        break;

                    case Piece.NorthEast:
                        builder.Append('└');
                        break;

                    case Piece.SouthEast:
                        builder.Append('┌');
                        break;

                    case Piece.NorthWest:
                        builder.Append('┘');
                        break;

                    case Piece.SouthWest:
                        builder.Append('┐');
                        break;
                    
                    case Piece.Cross:
                        builder.Append('⨉');
                        break;

                    case Piece.Empty:
                    default:
                        builder.Append(' ');
                        break;
                }
            }

            builder.AppendLine();
        }

        return builder.ToString();
    }
}