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

        private Task WriteToDb(object o)
        {
            Console.WriteLine("Save to db!");
            return Task.CompletedTask;
        }

        public override Task<object> Receive(object message)
        {
            switch (message)
            {
                case Activate _:
                    {
                        domainKey = this.GetPrimaryKeyString();
                        Console.WriteLine($"Grain for domain {domainKey} activated!");
                        //this.RegisterTimer(writeToDbCallback, domainKey, TimeSpan.FromMinutes(DatabaseWritePeriodInMinutes), TimeSpan.FromMinutes(DatabaseWritePeriodInMinutes));
                        return Task.FromResult<object>(Task.CompletedTask);
                    }
                case Deactivate _:
                    {
                        Console.WriteLine($"Grain for domain {domainKey} will be deactivated.");
                        WriteToDb(null);
                        return Task.FromResult<object>(Task.CompletedTask);
                    }
                case AddMail msg:
                    {
                        return Task.FromResult<object>(AddHackedMailAccount(msg.Account));
                    }
                case CheckMail msg:
                    {
                        return Task.FromResult<object>(CheckIfAccountWasHacked(msg.Account));
                    }
                default:
                    return Task.FromResult<object>(Task.CompletedTask);
            }
        }

        private bool AddHackedMailAccount(string account)
        {
            return hackedAccounts.Add(account);
        }

        private bool CheckIfAccountWasHacked(string account)
        {
            return hackedAccounts.Contains(account);
        }
    }
}