using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    private Grid _grid;
    
    private Point _position;

    private HashSet<(int X, int Y, Piece Piece)> _visited;
    
    public bool Solve(Grid grid)
    {
        _grid = grid;

        _position = _grid.Entry;

        _visited = [];
        
        return PlaceNextMove();
    }

    private bool PlaceNextMove()
    {
        var currentPiece = _grid[_position.X, _position.Y];

        foreach (var direction in Connector.Directions)
        {
            var mewPosition = new Point(_position.X + direction.Dx, _position.Y + direction.Dy);
            
            foreach (var nextPiece in Connector.GetConnections(currentPiece, direction.Dx, direction.Dy))
            {
                if (_visited.Add((mewPosition.X, mewPosition.Y, nextPiece)))
                {
                    _grid[mewPosition.X, mewPosition.Y] = nextPiece;

                    if (_grid.IsComplete)
                    {
                        return true;
                    }

                    if (_grid.IsValid)
                    {
                        PlaceNextMove();
                    }
                }
            }
        }

        return false;
    }
}