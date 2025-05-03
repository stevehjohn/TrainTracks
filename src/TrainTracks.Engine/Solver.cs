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

            foreach (var nextPiece in connections)
            {
                var nextCell = Grid[newPosition];
            
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

                StepCallback?.Invoke(Grid);
            }
        }

        return false;
    }
}