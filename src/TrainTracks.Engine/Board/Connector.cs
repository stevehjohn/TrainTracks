namespace TrainTracks.Engine.Board;

public static class Connector
{
    private static readonly List<Piece> _connections = [];
    
    public static readonly List<(int Dx, int Dy)> Directions =
    [
        (0, -1),
        (1, 0),
        (0, 1),
        (-1, 0)
    ];
    
    public static IReadOnlyList<Piece> GetConnections(Piece piece, int dX, int dY)
    {
        _connections.Clear();


    switch (piece)
    {
        case Piece.Horizontal:
            switch (dX, dY)
            {
                case (1, 0):
                    _connections.Add(Piece.Horizontal);
                    _connections.Add(Piece.NorthWest);
                    _connections.Add(Piece.SouthWest);
                    break;
                case (-1, 0):
                    _connections.Add(Piece.Horizontal);
                    _connections.Add(Piece.NorthEast);
                    _connections.Add(Piece.SouthEast);
                    break;
            }
            break;

        case Piece.Vertical:
            switch (dX, dY)
            {
                case (0, -1):
                    _connections.Add(Piece.Vertical);
                    _connections.Add(Piece.SouthEast);
                    _connections.Add(Piece.SouthWest);
                    break;
                case (0, 1):
                    _connections.Add(Piece.Vertical);
                    _connections.Add(Piece.NorthEast);
                    _connections.Add(Piece.NorthWest);
                    break;
            }
            break;

        case Piece.NorthEast:
            switch (dX, dY)
            {
                case (0, 1): 
                    _connections.Add(Piece.Horizontal);
                    _connections.Add(Piece.SouthEast);
                    break;
                case (-1, 0):
                    _connections.Add(Piece.Vertical);
                    _connections.Add(Piece.NorthWest);
                    break;
            }
            break;

        case Piece.SouthEast:
            switch (dX, dY)
            {
                case (0, -1):
                    _connections.Add(Piece.Horizontal);
                    _connections.Add(Piece.NorthEast);
                    break;
                case (-1, 0):
                    _connections.Add(Piece.Vertical);
                    _connections.Add(Piece.SouthWest);
                    break;
            }
            break;

        case Piece.SouthWest:
            switch (dX, dY)
            {
                case (0, -1):
                    _connections.Add(Piece.Horizontal);
                    _connections.Add(Piece.NorthWest);
                    break;
                case (1, 0):
                    _connections.Add(Piece.Vertical);
                    _connections.Add(Piece.SouthEast);
                    break;
            }
            break;

        case Piece.NorthWest:
            switch (dX, dY)
            {
                case (0, 1):
                    _connections.Add(Piece.Horizontal);
                    _connections.Add(Piece.SouthWest);
                    break;
                case (1, 0):
                    _connections.Add(Piece.Vertical);
                    _connections.Add(Piece.NorthEast);
                    break;
            }
            break;
    }        
        return _connections;
    }
}