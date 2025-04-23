using JetBrains.Annotations;

namespace TrainTracks.Engine.Models;

[UsedImplicitly]
public class Data
{
    public int[] StartingGrid { get; set; } = [];

    public int[] HorizontalClues { get; set; } = [];

    public int[] VerticalClues { get; set; } = [];
}