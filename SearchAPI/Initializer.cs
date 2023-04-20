using RestSharp;
using System.Net;
using System.Security.AccessControl;

namespace SearchAPI
{
    public class Initializer
    {
        private RestClient client;
        private string hostname;
        private int connectionAttempts;

        public Initializer()
        {
            client = new RestClient("http://loadbalancer");
            hostname = Environment.MachineName;
            connectionAttempts = 1;
        }

        public async void registerWithLoadBalancer()
        {
            try
            {
                Console.WriteLine($"service with host name: {hostname}, attempting to register with load balancer");
                var request = new RestRequest("/loadBalancer/AddService");
                request.AddQueryParameter("serviceName", hostname);
                var response = await client.PostAsync(request);
                response.ThrowIfError();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine($"service with host name: {hostname},has succesfully registered");
                }
                else
                {
                    Console.WriteLine($"Failed to register with load balancer");
                    connectionAttempts ++;

                    if (connectionAttempts > 10)
                    {
                        Console.WriteLine($"Tried connecting service {connectionAttempts} times and failed");
                        Console.WriteLine("Wont be reattempting, something is wrong with the connection");
                        Console.WriteLine("Contact the sysAdmin");
                        return;
                    }

                    Console.WriteLine($"Trying again for the: {connectionAttempts} time");
                    registerWithLoadBalancer();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to register with load balancer got error message: {e.Message}");
                connectionAttempts++;

                if (connectionAttempts > 10)
                {
                    Console.WriteLine($"Tried connecting service {connectionAttempts} times and failed");
                    Console.WriteLine("Wont be reattempting, something is wrong with the connection");
                    Console.WriteLine("Contact the sysAdmin");
                    return;
                }

                Console.WriteLine($"Trying again for the: {connectionAttempts} time");
                registerWithLoadBalancer();
            }
        }
    }
}
