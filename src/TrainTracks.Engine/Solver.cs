using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    public Grid Grid { get; private set; }

    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        Grid = grid;
        
        return PlaceNextMove(Grid.Entry, null);
    }

    private bool PlaceNextMove(Point position, (int Dx, int Dy)? fromDirection)
    {
        if (position.Equals(Grid.Exit))
        {
            return Grid.IsValid && Grid.IsComplete;
        }

        var currentPiece = Grid[position];

        var directions = Connector.GetDirections(currentPiece);

        // if we came from a direction, remove it from the list
        if (fromDirection != null)
        {
            directions.Remove((-fromDirection.Value.Dx,
                               -fromDirection.Value.Dy));
        }

        foreach (var direction in directions)
        {
            var newPosition = new Point(position.X + direction.Dx,
                                        position.Y + direction.Dy);

            if (newPosition.X < 0 || newPosition.X > Grid.Right
            || newPosition.Y < 0 || newPosition.Y > Grid.Bottom)
                continue;

            // if it's the exit square, recurse in so the base-case fires
            if (newPosition.Equals(Grid.Exit)) {
                return PlaceNextMove(newPosition, (direction.Dx, direction.Dy));
            }

            var validNext = Connector.GetConnections(currentPiece,
                                                    direction.Dx,
                                                    direction.Dy);

            var nextCell = Grid[newPosition];

            if (nextCell != Piece.Empty) {

                if (!validNext.Contains(nextCell))
                {
                    continue;
                }

                if (PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                {
                    return true;
                }
            }
            else
            {
                // unfilled – try each legal piece
                foreach (var candidate in validNext)
                {
                    Grid[newPosition] = candidate;
                    StepCallback?.Invoke(Grid);

                    if (Grid.IsValid &&
                         PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                    {
                        return true;
                    }

                    // Backtrack:
                    Grid[newPosition] = Piece.Empty;
                    StepCallback?.Invoke(Grid);
                }
            }
        }

        return false;
    }
}