using System;
using System.Collections.Generic;
using System.Text;

namespace Silo
{
    public class DomainGrainState
    {
        public HashSet<string> HackedAccounts { get; set; }
    }
}
