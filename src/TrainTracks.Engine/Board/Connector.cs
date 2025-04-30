namespace TrainTracks.Engine.Board;

public static class Connector
{
    public static readonly List<(int Dx, int Dy)> Directions =
    [
        (0, -1),
        (1, 0),
        (0, 1),
        (-1, 0)
    ];

    public static IReadOnlyList<Piece> GetConnections(Piece piece, int dX, int dY)
    {
        var connections = new List<Piece>();

        switch (piece)
        {
            case Piece.Horizontal:
                switch (dX, dY)
                {
                    case (1, 0):
                        connections.Add(Piece.Horizontal);
                        connections.Add(Piece.NorthWest);
                        connections.Add(Piece.SouthWest);
                        break;
                    case (-1, 0):
                        connections.Add(Piece.Horizontal);
                        connections.Add(Piece.NorthEast);
                        connections.Add(Piece.SouthEast);
                        break;
                }

                break;

            case Piece.Vertical:
                switch (dX, dY)
                {
                    case (0, -1):
                        connections.Add(Piece.Vertical);
                        connections.Add(Piece.SouthEast);
                        connections.Add(Piece.SouthWest);
                        break;
                    case (0, 1):
                        connections.Add(Piece.Vertical);
                        connections.Add(Piece.NorthEast);
                        connections.Add(Piece.NorthWest);
                        break;
                }

                break;

            case Piece.NorthEast:
                switch (dX, dY)
                {
                    case (0, 1):
                        connections.Add(Piece.Horizontal);
                        connections.Add(Piece.SouthEast);
                        break;
                    case (-1, 0):
                        connections.Add(Piece.Vertical);
                        connections.Add(Piece.NorthWest);
                        break;
                }

                break;

            case Piece.SouthEast:
                switch (dX, dY)
                {
                    case (0, -1):
                        connections.Add(Piece.Horizontal);
                        connections.Add(Piece.NorthEast);
                        break;
                    case (-1, 0):
                        connections.Add(Piece.Vertical);
                        connections.Add(Piece.SouthWest);
                        break;
                }

                break;

            case Piece.SouthWest:
                switch (dX, dY)
                {
                    case (0, -1):
                        connections.Add(Piece.Horizontal);
                        connections.Add(Piece.NorthWest);
                        break;
                    case (1, 0):
                        connections.Add(Piece.Vertical);
                        connections.Add(Piece.SouthEast);
                        break;
                }

                break;

            case Piece.NorthWest:
                switch (dX, dY)
                {
                    case (0, 1):
                        connections.Add(Piece.Horizontal);
                        connections.Add(Piece.SouthWest);
                        break;
                    case (1, 0):
                        connections.Add(Piece.Vertical);
                        connections.Add(Piece.NorthEast);
                        break;
                }

                break;

            case Piece.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(piece), piece, null);
        }

        return connections;
    }
}