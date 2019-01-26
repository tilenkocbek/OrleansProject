using ClientApi.Util;
using Grains.Contracts;
using Grains.ParameterObjects;
using Microsoft.AspNetCore.Mvc;
using Orleankka;
using Orleankka.Client;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApi.Controllers
{
    [ApiController]
    public class DomainController : Controller
    {
        private readonly IClientActorSystem system;

        public DomainController(IClientActorSystem system)
        {
            this.system = system;
        }

        [HttpGet("{email}")]
        public async Task<HttpResponseMessage> Get(string email)
        {
            if (!Utils.IsEmailValid(email))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            bool isHacked = await CheckIfAccountIsHacked(email);
            return isHacked ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost("{email}")]
        public async Task<HttpResponseMessage> Post(string email)
        {
            if (!Utils.IsEmailValid(email))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var added = await AddHackedAccount(email);
            return added ? new HttpResponseMessage(HttpStatusCode.Created) : new HttpResponseMessage(HttpStatusCode.Conflict);
        }

        private async Task<bool> CheckIfAccountIsHacked(string email)
        {
            Utils.SplitEmailAddress(email, out var username, out var domain);
            var domainGrain = system.ActorOf<IDomainGrain>(domain);
            return await domainGrain.Ask<bool>(new CheckMail { Account = username });
        }

        private async Task<bool> AddHackedAccount(string email)
        {
            Utils.SplitEmailAddress(email, out var username, out var domain);
            var domainGrain = system.ActorOf<IDomainGrain>(domain);
            return await domainGrain.Ask<bool>(new AddMail { Account = username });
        }
    }
}