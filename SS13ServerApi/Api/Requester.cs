using Newtonsoft.Json.Linq;
using SS13ServerApi.ChangeMySkin.Endpoints;
using SS13ServerApi.ChangeMySkin.Responses;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SS13ServerApi.Api
{
    /// <summary>
    /// Requester class to perform the actual requests.
    /// </summary>
    public static class Requester
    {
        /// <summary>
        /// Defines timeout for http requests.
        /// </summary>
        public static TimeSpan Timeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Defines encoding for reading responses and writing requests.
        /// </summary>
        public static Encoding Encoding = Encoding.Default;

        /// <summary>
        /// The Http client
        /// </summary>
        public static HttpClient Client
        {
            get
            {
                if (Client == null)
                {
                    Client = new HttpClient() { Timeout = Requester.Timeout };
                }
                return Client;
            }
            private set
            {
                Client = value;
            }
        }

        /// <summary>
        /// Sends a GET request to the given Endpoint
        /// </summary>
        /// <typeparam name="T">Type of the return Response</typeparam>
        /// <param name="endpoint">The endpoint where to send the request to</param>
        /// <returns></returns>
        internal async static Task<Response> Get<T>(EndpointBase<T> endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("Endpoint", "Endpoint should not be null");
            }

            HttpResponseMessage httpResponse = null;
            Error error = null;
            string rawMessage = null;
            try
            {
                // TODO: Add auth shit with bearer token

                httpResponse = await Requester.Client.GetAsync(endpoint.Address);
                rawMessage = await httpResponse.Content.ReadAsStringAsync();
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                error = new Error(e);
            }
            return new Response()
            {
                Code = httpResponse.StatusCode,
                RawMessage = rawMessage,
                Error = error
            };
        }

        internal async static Task<Response> Post<T>(EndpointBase<T> endpoint, StringContent contents)
        {                        
            CheckParams(endpoint, contents);

            HttpResponseMessage httpResponse = null;
            Error error = null;
            string rawMessage = null;
            
            try
            {
                Requester.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", endpoint.BearerToken);
                Requester.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                Requester.Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ChangeMySkin", "0.1"));

                httpResponse = await Requester.Client.PostAsync(endpoint.Address, contents);
                rawMessage = await httpResponse.Content.ReadAsStringAsync();
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                error = new Error(e);
            }

            return new Response()
            {
                Code = httpResponse.StatusCode,
                RawMessage = rawMessage,
                Error = error
            };
        }

        internal async static Task<Response> Put<T>(EndpointBase<T> endpoint, MultipartContent contents) // use MultipartFormDataContent for skin shit
        {
            CheckParams(endpoint, contents);

            HttpResponseMessage httpResponse = null;
            Error error = null;
            string rawMessage = null;

            try
            {
                Requester.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", endpoint.BearerToken);
                Requester.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                Requester.Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ChangeMySkin", "0.1"));

                httpResponse = await Requester.Client.PutAsync(endpoint.Address, contents);
                rawMessage = await httpResponse.Content.ReadAsStringAsync();
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized ||
                    httpResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    JObject err = JObject.Parse(rawMessage);
                    error = new Error()
                    {
                        ErrorMessage = err["errorMessage"].ToObject<string>(),
                        ErrorTag = err["error"].ToObject<string>(),
                        Exception = e
                    };
                }
                else
                {
                    error = new Error(e);
                }
            }

            return new Response()
            {
                Code = httpResponse.StatusCode,
                RawMessage = rawMessage,
                Error = error
            };
        }

        private static void CheckParams<T>(EndpointBase<T> endpoint, HttpContent contents, bool checkForBearer = false)
        {
            if (checkForBearer && (!endpoint.IsBearer || endpoint.BearerToken == null))
            {
                throw new ArgumentException("endpoint is not a bearer endpoint or has no token", "endpoint");
            }
            if (endpoint == null)
            {
                throw new ArgumentNullException("Endpoint", "Endpoint should not be null");
            }
            if (contents == null)
            {
                throw new ArgumentNullException("contents", "No contents given.");
            }
        }
    }
}
