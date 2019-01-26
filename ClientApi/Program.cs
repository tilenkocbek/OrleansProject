using ClientApi.OrleansClient;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleankka.Client;
using System.Threading.Tasks;

namespace ClientApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new Client();
            await client.Connect(3);
            CreateWebHostBuilder(client).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(Client client) =>
            WebHost.CreateDefaultBuilder().UseStartup<Startup>().ConfigureServices(services => services.AddSingleton<IClientActorSystem>(client.ClusterClient.ActorSystem()));
    }
}