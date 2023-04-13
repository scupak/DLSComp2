using System.Collections.Concurrent;
using LoadBalancer.Models;

namespace LoadBalancer.LoadBalancer;

public class LoadBalancer : ILoadBalancer
{
   
    private readonly List<Service> _services;
    private ILoadBalancerStrategy _currentStrategy;
    private readonly object _servicesLockObject = new object();
    public LoadBalancer(ILoadBalancerStrategy strategy)
    {
        _services = new List<Service>();
        _currentStrategy = strategy;
    }

    public List<string> GetAllServices()
    {
        lock (_servicesLockObject)
        {
            return _services.Select(service => service.HostName).ToList();
        }
    }

    public int AddService(string serviceName)
    {
        lock (_servicesLockObject)
        {
            _services.Add(new Service(serviceName, 0));
            return _services.Count;
        }
    }

    public string RemoveService(string serviceName)
    {
        lock (_servicesLockObject)
        {
            _services.Remove(_services.First(service => service.HostName == serviceName));
            return serviceName;
        }
    }
    
    public string IncrementServiceConnections(string serviceName)
    {
        lock (_servicesLockObject)
        {
            var serviceToSet = _services.Find(s => s.HostName == serviceName);
            if(serviceToSet == null)
            {
                throw new Exception("Service not found");
            }
            _services.Find(s => s.HostName == serviceName)!.NumberOfConnections += 1;
            Console.WriteLine("All services:");
            foreach (Service service in _services)
            {
                Console.WriteLine(service.HostName + " " + service.NumberOfConnections);
            }
            return serviceName;
        }
    
    }
    
    
    public string DecrementServiceConnections(string serviceName)
    {
        lock (_servicesLockObject)
        {
            var serviceToSet = _services.Find(s => s.HostName == serviceName);
            if(serviceToSet == null)
            {
                throw new Exception("Service not found");
            }
            _services.Find(s => s.HostName == serviceName)!.NumberOfConnections -= 1;
            Console.WriteLine("All services:");
            foreach (Service service in _services)
            {
                Console.WriteLine(service.HostName + " " + service.NumberOfConnections);
            }
            return serviceName;
        }
    
    }

    public ILoadBalancerStrategy GetActiveStrategy()
    {
        return _currentStrategy;
    }

    public void SetActiveStrategy(ILoadBalancerStrategy strategy)
    {
        _currentStrategy = strategy;
    }

    public string NextService()
    {
        lock (_servicesLockObject)
        {
            return _currentStrategy.NextService(_services);
        }
    }
}