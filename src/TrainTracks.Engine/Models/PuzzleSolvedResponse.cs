using System.Text.Json.Serialization;

namespace TrainTracks.Engine.Models;

public class PuzzleSolvedResponse
{
    [JsonPropertyName("global_leaderboard")]
    public LeaderboardEntry[] GlobalLeaderboard { get; init; }
}