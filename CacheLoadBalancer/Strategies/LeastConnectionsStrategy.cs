using CacheLoadBalancer.LoadBalancer;
using CacheLoadBalancer.Models;

namespace CacheLoadBalancer.Strategies;

public class LeastConnectionsStrategy : ILoadBalancerStrategy
{
    public string NextService(List<Service> services)
    {
        var service = services.OrderBy(s => s.NumberOfConnections).First();
        return service.HostName;
    }
}