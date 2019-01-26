using Grains.Contracts;
using Grains.ParameterObjects;
using Orleankka;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grains.Implementations
{
    /// <summary>
    /// TODO: Write storage repository that will use Azure blob storage and inject it to grains for persistence.
    /// </summary>
    public class DomainGrain : ActorGrain, IDomainGrain
    {
        private static readonly int DatabaseWritePeriodInMinutes = 5;
        private string domainKey;
        private readonly HashSet<string> hackedAccounts;
        private readonly Func<object, Task> writeToDbCallback;

        public DomainGrain()
        {
            writeToDbCallback = WriteToDb;
            hackedAccounts = new HashSet<string>();
        }

        public override Task OnActivateAsync()
        {
            domainKey = this.GetPrimaryKeyString();
            Console.WriteLine($"Grain for domain {domainKey} activated!");
            //this.RegisterTimer(writeToDbCallback, domainKey, TimeSpan.FromMinutes(DatabaseWritePeriodInMinutes), TimeSpan.FromMinutes(DatabaseWritePeriodInMinutes));
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            Console.WriteLine($"Grain for domain {domainKey} will be deactivated.");
            WriteToDb(null);
            return base.OnDeactivateAsync();
        }

        private Task WriteToDb(object o)
        {
            Console.WriteLine("Save to db!");
            return Task.CompletedTask;
        }

        public override Task<object> Receive(object message)
        {
            return Task.FromResult<object>(Handle((dynamic)message));
        }

        private bool Handle(AddMail msg)
        {
            return hackedAccounts.Add(msg.Account);
        }

        private bool Handle(CheckMail msg)
        {
            return hackedAccounts.Contains(msg.Account);
        }
    }
}