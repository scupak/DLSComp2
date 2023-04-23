using Common;
using ConsoleSearch;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net;

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
        var client = new RestClient("http://cacheloadBalancer");

        var request = new RestRequest("/CacheLoadBalancer/CheckSearchCache");
        request.AddQueryParameter("terms", terms);
        var response = await client.GetAsync(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            result = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchResult>(response.Content);
            return result;
        }

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

        DateTime now = DateTime.Now;
        result.SearchDateTime = now;

        result.SearchTerms = terms;

        int idx = 0;
        foreach (var doc in await mSearchLogic.GetDocumentDetails(top))
        {
            result.Documents.Add(new Document { Id = idx + 1, Path = doc, NumberOfAppearances = docIds[idx].Value });
            Console.WriteLine("" + (idx + 1) + ": " + doc + " -- contains " + docIds[idx].Value + " search terms");
            idx++;
        }
        Console.WriteLine("Documents: " + docIds.Count + ". Time: " + used.TotalMilliseconds);
        try
        {
            var postRequest = new RestRequest("/CacheLoadBalancer/AddToCache");
            postRequest.AddJsonBody(result);
            var postResponse = await client.PostAsync(postRequest);

            if (postResponse.IsSuccessful)
            {
                Console.WriteLine($"Cache updated with: {result}");
            }
        }
        catch (Exception e ) 
        {
            Console.WriteLine(e);
        }
            
        
        

        

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