using System.Net;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using TrainTracks.Engine.Board;
using TrainTracks.Engine.Models;

namespace TrainTracks.Engine.Infrastructure;

public sealed class PuzzleClient : IDisposable
{
    private const string BaseUri = "https://puzzlemadness.co.uk/";

    private readonly HttpClientHandler _handler;
    
    private readonly HttpClient _client;

    private readonly int _userId;

    private int _latestYear = 2026;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PuzzleClient()
    {
        var cookieContainer = new CookieContainer();
        
        _handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
        };

        var lines = File.ReadAllLines("cookies.txt");

        foreach (var line in lines)
        {
            var parts = line.Split('=');

            if (parts[0].Equals("userid", StringComparison.InvariantCultureIgnoreCase))
            {
                _userId = int.Parse(parts[1]);
            }

            cookieContainer.Add(new Uri(BaseUri), new Cookie(parts[0], parts[1]));
        }
        
        _client = new HttpClient(_handler)
        {
            BaseAddress = new Uri(BaseUri),
            Timeout = TimeSpan.FromSeconds(30)
        };

        _client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0");

        _client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

        _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

        _client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
    }
    
    public (DateOnly Date, Grid Grid, int Variant)? GetNextPuzzle(Difficulty difficulty)
    {
        var nextPuzzleDate = GetOldestIncompletePuzzleDate(difficulty);

        if (nextPuzzleDate == null)
        {
            return null;
        }
        
        Thread.Sleep(TimeSpan.FromMilliseconds(5_000));

        var year = nextPuzzleDate.Value.Year;
        
        var month = nextPuzzleDate.Value.Month;
        
        var day = nextPuzzleDate.Value.Day;
        
        using var response = _client.GetAsync($"traintracks/{difficulty}/{year}/{month}/{day}").Result;
            
        var page = response.Content.ReadAsStringAsync().Result;

        var puzzleJson = page[(page.IndexOf("puzzleData = ", StringComparison.InvariantCultureIgnoreCase) + 13)..];
        
        puzzleJson = puzzleJson[..puzzleJson.IndexOf(";", StringComparison.InvariantCultureIgnoreCase)];
        
        var puzzle = JsonSerializer.Deserialize<Puzzle>(puzzleJson, _jsonSerializerOptions);

        return (nextPuzzleDate.Value, new Grid(puzzle), puzzle.Source.Variant);
    }

    private DateOnly? GetOldestIncompletePuzzleDate(Difficulty difficulty)
    {
        var now = DateTime.Now;

        for (var year = _latestYear; year <= now.Year; year++)
        {
            using var response = _client.GetAsync($"/archive/traintracks/{difficulty.ToString().ToLower()}/{year}").Result;
            
            var page = response.Content.ReadAsStringAsync().Result;

            var dom = new HtmlDocument();
            
            dom.LoadHtml(page);
            
            var puzzles = dom.DocumentNode.SelectNodes("//td[@class='puzzleNotDone'] | //td[@class='puzzleSaveAvailable']");

            if (puzzles != null && puzzles.Count > 0)
            {
                var puzzle = puzzles[0];
                
                var id =puzzle.Attributes["id"].Value;

                var parts = id.Split('-');
                
                _latestYear = year - 1;

                return new DateOnly(int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]));
            }
            
            Thread.Sleep(TimeSpan.FromMilliseconds(3_000));
        }

        return null;   
    }

    public (HttpStatusCode StatusCode, PuzzleSolvedResponse Response) SendResult(DateOnly date, Grid grid, int variant)
    {
        Thread.Sleep(TimeSpan.FromMilliseconds(1_000));
        
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var score = 0;
        
        var builder = new StringBuilder();
        
        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var piece = grid[x, y];

                if (piece is not Piece.Empty and not Piece.Cross)
                {
                    score += 5;
                }
                
                builder.Append((int) piece);
            }
        }

        score -= grid.FixedPieceCount * 5;

        var payload = new PuzzleSolution
        {
            Type = 33,
            Variant = variant,
            Year = date.Year,
            Month = date.Month,
            Day = date.Day,
            Score = score,
            Solution = builder.ToString(),
            UserId = _userId,
            Status = "PENDING",
            CreatedAt = timestamp
        };

        var json = JsonSerializer.Serialize(payload, _jsonSerializerOptions);

        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var responseObject = _client.PostAsync("user/puzzlecomplete", content).Result;

        var response = JsonSerializer.Deserialize<PuzzleSolvedResponse>(responseObject.Content.ReadAsStringAsync().Result, _jsonSerializerOptions);

        return (responseObject.StatusCode, response);
    }

    public void Dispose()
    {
        _handler?.Dispose();
        
        _client?.Dispose();
    }
}