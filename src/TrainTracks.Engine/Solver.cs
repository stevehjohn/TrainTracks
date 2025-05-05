using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    public Grid Grid { get; private set; }

    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        Grid = grid;
        
        PrefillCrosses();
        
        PrefillKnownPieces();
        
        return PlaceNextMove(Grid.Entry, null);
    }

    private bool PlaceNextMove(Point position, (int Dx, int Dy)? fromDirection)
    {
        var currentPiece = Grid[position];

        var directions = Connector.Directions[currentPiece];

        foreach (var direction in directions)
        {
            if (fromDirection != null && direction.Dx == -fromDirection.Value.Dx && direction.Dy == -fromDirection.Value.Dy)
            {
                continue;
            }

            var newPosition = new Point(position.X + direction.Dx, position.Y + direction.Dy);

            if (newPosition.X < 0 || newPosition.X > Grid.Right || newPosition.Y < 0 || newPosition.Y > Grid.Bottom)
            {
                continue;
            }

            if (Grid[newPosition] == Piece.Empty)
            {
                if (Grid.GetRowCount(newPosition.Y) + 1 > Grid.RowConstraints[newPosition.Y]
                    || Grid.GetColumnCount(newPosition.X) + 1 > Grid.ColumnConstraints[newPosition.X])
                {
                    continue;
                }
            }

            var connections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);

            var nextCell = Grid[newPosition];

            if (nextCell == Piece.Cross)
            {
                continue;
            }

            foreach (var nextPiece in connections)
            {
                if (nextCell != Piece.Empty)
                {
                    var backConnections = Connector.GetConnections(nextCell, -direction.Dx, -direction.Dy);
                
                    if (! backConnections.Contains(nextPiece))
                    {
                        continue;
                    }

                    if (PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                    {
                        return true;
                    }

                    continue;                
                }

                if (WouldExitBoard(newPosition, nextPiece))
                {
                    continue;
                }

                Grid[newPosition] = nextPiece;

                StepCallback?.Invoke(Grid);

                if (Grid.IsComplete)
                {
                    return true;
                }

                if (PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                {
                    return true;
                }

                Grid[newPosition] = Piece.Empty;
            }
        }

        return false;
    }

    private bool WouldExitBoard(Point position, Piece piece)
    {
                
        if (position.Y == Grid.Bottom && piece is Piece.Vertical or Piece.SouthEast or Piece.SouthWest)
        {
            return true;
        }
                
        if (position.Y == 0 && piece is Piece.Vertical or Piece.NorthEast or Piece.NorthWest)
        {
            return true;
        }
                
        if (position.X == Grid.Right && piece is Piece.Horizontal or Piece.NorthEast or Piece.SouthEast)
        {
            return true;
        }
                
        if (position.X == 0 && piece is Piece.Horizontal or Piece.NorthWest or Piece.SouthWest)
        {
            return true;
        }

        return false;
    }

    private void PrefillCrosses()
    {
        if (Grid.GetRowRemaining(0) == 2)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                if (Grid[x, 1] is Piece.Vertical or Piece.NorthWest or Piece.NorthEast)
                {
                    PlaceRowCrosses(0, x);
                }
            }
        }
    }

    private void PlaceRowCrosses(int y, int candidate)
    {
        for (var x = 0; x < Grid.Width; x++)
        {
            if (Grid[x, y] == Piece.Empty && Math.Abs(x - candidate) > 1)
            {
                Grid[x, y] = Piece.Cross;
            }
        }
    }

    private void PrefillKnownPieces()
    {
        for (var y = 0; y < Grid.Height; y++)
        {
            if (Grid[0, y] == Piece.NorthEast)
            {
                Grid[0, y - 1] = Piece.SouthEast;
            }
            
            if (Grid[0, y] == Piece.SouthEast)
            {
                Grid[0, y + 1] = Piece.NorthEast;
            }

            if (Grid[Grid.Right, y] == Piece.NorthWest)
            {
                Grid[Grid.Right, y - 1] = Piece.SouthWest;
            }
            
            if (Grid[Grid.Right, y] == Piece.SouthWest)
            {
                Grid[Grid.Right, y + 1] = Piece.NorthWest;
            }
        }
    }
}