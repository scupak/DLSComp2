namespace LoadBalancer.Models;

public class Service
{
    public Service(string hostName, int numberOfConnections)
    {
        this.HostName = hostName;
        this.NumberOfConnections = numberOfConnections;
    }

    public string HostName { get; set; }
    public int NumberOfConnections { get; set; }
    
}