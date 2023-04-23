namespace CacheLoadBalancer.Infrastructure;

public interface IServiceGateway<T>
{
    Task<T> Get(string serviceName ,string parameters);
    public Task<T[]> GetAll(string serviceName);
    Task<bool> Ping(string serviceName);

    public  Task<bool> Post(string serviceName, string parameters, T result);

}