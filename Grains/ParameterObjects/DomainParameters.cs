using System;
using Grains.Implementations;

namespace Grains.ParameterObjects
{
    /// <summary>
    /// Parameter object that are used by the <see cref="DomainGrain"/> grain.
    /// </summary>
    [Serializable]
    public class AddMail
    {
        public string Account;
    }

    [Serializable]
    public class CheckMail
    {
        public string Account;
    }
}