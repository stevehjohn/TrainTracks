using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    // TODO: Should probably return a copy.
    public Grid Grid { get; private set; }

    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        Grid = grid;
        
        // TODO: Maybe check for continuity when PlaceNextMove returns.
        
        return PlaceNextMove(Grid.Entry, null);
    }

    private bool PlaceNextMove(Point position, (int Dx, int Dy)? fromDirection)
    {
        var currentPiece = Grid[position];

        var directions = Connector.GetDirections(currentPiece);

        if (fromDirection != null)
        {
            directions.Remove((-fromDirection.Value.Dx, -fromDirection.Value.Dy));
        }

        foreach (var direction in directions)
        {
            var newPosition = new Point(position.X + direction.Dx, position.Y + direction.Dy);

            if (newPosition.X < 0 || newPosition.X > Grid.Right || newPosition.Y < 0 || newPosition.Y > Grid.Bottom)
            {
                continue;
            }

            var connections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);

            var nextCell = Grid[newPosition];
            
            if (nextCell != Piece.Empty)
            {
                // This may not be the correct logic. Check it.
                if (! connections.Contains(nextCell))
                {
                    continue;
                }

                if (PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                {
                    return true;
                }

                continue;                
            }

            foreach (var nextPiece in connections)
            {
                Grid[newPosition] = nextPiece;

                StepCallback?.Invoke(Grid);

                if (Grid.IsComplete)
                {
                    return true;
                }

                if (Grid.IsValid && PlaceNextMove(newPosition, (direction.Dx, direction.Dy)))
                {
                    return true;
                }

                Grid[newPosition] = Piece.Empty;
            }
        }

        return false;
    }
}