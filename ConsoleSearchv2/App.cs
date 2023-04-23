using Common;
using Newtonsoft.Json;
using RestSharp;

namespace ConsoleSearchv2;

public class App
{
    public void Run()
    {
        try
        {
            RestClient c = new RestClient("http://loadbalancer/LoadBalancer");
            Console.WriteLine("Console Search");

            while (true)
            {
                Console.WriteLine("Press s to start");
                if(Console.ReadLine() == "s") break;
            }

            while (true)
            {
                Console.WriteLine("enter search terms - q for quit");
                string input = Console.ReadLine() ?? string.Empty;
                if (input.Equals("q")) break;

                string resultString = string.Empty;
                
                var request = new RestRequest("/Search?terms=" + input + "&numberOfResults=" + 10);
                var response = c.GetAsync<SearchResult>(request);
                response.Wait();
                SearchResult result = response.Result;

                foreach (var ignored in result.IgnoredTerms)
                {
                    Console.WriteLine(ignored + " will be ignored");
                }

                foreach (var doc in result.Documents)
                {
                    Console.WriteLine(doc.Id + ": " + doc.Path + " -- contains " + doc.NumberOfAppearances +
                                      " search terms");
                }

                Console.WriteLine("Documents: " + result.Documents.Count + ". Time: " + result.EllapsedMiliseconds +
                                  " ms");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            
        }
    }
}