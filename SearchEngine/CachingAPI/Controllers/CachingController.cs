using Microsoft.AspNetCore.Mvc;
using Common;

namespace CachingService.Controllers;

[ApiController]
[Route("[controller]")]
public class CachingController : ControllerBase
{
   

    public CachingController( )
    {
        
        
    }

    [HttpGet]
    public async Task<SearchResult> CheckSearchCache(string terms)
    {
        var res = await CachingService.GetData<SearchResult>(terms);

        if (res == null)
        {
            return null;
        }

        return res;



    }

    [HttpPost]
    public async Task<IActionResult> AddToSearchCache(string terms, [FromBody]SearchResult result)
    {
        bool res = await CachingService.SetData(terms, result);
        if (res)
        {
            return Ok();
        }
        else
        {
            return StatusCode(500, "Couldn't add to cache");
        }
    }
}