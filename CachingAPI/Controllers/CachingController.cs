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
        var res = await CachingService.GetData<SearchResult>("searchResultCache", terms);

        if (res == null)
        {
            return null;
        }

        return res;



    }
    [HttpGet("GetAll")]
    public async Task<SearchResult[]> GetWholeHash()
    {
        var res = await CachingService.GetWholeHash<SearchResult>("searchResultCache");

        if (res == null)
        {
            return null;
        }

        return res;
    }

        [HttpPost]
    public async Task<IActionResult> AddToSearchCache(string terms, [FromBody]SearchResult result)
    {
         await CachingService.SetData("searchResultCache",terms , result);
     
            return Ok();
        
       
    }
}