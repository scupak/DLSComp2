using System.Collections.Concurrent;
using CacheLoadBalancer.Models;

namespace CacheLoadBalancer.LoadBalancer;

public interface ILoadBalancerStrategy
{
    public string NextService(List<Service> services);
}