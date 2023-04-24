using CacheLoadBalancer.Infrastructure;
using CacheLoadBalancer.LoadBalancer;
using CacheLoadBalancer.Strategies;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace CacheLoadBalancer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheLoadBalancerController : ControllerBase
    {

        private readonly ILoadBalancer _loadBalancer;
        private readonly IServiceGateway<SearchResult> _serviceGateway;

        public CacheLoadBalancerController(ILoadBalancer loadBalancer, IServiceGateway<SearchResult> serviceGateway)
        {
            _loadBalancer = loadBalancer;
            _serviceGateway = serviceGateway;
        }

        [HttpGet]
        [Route("CheckSearchCache")]
        public async Task<IActionResult> CheckSearchCache(string terms, int numberOfResults)
        {
            Console.WriteLine("CheckSearchCache has been called");
            try
            {
                var pingResult = false;
                var serviceName = "";

                while (!pingResult)
                {
                    serviceName = _loadBalancer.NextService();
                    pingResult = await _serviceGateway.Ping(serviceName);
                    if (!pingResult)
                    {
                        _loadBalancer.RemoveService(serviceName);
                        Console.WriteLine("removed service: " + serviceName);
                    }

                    if (_loadBalancer.GetAllServices().Count <= 0)
                    {
                        return StatusCode(503, "No Services Available");
                    }
                }

                _loadBalancer.IncrementServiceConnections(serviceName);
                Console.WriteLine("successfully pinged: " + serviceName + " Getting result...");
                var result = await _serviceGateway.Get(serviceName, $"terms={terms}&numberOfResults={numberOfResults}");
                
                if (result == null)
                {
                    var message = "Not in cache";
                    Console.WriteLine(message);
                    return StatusCode(204, message);

                }
                
                Console.WriteLine("successfully got result from: " + serviceName);
                _loadBalancer.DecrementServiceConnections(serviceName);
                return new ObjectResult(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Couldn't Connect to service");
                Console.WriteLine(ex);

                return StatusCode(503, ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Search result was null");
                Console.WriteLine(ex);
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllFromCache")]
        public async Task<IActionResult> GetAllFromCache()
        {
            Console.WriteLine("GetAllFromCache has been called");
            try
            {
                var pingResult = false;
                var serviceName = "";

                while (!pingResult)
                {
                    serviceName = _loadBalancer.NextService();
                    pingResult = await _serviceGateway.Ping(serviceName);
                    if (!pingResult)
                    {
                        _loadBalancer.RemoveService(serviceName);
                        Console.WriteLine("removed service: " + serviceName);
                    }

                    if (_loadBalancer.GetAllServices().Count <= 0)
                    {
                        return StatusCode(503, "No Services Available");
                    }
                }

                _loadBalancer.IncrementServiceConnections(serviceName);
                Console.WriteLine("successfully pinged: " + serviceName + " Getting result...");
                var result = await _serviceGateway.GetAll(serviceName);

                

                if (result == null)
                {
                    var message = "Not in cache";
                    Console.WriteLine(message);
                    return StatusCode(204, message);

                }

              var sortedResult =  result.OrderByDescending(s =>  s.SearchDateTime.Date )
                    .ThenByDescending(s => s.SearchDateTime.TimeOfDay);

                Console.WriteLine("successfully got result from: " + serviceName);
                _loadBalancer.DecrementServiceConnections(serviceName);
                return new ObjectResult(sortedResult);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Couldn't Connect to service");
                Console.WriteLine(ex);

                return StatusCode(503, ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Search result was null");
                Console.WriteLine(ex);
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("AddToCache")]
        public async Task<IActionResult> AddToCache([FromBody] SearchResult searchResult)
        {
            Console.WriteLine("AddToCache has been called");
            try
            {
                var pingResult = false;
                var serviceName = "";

                while (!pingResult)
                {
                    serviceName = _loadBalancer.NextService();
                    pingResult = await _serviceGateway.Ping(serviceName);
                    if (!pingResult)
                    {
                        _loadBalancer.RemoveService(serviceName);
                        Console.WriteLine("removed service: " + serviceName);
                    }

                    if (_loadBalancer.GetAllServices().Count <= 0)
                    {
                        return StatusCode(503, "No Services Available");
                    }
                }

                _loadBalancer.IncrementServiceConnections(serviceName);
                Console.WriteLine("successfully pinged: " + serviceName + " Getting result...");
                var result = await _serviceGateway.Post(serviceName, $"terms={searchResult.SearchTerms}", searchResult);


                Console.WriteLine("successfully got result from: " + serviceName);
                _loadBalancer.DecrementServiceConnections(serviceName);
                return new ObjectResult(result);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Couldn't Connect to service");
                Console.WriteLine(ex);

                return StatusCode(503, ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Search result was null");
                Console.WriteLine(ex);
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddService")]
        public async Task<IActionResult> AddService(string serviceName)
        {
            Console.WriteLine("called loadbalancer add service");
            // Here we want to check if the given service name is correct
            // So we attempt to ping to check the connection

            try
            {
                var response = await _serviceGateway.Ping(serviceName);

                if (!response) throw new BadHttpRequestException("Connection Failed");

                _loadBalancer.AddService(serviceName);

                Console.WriteLine($"Succefully connected and added the service with name: {serviceName} to the list");
                Console.WriteLine("Current list of services:");

                foreach (var service in _loadBalancer.GetAllServices())
                {
                    Console.WriteLine(service);
                }

                return Ok("Connection succesfully, you have been added to the list of services");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Couldn't Connect to service");
                Console.WriteLine(ex);
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("ping")]
        public IActionResult Ping()
        {
            Console.WriteLine("Ping");
            return Ok("ping");
        }

        //Endpoint for setting loadbalancer strategy
        [HttpPost]
        [Route("SetStrategy")]
        public IActionResult SetStrategy(string strategy)
        {
            Console.WriteLine("SetStrategy");
            try
            {
                // set strategy based on the received string 
                switch (strategy)
                {
                    case "RoundRobin":
                        _loadBalancer.SetActiveStrategy(new RoundRobinStrategy());
                        break;
                    case "LeastConnections":
                        _loadBalancer.SetActiveStrategy(new LeastConnectionsStrategy());
                        break;
                    default:
                        return StatusCode(400, "Invalid strategy");
                }

                return Ok("Strategy set to: " + strategy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something Went Wrong");
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

    }
}