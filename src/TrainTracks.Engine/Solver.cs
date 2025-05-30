using TrainTracks.Engine.Board;
using TrainTracks.Engine.Logic;

namespace TrainTracks.Engine;

public class Solver
{
    private readonly Preprocessor _preprocessor = new();

    private Grid Grid { get; set; }

    public Action<Grid> StepCallback { get; init; }

    public Action<Grid> PreprocessingCompleteCallback { get; init; }
    
    public Action<(Piece Piece, int X, int Y)> DeltaStepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        Grid = grid;
        
        _preprocessor.Preprocess(grid);
        
        PreprocessingCompleteCallback?.Invoke(Grid.Clone());

        var result = PlaceNextMove(Grid.Entry, null);

        if (result)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                for (var y = 0; y < Grid.Height; y++)
                {
                    if (Grid[x, y] == Piece.Cross)
                    {
                        Grid[x, y] = Piece.Empty;
                    }
                }
            }
        }

        return result;
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

            var nextCell = Grid[newPosition];

            if (nextCell == Piece.OutOfBounds)
            {
                continue;
            }

            if (nextCell == Piece.Empty)
            {
                if (Grid.GetRowCount(newPosition.Y) + 1 > Grid.RowConstraints[newPosition.Y]
                    || Grid.GetColumnCount(newPosition.X) + 1 > Grid.ColumnConstraints[newPosition.X])
                {
                    continue;
                }
            }

            if (nextCell == Piece.Cross)
            {
                continue;
            }

            var connections = CheckForObviousPiece(currentPiece, newPosition, direction);
            
            if (connections == null)
            {
                connections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);

                connections = FilterCandidates(newPosition, direction, connections);
            }
            else
            {
                if (connections[0] == Piece.OutOfBounds)
                {
                    continue;
                }
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
                
                DeltaStepCallback?.Invoke((nextPiece, newPosition.X, newPosition.Y));

                if (Grid.IsComplete)
                {
                    return true;
                }

                if (PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                {
                    return true;
                }

                Grid[newPosition] = Piece.Empty;
                
                DeltaStepCallback?.Invoke((Piece.Empty, newPosition.X, newPosition.Y));
            }
        }

        return false;
    }

    private IReadOnlyList<Piece> CheckForObviousPiece(Piece currentPiece, Point position, (int Dx, int Dy) direction)
    {
        var x = position.X;
        
        var y = position.Y;

        if (Grid[x, y - 1] is Piece.Vertical or Piece.SouthEast or Piece.SouthWest &&
            Grid[x, y + 1] is Piece.Vertical or Piece.NorthEast or Piece.NorthWest)
        {
            var backConnections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);

            if (backConnections.Contains(Piece.Vertical))
            {
                return [Piece.Vertical];
            }

            return [Piece.OutOfBounds];
        }

        if (Grid[x - 1, y] is Piece.Horizontal or Piece.NorthEast or Piece.SouthEast &&
            Grid[x + 1, y] is Piece.Horizontal or Piece.NorthWest or Piece.SouthWest)
        {
            var backConnections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);

            if (backConnections.Contains(Piece.Horizontal))
            {
                return [Piece.Horizontal];
            }

            return [Piece.OutOfBounds];
        }

        return null;
    }

    private IReadOnlyList<Piece> FilterCandidates(Point position, (int dX, int dY) direction, IReadOnlyList<Piece> connections)
    {
        if (Grid[position] != Piece.Empty)
        {
            return connections;
        }

        if (position.Y > 0 && Grid[position.X, position.Y - 1] is Piece.Vertical or Piece.SouthEast or Piece.SouthWest)
        {
            switch (direction)
            {
                case (1, 0):
                    return [Piece.NorthWest];

                case (-1, 0):
                    return [Piece.NorthEast];
            }
        }
        
        if (position.Y < Grid.Bottom && Grid[position.X, position.Y + 1] is Piece.Vertical or Piece.NorthEast or Piece.NorthWest)
        {
            switch (direction)
            {
                case (1, 0):
                    return [Piece.SouthWest];

                case (-1, 0):
                    return [Piece.SouthEast];
            }
        }
        
        if (position.X > 0 && Grid[position.X - 1, position.Y] is Piece.Horizontal or Piece.NorthEast or Piece.SouthEast)
        {
            switch (direction)
            {
                case (0, 1):
                    return [Piece.NorthWest];
        
                case (0, -1):
                    return [Piece.SouthWest];
            }
        }
        
        if (position.X < Grid.Right && Grid[position.X + 1, position.Y] is Piece.Horizontal or Piece.NorthWest or Piece.SouthWest)
        {
            switch (direction)
            {
                case (0, 1):
                    return [Piece.NorthEast];
        
                case (0, -1):
                    return [Piece.SouthEast];
            }
        }
        
        if (position.Y > 0 && Grid[position.X, position.Y - 1] is Piece.Vertical or Piece.SouthEast or Piece.SouthWest)
        {
            switch (direction)
            {
                case (0, -1):
                    return [Piece.Vertical];
            }
        }
        
        if (position.Y < Grid.Bottom && Grid[position.X, position.Y + 1] is Piece.Vertical or Piece.NorthEast or Piece.NorthWest)
        {
            switch (direction)
            {
                case (0, 1):
                    return [Piece.Vertical];
            }
        }
        
        if (position.X < Grid.Right && Grid[position.X + 1, position.Y] is Piece.Horizontal or Piece.NorthWest or Piece.SouthWest)
        {
            switch (direction)
            {
                case (1, 0):
                    return [Piece.Horizontal];
            }
        }
        
        if (position.X > 0 && Grid[position.X - 1, position.Y] is Piece.Horizontal or Piece.NorthEast or Piece.SouthEast)
        {
            switch (direction)
            {
                case (-1, 0):
                    return [Piece.Horizontal];
            }
        }

        return connections;
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
}