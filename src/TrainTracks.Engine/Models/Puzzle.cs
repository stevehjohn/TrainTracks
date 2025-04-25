using JetBrains.Annotations;

namespace TrainTracks.Engine.Models;

[UsedImplicitly]
public class Puzzle
{
    public int GridWidth { get; set; }
    
    public int GridHeight { get; set; }
    
    public Data Data { get; set; }
}