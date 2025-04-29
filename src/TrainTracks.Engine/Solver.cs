using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    private Grid _grid;
    
    private Point _position;

    private HashSet<(int X, int Y, Piece Piece)> _visited;
    
    public void Solve(Grid grid)
    {
        _grid = grid;

        _position = _grid.Entry;

        _visited = [];

        while (! _grid.IsComplete)
        {
            PlaceNextMove();
        }
    }

    private void PlaceNextMove()
    {
        var currentPiece = _grid[_position.X, _position.Y];

        foreach (var direction in Connector.Directions)
        {
            foreach (var nextPiece in Connector.GetConnections(currentPiece, 0, 0))
            {
            
            }
        }
    }
}