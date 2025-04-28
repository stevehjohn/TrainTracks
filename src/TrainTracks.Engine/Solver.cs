using TrainTracks.Engine.Board;

namespace TrainTracks.Engine;

public class Solver
{
    private Grid _grid;
    
    private Point _position;
    
    public void Solve(Grid grid)
    {
        _grid = grid;

        _position = _grid.Entry;
    }
}