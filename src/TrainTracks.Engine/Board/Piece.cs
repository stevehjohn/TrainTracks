namespace TrainTracks.Engine.Board;

public enum Piece : byte
{
    Empty = 0,
    Cross = 1,
    Horizontal = 3,
    Vertical = 4,
    NorthEast = 5,
    SouthEast = 6,
    SouthWest = 7,
    NorthWest = 8,
    Placeholder = 9
}