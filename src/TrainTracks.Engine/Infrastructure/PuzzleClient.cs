using System.Net;
using HtmlAgilityPack;
using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine.Board;

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
    
    public Grid GetNextNuzzle(Difficulty difficulty)
    {
        var now = DateTime.Now;

        for (var year = 2005; year <= now.Year; year++)
        {
            var response = _client.GetAsync($"/archive/traintracks/{difficulty.ToString().ToLower()}/{year}").Result;
            
            var page = response.Content.ReadAsStringAsync().Result;

            var dom = new HtmlDocument();
            
            dom.LoadHtml(page);
            
            var puzzles = dom.DocumentNode.SelectNodes("//td[@class='puzzleNotDone']");

            if (puzzles != null && puzzles.Count > 0)
            {
                var puzzle = puzzles[0];
                
                System.Console.WriteLine(puzzle.Attributes["id"].Value);
                
                break;
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