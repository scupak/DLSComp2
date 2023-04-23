using System.Net;
using Common;
using RestSharp;

namespace CacheLoadBalancer.Infrastructure;

public class CacheServiceGateway : IServiceGateway<SearchResult>
{
    public RestClient client;

    public CacheServiceGateway()
    {
        client = new RestClient();
    }

    public async Task<SearchResult> Get(string serviceName, string parameters)
    {
        //this.baseUrl + '/LoadBalancer/Search?terms=' + searchTerms + "&numberOfResults=" + 10
        var request = new RestRequest($"http://{serviceName}/Search?{parameters}");
        var response = await client.GetAsync(request);
        response.ThrowIfError();

        if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SearchResult>(response.Content) ??
                   throw new ArgumentException();

        throw new BadHttpRequestException("Connection Failed");
    }


    public async Task<bool> Ping(string serviceName)
    {
        try
        {
            var request = new RestRequest($"http://{serviceName}/Search/ping");
            var response = await client.GetAsync(request);
            response.ThrowIfError();

            if (response.StatusCode == HttpStatusCode.OK) return true;

            Console.WriteLine("Connection Failed could not connect to: " + serviceName);
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection Failed could not connect to: " + serviceName);
            return false;
        }
    }
}