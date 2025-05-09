using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    public Grid Grid { get; private set; }

    public Action<Grid> StepCallback { get; init; }

    public bool Solve(Grid grid)
    {
        Grid = grid;

        PopulateCrosses();

        var copy = Grid.Clone();

        PopulateImpliedCrosses(copy);

        PlaceObviousPieces(copy);

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

    private void PopulateCrosses()
    {
        for (var x = 0; x < Grid.Width; x++)
        {
            if (Grid.GetColumnCount(x) == Grid.ColumnConstraints[x])
            {
                for (var y = 0; y < Grid.Height; y++)
                {
                    if (Grid[x, y] == Piece.Empty)
                    {
                        Grid[x, y] = Piece.Cross;
                    }
                }
            }
        }

        for (var y = 0; y < Grid.Height; y++)
        {
            if (Grid.GetRowCount(y) == Grid.RowConstraints[y])
            {
                for (var x = 0; x < Grid.Width; x++)
                {
                    if (Grid[x, y] == Piece.Empty)
                    {
                        Grid[x, y] = Piece.Cross;
                    }
                }
            }
        }
    }

    private void PopulateImpliedCrosses(Grid copy)
    {
        for (var x = 0; x < Grid.Width; x++)
        {
            for (var y = 0; y < Grid.Height; y++)
            {
                var piece = copy[x, y];

                if (piece != Piece.Empty)
                {
                    switch (piece)
                    {
                        case Piece.Horizontal:
                            copy[x - 1, y] = Piece.Placeholder;
                            copy[x + 1, y] = Piece.Placeholder;
                            break;

                        case Piece.Vertical:
                            copy[x, y - 1] = Piece.Placeholder;
                            copy[x, y + 1] = Piece.Placeholder;
                            break;

                        case Piece.NorthEast:
                            copy[x + 1, y] = Piece.Placeholder;
                            copy[x, y - 1] = Piece.Placeholder;
                            break;

                        case Piece.SouthEast:
                            copy[x, y + 1] = Piece.Placeholder;
                            copy[x + 1, y] = Piece.Placeholder;
                            break;

                        case Piece.SouthWest:
                            copy[x, y + 1] = Piece.Placeholder;
                            copy[x - 1, y] = Piece.Placeholder;
                            break;

                        case Piece.NorthWest:
                            copy[x, y - 1] = Piece.Placeholder;
                            copy[x - 1, y] = Piece.Placeholder;
                            break;
                    }
                }
            }
        }

        for (var x = 0; x < copy.Width; x++)
        {
            if (copy.GetColumnCount(x) == copy.ColumnConstraints[x])
            {
                for (var y = 0; y < copy.Height; y++)
                {
                    if (copy[x, y] == Piece.Empty)
                    {
                        Grid[x, y] = Piece.Cross;
                    }
                }
            }
        }

        for (var y = 0; y < copy.Height; y++)
        {
            if (copy.GetRowCount(y) == copy.RowConstraints[y])
            {
                for (var x = 0; x < copy.Width; x++)
                {
                    if (copy[x, y] == Piece.Empty)
                    {
                        Grid[x, y] = Piece.Cross;
                    }
                }
            }
        }
    }

    private void PlaceObviousPieces(Grid copy)
    {
        for (var x = 0; x < Grid.Width; x++)
        {
            if (copy.GetColumnRemaining(x) == 0)
            {
                for (var y = 0; y < Grid.Height; y++)
                {
                    if (Grid[x, y] == Piece.NorthWest)
                    {
                        if (x == 0 || Grid[x - 1, y - 1] == Piece.Cross)
                        {
                            Grid[x, y - 1] = Piece.SouthEast;
                        }

                        if (x == Grid.Right || Grid[x + 1, y - 1] == Piece.Cross)
                        {
                            Grid[x, y - 1] = Piece.SouthWest;
                        }
                    }
                }
            }
        }

        for (var x = 1; x < Grid.Right; x++)
        {
            for (var y = 1; y < Grid.Bottom; y++)
            {
                if (Grid[x, y] == Piece.Empty)
                {
                    if (Grid[x - 1, y] == Piece.Cross && Grid[x + 1, y] == Piece.Cross)
                    {
                        Grid[x, y] = Piece.Vertical;
                    }

                    if (Grid[x, y - 1] == Piece.Cross && Grid[x, y + 1] == Piece.Cross)
                    {
                        Grid[x, y] = Piece.Horizontal;
                    }
                }
            }
        }
    }
}