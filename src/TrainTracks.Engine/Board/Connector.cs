namespace TrainTracks.Engine.Board;

public static class Connector
{
    public static IReadOnlyList<(int Dx, int Dy)> GetDirections(Piece piece)
    {
        var directions = new List<(int Dx, int Dy)>();

        switch (piece)
        {
            case Piece.Horizontal:
                directions.Add((-1, 0));
                directions.Add((1, 0));
                break;

            case Piece.Vertical:
                directions.Add((0, -1));
                directions.Add((0, 1));
                break;

            case Piece.NorthEast:
                directions.Add((-1, 0));
                directions.Add((0, 1));
                break;


            case Piece.SouthEast:
                directions.Add((1, 0));
                directions.Add((0, 1));
                break;

            case Piece.SouthWest:
                directions.Add((1, 0));
                directions.Add((0, -1));
                break;
            
            case Piece.NorthWest:
                directions.Add((-1, 0));
                directions.Add((0, -1));
                break;
        }

        return directions;
    }

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