using HtmlAgilityPack;
using TrainTracks.Console.Infrastructure;
using TrainTracks.Engine.Board;

namespace TrainTracks.Engine.Infrastructure;

public class PuzzleClient
{
    private const string BaseUri = "https://puzzlemadness.co.uk/";

    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri(BaseUri)
    };
    
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
                
                
                break;
            }
        }

        return null;
    }
}