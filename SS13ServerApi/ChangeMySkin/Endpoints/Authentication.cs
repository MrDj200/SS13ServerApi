using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SS13ServerApi.Api;
using SS13ServerApi.ChangeMySkin.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SS13ServerApi.ChangeMySkin.Endpoints
{
    internal class Authentication : EndpointBase<AuthenticateResponse>
    {
        public Dictionary<string, string> Credentials { get; set; } = new Dictionary<string, string>();

        public Authentication(string username, string password)
        {
            this.Address = new Uri("https://authserver.mojang.com/authenticate");

            Credentials["username"] = username;
            Credentials["password"] = password;
        }

        internal override async Task<AuthenticateResponse> PerformRequestAsync()
        {
            this.PostContent = JsonConvert.SerializeObject(Credentials);

            var contents = new StringContent(this.PostContent, Requester.Encoding, "application/json");
            this.Response = await Requester.Post(this, contents);
            if (this.Response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<AuthenticateResponse>(this.Response.RawMessage);
            }
            else
            {
                return new AuthenticateResponse(Error.GetError(this.Response));
            }
        }
    }
}
