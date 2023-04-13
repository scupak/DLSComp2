using Common;
using ConsoleSearch;
using Microsoft.AspNetCore.Mvc;

namespace SearchAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<SearchResult> Search(string terms, int numberOfResults)
    {

        var wordIds = new List<int>();
        var searchTerms = terms.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var mSearchLogic = new SearchLogic(new Database());
        var result = new SearchResult();
        foreach (var t in searchTerms)
        {
            int id = mSearchLogic.GetIdOf(t);
            if (id != -1)
            {
                wordIds.Add(id);
            }
            else
            {
                result.IgnoredTerms.Add(t);
                Console.WriteLine(t + " will be ignored");
            }
        }

        DateTime start = DateTime.Now;

        var docIds = await mSearchLogic.GetDocuments(wordIds);

        // get details for the first 10             
        var top = new List<int>();
        foreach (var p in docIds.GetRange(0, Math.Min(numberOfResults, docIds.Count)))
        {
            top.Add(p.Key);
        }

        TimeSpan used = DateTime.Now - start;
        result.EllapsedMiliseconds = used.TotalMilliseconds;

        int idx = 0;
        foreach (var doc in await mSearchLogic.GetDocumentDetails(top))
        {
            result.Documents.Add(new Document { Id = idx + 1, Path = doc, NumberOfAppearances = docIds[idx].Value });
            Console.WriteLine("" + (idx + 1) + ": " + doc + " -- contains " + docIds[idx].Value + " search terms");
            idx++;
        }
        Console.WriteLine("Documents: " + docIds.Count + ". Time: " + used.TotalMilliseconds);
        return result;
    }

    [HttpGet]
    [Route("ping")]
    public IActionResult Ping()
    {
        Console.WriteLine("Ping");
        return Ok("ping");
    }
}