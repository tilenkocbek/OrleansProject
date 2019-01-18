using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace Grains.Contracts
{
    /// <summary>
    /// Interface that will be used by grains.
    /// </summary>
    public interface IDomainGrain : IGrainWithStringKey
    {
        /// <summary>
        /// Add specified e-mail account to list of mail accounts that have been breached/hacked.
        /// </summary>
        /// <param name="mailAccount"></param>
        /// <returns>true if e-mail account has been successfully added to list, false if e-mail account already exists in the list of breached/hacked e-mails.</returns>
        Task<bool> AddHackedMailAccount(string mailAccount);

        /// <summary>
        /// Check if specified e-mail accounts has been hacked.
        /// </summary>
        /// <param name="mailAccount"></param>
        /// <returns>true if e-mail account has been hacked, false otherwise.</returns>
        Task<bool> CheckIfMailWasHacked(string mailAccount);
    }
}
