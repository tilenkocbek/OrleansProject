using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;
using Orleankka.Cluster;
using Orleans;
using static Config.Configuration;

namespace Silo
{
    internal class Silo
    {
        public static async Task Main(string[] args)
        {
            var siloHost = await StartSilo();
            Console.WriteLine("Silo Host started..");
            Console.WriteLine("Press any key to terminate Silo Host...");
            Console.ReadKey();
            await siloHost.StopAsync();
        }

        private static async Task<ISiloHost> StartSilo()
        {
            ISiloHostBuilder siloBuilder = new SiloHostBuilder().UseLocalhostClustering().Configure<ClusterOptions>(
                    options =>
                    {
                        options.ClusterId = ClusterId;
                        options.ServiceId = ServiceId;
                    }).Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(x => x.ConfigureDefaults())
                .UseOrleankka()
                .AddAzureBlobGrainStorage("BlobStorage",
                    options => options.ConnectionString = "UseDevelopmentStorage=true");
            ISiloHost host = siloBuilder.Build();
            await host.StartAsync();
            return host;
        }
    }
}