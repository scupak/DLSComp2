using LoadBalancer.LoadBalancer;
using LoadBalancer.Models;

namespace LoadBalancer.Strategies;

public class LeastConnectionsStrategy : ILoadBalancerStrategy
{
    public string NextService(List<Service> services)
    {
        var service = services.OrderBy(s => s.NumberOfConnections).First();
        return service.HostName;
    }
}