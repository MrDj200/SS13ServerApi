using SS13ServerApi.ChangeMySkin.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS13ServerApi.ChangeMySkin.Endpoints
{
    public abstract class EndpointBase<T>
    {
        /// <summary>
        /// Full address of the Endpoint
        /// </summary>
        public Uri Address { get; set; }

        internal bool IsBearer { get; set; } = false;

        internal string BearerToken { get; set; }

        public string PostContent { get; set; }

        public Response Response { get; set; }

        /// <summary>
        /// Performs an async request
        /// </summary>
        /// <returns>
        /// Task Object
        /// </returns>
        internal abstract Task<T> PerformRequestAsync();
    }
}
