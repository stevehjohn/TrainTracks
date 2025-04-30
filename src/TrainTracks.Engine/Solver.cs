using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    private Grid _grid;
    
    private HashSet<(int X, int Y, Piece Piece)> _visited;

    // TODO: Should return a copy.
    public Grid Grid => _grid;
    
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

        foreach (var direction in Connector.Directions)
        {
            var newPosition = new Point(position.X + direction.Dx, position.Y + direction.Dy);

            if (newPosition.X < 0 || newPosition.X > _grid.Right || newPosition.Y < 0 || newPosition.Y > _grid.Bottom)
            {
                continue;
            }

            if (_grid[newPosition.X, newPosition.Y] != Piece.Empty)
            {
                continue;
            }

            foreach (var nextPiece in Connector.GetConnections(currentPiece, direction.Dx, direction.Dy))
            {
                if (_visited.Add((newPosition.X, newPosition.Y, nextPiece)))
                {
                    _grid[newPosition] = nextPiece;

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
        }

        return false;
    }
}