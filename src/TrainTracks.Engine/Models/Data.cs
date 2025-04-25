// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using JetBrains.Annotations;
using TrainTracks.Engine.Board;

namespace TrainTracks.Engine.Models;

[UsedImplicitly]
public class Data
{
    public Piece[] StartingGrid { get; set; } = [];

    public int[] HorizontalClues { get; set; } = [];

    public int[] VerticalClues { get; set; } = [];
}