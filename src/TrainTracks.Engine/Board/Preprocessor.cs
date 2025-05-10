namespace TrainTracks.Engine.Board;

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
                if (_grid[x, 0] is Piece.SouthEast or Piece.NorthEast or Piece.Horizontal)
                {
                    _grid[x + 1, 0] = Piece.SouthWest;

                    break;
                }

                if (_grid[x, 0] is Piece.SouthWest or Piece.NorthEast or Piece.Horizontal)
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
                if (_grid[x, _grid.Bottom] is Piece.NorthEast or Piece.SouthEast or Piece.Horizontal)
                {
                    _grid[x + 1, _grid.Bottom] = Piece.NorthWest;

                    break;
                }

                if (_grid[x, _grid.Bottom] is Piece.NorthWest or Piece.SouthEast or Piece.Horizontal)
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
                if (_grid[0, y] is Piece.NorthEast or Piece.NorthWest or Piece.Vertical)
                {
                    _grid[0, y - 1] = Piece.SouthEast;

                    break;
                }

                if (_grid[0, y] is Piece.SouthEast or Piece.SouthWest or Piece.Vertical)
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
                if (_grid[_grid.Right, y] is Piece.SouthWest or Piece.SouthEast or Piece.Vertical)
                {
                    _grid[_grid.Right, y + 1] = Piece.NorthWest;

                    break;
                }

                if (_grid[_grid.Right, y] is Piece.NorthWest or Piece.NorthEast or Piece.Vertical)
                {
                    _grid[_grid.Right, y - 1] = Piece.SouthWest;

                    break;
                }
            }
        }

        if (_grid.ColumnConstraints[_grid.Right] == 3)
        {
            for (var y = 1; y < _grid.Bottom; y++)
            {
                if (_grid[_grid.Right, y] == Piece.Vertical)
                {
                    _grid[_grid.Right, y - 1] = Piece.SouthWest;
        
                    _grid[_grid.Right, y + 1] = Piece.NorthWest;
        
                    break;
                }
            }
        }

        if (_grid.ColumnConstraints[0] == 3)
        {
            for (var y = 1; y < _grid.Bottom; y++)
            {
                if (_grid[0, y] == Piece.Vertical)
                {
                    _grid[0, y - 1] = Piece.SouthEast;
        
                    _grid[0, y + 1] = Piece.NorthEast;
        
                    break;
                }
            }
        }
        
        if (_grid.RowConstraints[0] == 3)
        {
            for (var x = 1; x < _grid.Right; x++)
            {
                if (_grid[x, 0] == Piece.Horizontal)
                {
                    _grid[x - 1, 0] = Piece.SouthEast;
        
                    _grid[x + 1, 0] = Piece.SouthWest;
        
                    break;
                }
            }
        }
        
        if (_grid.RowConstraints[_grid.Bottom] == 3)
        {
            for (var x = 1; x < _grid.Right; x++)
            {
                if (_grid[x, _grid.Bottom] == Piece.Horizontal)
                {
                    _grid[x - 1, _grid.Bottom] = Piece.NorthEast;
        
                    _grid[x + 1, _grid.Bottom] = Piece.NorthWest;
        
                    break;
                }
            }
        }

        // for (var x = 1; x < _grid.Right; x++)
        // {
        //     for (var y = 1; y < _grid.Bottom; y++)
        //     {
        //         if (_grid[x, y] == Piece.Empty &&
        //             _grid[x, y - 1] is Piece.Vertical or Piece.SouthEast or Piece.SouthWest &&
        //             _grid[x, y + 1] is Piece.Vertical or Piece.NorthEast or Piece.NorthWest)
        //         {
        //             _grid[x, y] = Piece.Vertical;
        //         }
        //
        //         if (_grid[x, y] == Piece.Empty &&
        //             _grid[x - 1, y] is Piece.Horizontal or Piece.SouthEast or Piece.NorthEast &&
        //             _grid[x + 1, y] is Piece.Horizontal or Piece.NorthWest or Piece.SouthWest)
        //         {
        //             _grid[x, y] = Piece.Horizontal;
        //         }
        //
        //         if (_grid[x, y] == Piece.Empty && _grid[x, y - 1] == Piece.Cross && _grid[x, y + 1] == Piece.Cross
        //             && (_grid[x - 1, y] is Piece.NorthEast or Piece.SouthEast or Piece.Horizontal
        //                 || _grid[x + 1, y] is Piece.NorthWest or Piece.SouthWest or Piece.Horizontal))
        //         {
        //             _grid[x, y] = Piece.Horizontal;
        //         }
        //
        //         if (_grid[x, y] == Piece.Empty && _grid[x - 1, y] == Piece.Cross && _grid[x + 1, y] == Piece.Cross
        //             && (_grid[x, y - 1] is Piece.Vertical or Piece.SouthEast or Piece.SouthWest
        //                 || _grid[x, y + 1] is Piece.Vertical or Piece.NorthEast or Piece.NorthWest))
        //         {
        //             _grid[x, y] = Piece.Vertical;
        //         }
        //     }
        // }
    }
}