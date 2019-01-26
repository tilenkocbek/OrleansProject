using System;
using System.Threading.Tasks;
using Grains.Contracts;
using Orleankka;

namespace Grains.Implementations
{
    public class TestGrain : ActorGrain, ITestGrain
    {
        public override Task OnActivateAsync()
        {
            Console.WriteLine("Activated!!");
            return Task.CompletedTask;
        }

        public override Task<object> Receive(object message)
        {
            Console.WriteLine("Received something!");
            return Task.FromResult<object>("test");
        }
    }
}