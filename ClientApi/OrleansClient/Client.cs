using Config;
using Grains.Implementations;
using Microsoft.Extensions.Logging;
using Orleankka.Client;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace ClientApi.OrleansClient
{
    public sealed class Client
    {
        public IClusterClient ClusterClient { get; set; }

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
                .ConfigureApplicationParts(x => x
                    .AddApplicationPart(typeof(DomainGrain).Assembly).WithCodeGeneration())
                .UseOrleankka()
                .Build();
        }

        public async Task Connect(int retries = 0, TimeSpan? delay = null)
        {
            if (ClusterClient.IsInitialized)
            {
                Console.WriteLine("OrleansClient is already initialized!");
                return;
            }

            if (delay == null)
            {
                delay = TimeSpan.FromSeconds(5);
            }

            while (true)
            {
                try
                {
                    await ClusterClient.Connect();
                    return;
                }
                catch (Exception e)
                {
                    if (retries-- == 0)
                    {
                        Console.WriteLine($"\nException while trying to connect to silo: {e.Message}");
                        Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                        Console.WriteLine("\nPress any key to exit.");
                        Console.ReadKey();
                        throw;
                    }

                    ClusterClient = BuildClient();
                    Console.WriteLine($"\nClient couldn't connect to silo.. retrying in {delay.Value.Seconds} seconds.");
                    await Task.Delay(delay.Value);
                }
            }
        }
    }
}