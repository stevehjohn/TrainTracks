using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    // TODO: Should return a copy.
    public Grid Grid { get; private set; }

    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        Grid = grid;
        
        // TODO: Maybe check for continuity when PlaceNextMove returns.
        
        return PlaceNextMove(Grid.Entry);
    }

    private bool PlaceNextMove(Point position)
    {
        var currentPiece = Grid[position];

        foreach (var direction in Connector.GetDirections(currentPiece))
        {
            var newPosition = new Point(position.X + direction.Dx, position.Y + direction.Dy);

            if (newPosition.X < 0 || newPosition.X > Grid.Right || newPosition.Y < 0 || newPosition.Y > Grid.Bottom)
            {
                continue;
            }

            if (Grid[newPosition.X, newPosition.Y] != Piece.Empty)
            {
                // TODO: If the piece is a valid connection, pass through it and continue on.
                continue;
            }

            var connections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);
            
            foreach (var nextPiece in connections)
            {
                Grid[newPosition] = nextPiece;

                StepCallback?.Invoke(Grid);

                if (Grid.IsComplete)
                {
                    return true;
                }

                if (Grid.IsValid && PlaceNextMove(newPosition))
                {
                    return true;
                }

                Grid[newPosition] = Piece.Empty;
            }
        }

        return false;
    }
}