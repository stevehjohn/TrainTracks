using System.Diagnostics.CodeAnalysis;

namespace TrainTracks.Engine.Board;

public class Point
{
    public int X { get; }
    
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        
        Y = y;
    }

    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}