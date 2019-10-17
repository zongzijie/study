using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityServer.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : ControllerBase
    {
        public async Task<JObject> Get()
        {
            var disco = await DiscoveryClient.GetAsync($"{Request.Scheme}://{Request.Host}");
            if(disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            return tokenResponse.Json;
        }
        [Route("Login")]
        public async Task<JObject> Login(string u,string p)
        {
            var disco = await DiscoveryClient.GetAsync($"{Request.Scheme}://{Request.Host}");
            if(disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(u,p);
            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            return tokenResponse.Json;
        }
    }
}