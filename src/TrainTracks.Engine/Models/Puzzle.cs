// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

using JetBrains.Annotations;

namespace TrainTracks.Engine.Models;

[UsedImplicitly]
public class Puzzle
{
    public int GridWidth { get; set; }
    
    public int GridHeight { get; set; }
    
    public Data Data { get; set; }
    
    public Source Source { get; set; }
}