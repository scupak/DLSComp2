using System.Collections.Concurrent;
using LoadBalancer.Models;

namespace LoadBalancer.LoadBalancer;

public interface ILoadBalancerStrategy
{
    public string NextService(List<Service> services);
}