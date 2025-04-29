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
    }
}