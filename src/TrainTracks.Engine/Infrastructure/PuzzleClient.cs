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
        var userId = "XXX";

        var token = "XXX";
        
        var cookieContainer = new CookieContainer();
        
        _handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer
        };

        cookieContainer.Add(new Uri(BaseUri), new Cookie("userid", userId));
        
        cookieContainer.Add(new Uri(BaseUri), new Cookie("token", token));

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
                System.Console.WriteLine(puzzles.Count);
                
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