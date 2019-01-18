using Config;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;

namespace ClientApi.OrleansClient
{
    public sealed class Client
    {
        public IClusterClient ClusterClient { get; }

        public Client()
        {
            ClusterClient = BuildClient();
        }

        private IClusterClient BuildClient()
        {
            return new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Configuration.ClusterId;
                    options.ServiceId = Configuration.ServiceId;
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
        }

        public async Task Initialize()
        {
            if (ClusterClient.IsInitialized)
            {
                Console.WriteLine("OrleansClient is already initialized!");
                return;
            }
            try
            {
                await ClusterClient.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to connect to silo: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                throw new Exception("ClusterClient not connected", e);
            }
        }
    }
}