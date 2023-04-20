namespace CacheLoadBalancer.Infrastructure;

public interface IServiceGateway<T>
{
    Task<T> Get(string serviceName ,string parameters);
    Task<bool> Ping(string serviceName);

}