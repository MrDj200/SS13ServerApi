using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SS13ServerApi.ChangeMySkin.Responses
{
    public class Response
    {
        /// <summary>
        /// Status code of the response.
        /// </summary>
        public HttpStatusCode Code { get; internal set; }

        /// <summary>
        /// Used to easily check if the request was a success
        /// </summary>
        public bool IsSuccess { get; internal set; }

        internal Response() { }

        internal Response(HttpStatusCode code, bool isSuccess)
        {
            this.Code = code;
            this.IsSuccess = isSuccess;
        }
    }
}
