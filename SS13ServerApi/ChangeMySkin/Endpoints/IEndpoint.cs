using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS13ServerApi.ChangeMySkin.Endpoints
{
    interface IEndpoint<T>
    {
        /// <summary>
        /// Full address of the Endpoint
        /// </summary>
        public Uri Address { get; set; }
        /// <summary>
        /// Arguments to be sent.
        /// </summary>
        public List<string> Arguments
        {
            get
            {
                if (Arguments == null)
                    Arguments = new List<string>() { };
                return Arguments;
            }
            set { Arguments = value; }
        }

        /// <summary>
        /// Performs an async request
        /// </summary>
        /// <returns>
        /// Task Object
        /// </returns>
        public abstract Task<T> PerformRequestAsync();
    }
}
