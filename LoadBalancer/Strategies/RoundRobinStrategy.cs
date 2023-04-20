using LoadBalancer.LoadBalancer;
using LoadBalancer.Models;

namespace LoadBalancer.Strategies;

public class RoundRobinStrategy : ILoadBalancerStrategy
{
    private int _currentServiceIndex = 0;
    public string NextService(List<Service> services)
    {
        
        var count = services.Count;
        if (count == 0)
        {
            throw new Exception("No services available");
        }
        
        var service = services.ElementAt(_currentServiceIndex);
        _currentServiceIndex = (_currentServiceIndex + 1) % count;
        return service.HostName;




    }
}