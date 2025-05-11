using TrainTracks.Engine.Board;
using TrainTracks.Engine.Logic;

namespace TrainTracks.Engine;

public class Solver
{
    private readonly Preprocessor _preprocessor = new();
    
    public Grid Grid { get; private set; }

    public Action<Grid> StepCallback { get; init; }

    public bool Solve(Grid grid)
    {
        Grid = grid;
        
        _preprocessor.Preprocess(grid);

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

}