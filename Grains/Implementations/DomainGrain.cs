using Grains.Contracts;
using Orleans;
using Orleans.Providers;
using Silo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grains.Implementations
{
    [StorageProvider(ProviderName = "BlobStorage")]
    internal class DomainGrain : Grain<DomainGrainState>, IDomainGrain
    {
        private static readonly int DatabaseWritePeriodInMinutes = 5;
        private string domainKey;
        private readonly Func<object, Task> writeToDbCallback;

        public DomainGrain()
        {
            writeToDbCallback = WriteToDb;
        }

        public override Task OnActivateAsync()
        {
            if (State.HackedAccounts == null)
                State.HackedAccounts = new HashSet<string>();
            domainKey = this.GetPrimaryKeyString();
            Console.WriteLine($"Grain for domain {domainKey} activated!");
            this.RegisterTimer(writeToDbCallback, domainKey, TimeSpan.FromMinutes(DatabaseWritePeriodInMinutes), TimeSpan.FromMinutes(DatabaseWritePeriodInMinutes));
            return base.OnActivateAsync();
        }

        public async Task<bool> AddHackedMailAccount(string mailAccount)
        {
            HashSet<String> set = State.HackedAccounts;
            return await Task.FromResult(set.Add(mailAccount));
        }

        public async Task<bool> CheckIfMailWasHacked(string mailAccount)
        {
            return await Task.FromResult(State.HackedAccounts.Contains(mailAccount));
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
            base.WriteStateAsync();
            return Task.CompletedTask;
        }
    }
}