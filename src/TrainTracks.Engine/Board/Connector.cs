namespace TrainTracks.Engine.Board;

public static class Connector
{
    public static readonly Dictionary<Piece, (int Dx, int Dy)[]> Directions = new()
    {
        { Piece.Horizontal, [(-1, 0), (1, 0)] },
        { Piece.Vertical, [(0, -1), (0, 1)] },
        { Piece.NorthEast, [(1, 0), (0, -1)] },
        { Piece.SouthEast, [(1, 0), (0, 1)] },
        { Piece.SouthWest, [(-1, 0), (0, 1)] },
        { Piece.NorthWest, [(-1, 0), (0, -1)] }
    };

    private static readonly Dictionary<int, Piece[]> Connections = new()
    {
        { 0, [Piece.Horizontal, Piece.NorthWest, Piece.SouthWest] },
        { 1, [Piece.Horizontal, Piece.NorthEast, Piece.SouthEast] },
        { 2, [Piece.Vertical, Piece.SouthEast, Piece.SouthWest] },
        { 3, [Piece.Vertical, Piece.NorthEast, Piece.NorthWest] },
        { 4, [Piece.Vertical, Piece.SouthEast, Piece.SouthWest] },
        { 5, [Piece.Horizontal, Piece.NorthWest, Piece.SouthWest] },
        { 6, [Piece.Vertical, Piece.NorthEast, Piece.NorthWest] },
        { 7, [Piece.Horizontal, Piece.NorthWest, Piece.SouthWest] },
        { 8, [Piece.Vertical, Piece.NorthWest, Piece.NorthEast] },
        { 9, [Piece.Horizontal, Piece.NorthEast, Piece.SouthEast] },
        { 10, [Piece.Horizontal, Piece.NorthEast, Piece.SouthEast] },
        { 11, [Piece.Vertical, Piece.SouthEast, Piece.SouthWest] }
    };

    public static IReadOnlyList<Piece> GetConnections(Piece piece, int dX, int dY)
    {
        switch (piece)
        {
            case Piece.Horizontal:
                switch (dX, dY)
                {
                    case (1, 0):
                        return Connections[0];
                    case (-1, 0):
                        return Connections[1];
                }

                break;

            case Piece.Vertical:
                switch (dX, dY)
                {
                    case (0, -1):
                        return Connections[2];
                    case (0, 1):
                        return Connections[3];
                }

                break;

            case Piece.NorthEast:
                switch (dX, dY)
                {
                    case (0, -1):
                        return Connections[4];
                    case (1, 0):
                        return Connections[5];
                }

                break;

            case Piece.SouthEast:
                switch (dX, dY)
                {
                    case (0, 1):
                        return Connections[6];
                    case (1, 0):
                        return Connections[7];
                }

                break;

            case Piece.SouthWest:
                switch (dX, dY)
                {
                    case (0, 1):
                        return Connections[8];
                    case (-1, 0):
                        return Connections[9];
                }

                break;

            case Piece.NorthWest:
                switch (dX, dY)
                {
                    case (-1, 0):
                        return Connections[10];
                    case (0, -1):
                        return Connections[11];
                }

                break;

            case Piece.Empty:
            case Piece.Cross:
            default:
                throw new ArgumentOutOfRangeException(nameof(piece), piece, null);
        }

        return [];
    }
}