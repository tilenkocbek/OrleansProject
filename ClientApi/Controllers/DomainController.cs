using ClientApi.Util;
using Grains.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApi.Controllers
{
    [ApiController]
    public class DomainController : Controller
    {
        private IClusterClient client;

        public DomainController(IClusterClient client)
        {
            this.client = client;
        }

        [HttpGet("{email}")]
        public async Task<HttpResponseMessage> Get(string email)
        {
            if (!Utils.IsEmailValid(email))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var isHacked = await CheckIfAccountIsHacked(email);
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
            var domainGrain = client.GetGrain<IDomainGrain>(domain);
            return await domainGrain.CheckIfMailWasHacked(username);
        }

        private async Task<bool> AddHackedAccount(string email)
        {
            Utils.SplitEmailAddress(email, out var username, out var domain);
            var domainGrain = client.GetGrain<IDomainGrain>(domain);
            return await domainGrain.AddHackedMailAccount(username);
        }
    }
}