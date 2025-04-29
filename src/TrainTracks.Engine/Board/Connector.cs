namespace TrainTracks.Engine.Board;

public static class Connector
{
    private static readonly List<Piece> _connections = [];
    
    public static List<(int Dx, int Dy)> Directions =
    [
        (0, -1),
        (1, 0),
        (0, 1),
        (-1, 0)
    ];
    
    public static IReadOnlyList<Piece> GetConnections(Piece piece, int dX, int dY)
    {
        _connections.Clear();
        
        return _connections;
    }
}