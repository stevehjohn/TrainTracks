using System.Net;
using System.Text.Json;
using HtmlAgilityPack;
using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Models;

namespace TrainTracks.Engine.Infrastructure;

public sealed class PuzzleClient : IDisposable
{
    private const string BaseUri = "https://puzzlemadness.co.uk/";

    private readonly HttpClientHandler _handler;
    
    private readonly HttpClient _client;

    public PuzzleClient()
    {
        var cookieContainer = new CookieContainer();
        
        _handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer
        };

        var lines = File.ReadAllLines("cookies.txt");

        foreach (var line in lines)
        {
            var parts = line.Split('=');
        
            cookieContainer.Add(new Uri(BaseUri), new Cookie(parts[0], parts[1]));
        }
        
        _client = new HttpClient(_handler)
        {
            BaseAddress = new Uri(BaseUri)
        };
    }
    
    public (DateOnly Date, Grid Grid)? GetNextPuzzle(Difficulty difficulty)
    {
        var nextPuzzleDate = GetOldestIncompletePuzzleDate(difficulty);

        if (nextPuzzleDate == null)
        {
            return null;
        }
        
        var year = nextPuzzleDate.Value.Year;
        
        var month = nextPuzzleDate.Value.Month;
        
        var day = nextPuzzleDate.Value.Day;
        
        using var response = _client.GetAsync($"traintracks/small/{year}/{month}/{day}").Result;
            
        var page = response.Content.ReadAsStringAsync().Result;

        var puzzleJson = page.Substring(page.IndexOf("puzzleData = ", StringComparison.InvariantCultureIgnoreCase) + 13);
        
        puzzleJson = puzzleJson[..puzzleJson.IndexOf(";", StringComparison.InvariantCultureIgnoreCase)];
        
        var puzzle = JsonSerializer.Deserialize<Puzzle>(puzzleJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return (nextPuzzleDate.Value, new Grid(puzzle));
    }

    public void SendResult(DateOnly date, Grid grid)
    {
        // TODO: Properly use JSON.
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
        var content = new StringContent($"{{ \"type\":33,\"variant\":0,\"year\":{date.Year},\"month\":{date.Month},\"day\":{date.Day},\"score\":75,\"solution\":\"116371168053681111571111157063115381\",\"userID\":30308,\"status\":\"PENDING\",\"createdAt\":{timestamp}}}");
        
        using var response = _client.PostAsync("user/puzzlecomplete", content).Result;
            
    }

    private DateOnly? GetOldestIncompletePuzzleDate(Difficulty difficulty)
    {
        var now = DateTime.Now;

        for (var year = 2005; year <= now.Year; year++)
        {
            using var response = _client.GetAsync($"/archive/traintracks/{difficulty.ToString().ToLower()}/{year}").Result;
            
            var page = response.Content.ReadAsStringAsync().Result;

            var dom = new HtmlDocument();
            
            dom.LoadHtml(page);
            
            var puzzles = dom.DocumentNode.SelectNodes("//td[@class='puzzleNotDone']");

            if (puzzles != null && puzzles.Count > 0)
            {
                var puzzle = puzzles[0];
                
                var id =puzzle.Attributes["id"].Value;

                var parts = id.Split('-');
                
                return new DateOnly(int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]));
            }
        }

        return null;   
    }

    public void Dispose()
    {
        _handler?.Dispose();
        
        _client?.Dispose();
    }
}