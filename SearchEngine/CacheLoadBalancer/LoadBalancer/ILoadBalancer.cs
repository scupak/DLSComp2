namespace CacheLoadBalancer.LoadBalancer;

public interface ILoadBalancer
{
    public List<string> GetAllServices();
    public int AddService(string serviceName);
    public string RemoveService(string serviceName);
    public string IncrementServiceConnections(string serviceName);
    public string DecrementServiceConnections(string serviceName);
    public ILoadBalancerStrategy GetActiveStrategy();
    public void SetActiveStrategy(ILoadBalancerStrategy strategy);
    public string NextService();
}