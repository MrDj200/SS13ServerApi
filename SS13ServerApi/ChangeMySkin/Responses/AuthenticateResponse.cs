using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS13ServerApi.ChangeMySkin.Responses
{
    internal class AuthenticateResponse : Response
    {
        /// <summary>
        /// Access Token for this user
        /// </summary>
        [JsonProperty("accessToken")]
        public string AccessToken { get; internal set; }

        /// <summary>
        /// Must be the same as Requester.ClientToken
        /// </summary>
        [JsonProperty("clientToken")]
        public string ClientToken { get; internal set; }
    }
}
