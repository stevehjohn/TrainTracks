using TrainTracks.Engine.Board;

namespace TrainTracks.Engine.Logic;

public class Preprocessor
{
    private Grid _grid;

    public void Preprocess(Grid grid)
    {
        _grid = grid;

        PopulateCrosses();

        var copy = _grid.Clone();

        PopulateImpliedCrosses(copy);

        PlaceObviousPieces();
    }

    private void PopulateCrosses()
    {
        for (var x = 0; x < _grid.Width; x++)
        {
            if (_grid.GetColumnCount(x) == _grid.ColumnConstraints[x])
            {
                for (var y = 0; y < _grid.Height; y++)
                {
                    if (_grid[x, y] == Piece.Empty)
                    {
                        _grid[x, y] = Piece.Cross;
                    }
                }
            }
        }

        for (var y = 0; y < _grid.Height; y++)
        {
            if (_grid.GetRowCount(y) == _grid.RowConstraints[y])
            {
                for (var x = 0; x < _grid.Width; x++)
                {
                    if (_grid[x, y] == Piece.Empty)
                    {
                        _grid[x, y] = Piece.Cross;
                    }
                }
            }
        }
    }

    private void PopulateImpliedCrosses(Grid copy)
    {
        for (var x = 0; x < _grid.Width; x++)
        {
            for (var y = 0; y < _grid.Height; y++)
            {
                var piece = copy[x, y];

                if (piece != Piece.Empty)
                {
                    switch (piece)
                    {
                        case Piece.Horizontal:
                            copy[x - 1, y] = Piece.Placeholder;
                            copy[x + 1, y] = Piece.Placeholder;
                            break;

                        case Piece.Vertical:
                            copy[x, y - 1] = Piece.Placeholder;
                            copy[x, y + 1] = Piece.Placeholder;
                            break;

                        case Piece.NorthEast:
                            copy[x + 1, y] = Piece.Placeholder;
                            copy[x, y - 1] = Piece.Placeholder;
                            break;

                        case Piece.SouthEast:
                            copy[x, y + 1] = Piece.Placeholder;
                            copy[x + 1, y] = Piece.Placeholder;
                            break;

                        case Piece.SouthWest:
                            copy[x, y + 1] = Piece.Placeholder;
                            copy[x - 1, y] = Piece.Placeholder;
                            break;

                        case Piece.NorthWest:
                            copy[x, y - 1] = Piece.Placeholder;
                            copy[x - 1, y] = Piece.Placeholder;
                            break;
                    }
                }
            }
        }

        for (var x = 0; x < copy.Width; x++)
        {
            if (copy.GetColumnCount(x) == copy.ColumnConstraints[x])
            {
                for (var y = 0; y < copy.Height; y++)
                {
                    if (copy[x, y] == Piece.Empty)
                    {
                        _grid[x, y] = Piece.Cross;
                    }
                }
            }
        }

        for (var y = 0; y < copy.Height; y++)
        {
            if (copy.GetRowCount(y) == copy.RowConstraints[y])
            {
                for (var x = 0; x < copy.Width; x++)
                {
                    if (copy[x, y] == Piece.Empty)
                    {
                        _grid[x, y] = Piece.Cross;
                    }
                }
            }
        }
    }

    private void PlaceObviousPieces()
    {
        if (_grid.RowConstraints[0] == 2)
        {
            for (var x = 0; x < _grid.Width; x++)
            {
                if (_grid[x, 0] is Piece.SouthEast or Piece.NorthEast)
                {
                    _grid[x + 1, 0] = Piece.SouthWest;

                    break;
                }

                if (_grid[x, 0] is Piece.SouthWest or Piece.NorthWest)
                {
                    _grid[x - 1, 0] = Piece.SouthEast;

                    break;
                }
            }
        }

        if (_grid.RowConstraints[_grid.Bottom] == 2)
        {
            for (var x = 0; x < _grid.Width; x++)
            {
                if (_grid[x, _grid.Bottom] is Piece.NorthEast or Piece.SouthEast)
                {
                    _grid[x + 1, _grid.Bottom] = Piece.NorthWest;
        
                    break;
                }
        
                if (_grid[x, _grid.Bottom] is Piece.NorthWest or Piece.SouthWest)
                {
                    _grid[x - 1, _grid.Bottom] = Piece.NorthEast;
        
                    break;
                }
            }
        }
        
        if (_grid.ColumnConstraints[0] == 2)
        {
            for (var y = 0; y < _grid.Height; y++)
            {
                if (_grid[0, y] is Piece.NorthEast or Piece.NorthWest)
                {
                    _grid[0, y - 1] = Piece.SouthEast;
        
                    break;
                }
        
                if (_grid[0, y] is Piece.SouthEast or Piece.SouthWest)
                {
                    _grid[0, y + 1] = Piece.NorthEast;
        
                    break;
                }
            }
        }
        
        if (_grid.ColumnConstraints[_grid.Right] == 2)
        {
            for (var y = 0; y < _grid.Height; y++)
            {
                if (_grid[_grid.Right, y] is Piece.SouthWest or Piece.SouthEast)
                {
                    _grid[_grid.Right, y + 1] = Piece.NorthWest;
        
                    break;
                }
        
                if (_grid[_grid.Right, y] is Piece.NorthWest or Piece.NorthEast)
                {
                    _grid[_grid.Right, y - 1] = Piece.SouthWest;
        
                    break;
                }
            }
        }
    }
}