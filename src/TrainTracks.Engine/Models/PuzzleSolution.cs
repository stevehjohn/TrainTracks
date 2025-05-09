// ReSharper disable UnusedAutoPropertyAccessor.Global
using System.Text.Json.Serialization;

namespace TrainTracks.Engine.Models;

public class PuzzleSolution
{
    [JsonPropertyName("type")]
    public int Type { get; set; }
    
    [JsonPropertyName("variant")]
    public int Variant { get; set; }
    
    [JsonPropertyName("year")]
    public int Year { get; set; }
    
    [JsonPropertyName("month")]
    public int Month { get; set; }
    
    [JsonPropertyName("day")]
    public int Day { get; set; }
    
    [JsonPropertyName("score")]
    public int Score { get; set; }
    
    [JsonPropertyName("solution")]
    public string Solution { get; set; }
    
    [JsonPropertyName("userID")]
    public int UserId { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; }
}