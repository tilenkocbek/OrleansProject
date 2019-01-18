using ClientApi.OrleansClient;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ClientApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new Client();
            await client.Initialize();
            CreateWebHostBuilder(client).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(Client client) =>
            WebHost.CreateDefaultBuilder().UseStartup<Startup>().ConfigureServices(services => services.AddSingleton<IClusterClient>(client.ClusterClient));
    }
}