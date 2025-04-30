using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    private Grid _grid;
    
    // TODO: Should return a copy.
    public Grid Grid => _grid;
    
    public Action<Grid> StepCallback { get; init; }
    
    public bool Solve(Grid grid)
    {
        _grid = grid;
        
        _visited = [];
        
        // TODO: Maybe check for continuity when PlaceNextMove returns.
        
        return PlaceNextMove(_grid.Entry);
    }

    private bool PlaceNextMove(Point position)
    {
        var currentPiece = _grid[position];

        foreach (var direction in Connector.GetDirections(currentPiece))
        {
            var newPosition = new Point(position.X + direction.Dx, position.Y + direction.Dy);

            if (newPosition.X < 0 || newPosition.X > _grid.Right || newPosition.Y < 0 || newPosition.Y > _grid.Bottom)
            {
                continue;
            }

            if (_grid[newPosition.X, newPosition.Y] != Piece.Empty)
            {
                // TODO: If the piece is a valid connection, pass through it and continue on.
                continue;
            }

            var connections = Connector.GetConnections(currentPiece, direction.Dx, direction.Dy);
            
            foreach (var nextPiece in connections)
            {
                _grid[newPosition] = nextPiece;

                StepCallback?.Invoke(_grid);

                if (_grid.IsComplete)
                {
                    return true;
                }

                if (_grid.IsValid && PlaceNextMove(newPosition))
                {
                    return true;
                }

                _grid[newPosition] = Piece.Empty;
            }
        }

        return false;
    }
}