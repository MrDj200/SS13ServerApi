using System;
using System.Net;

namespace SS13ServerApi.ChangeMySkin.Responses
{
    public class Response
    {
        private Response response;

        /// <summary>
        /// Status code of the response.
        /// </summary>
        public HttpStatusCode Code { get; internal set; }

        /// <summary>
        /// Used to easily check if the request was a success
        /// </summary>
        public bool IsSuccess { get => CheckSuccess(); internal set { } }

        /// <summary>
        /// Contains an error if the request failed.
        /// </summary>
        public Error Error { get; internal set; }

        public string RawMessage { get; internal set; }

        internal Response() { }

        internal Response(HttpStatusCode code, bool isSuccess)
        {
            this.Code = code;
            this.IsSuccess = isSuccess;
        }

        public Response(Response response)
        {
            this.response = response;
            this.Code = response.Code;
            this.IsSuccess = response.IsSuccess;
            this.RawMessage = response.RawMessage;
            this.Error = response.Error;
        }

        private bool CheckSuccess() => (
                    Code == HttpStatusCode.Accepted ||
                    Code == HttpStatusCode.Continue ||
                    Code == HttpStatusCode.Created ||
                    Code == HttpStatusCode.Found ||
                    Code == HttpStatusCode.OK ||
                    Code == HttpStatusCode.PartialContent ||
                    Code == HttpStatusCode.NoContent
                ) && Error == null;
    }
    /// <summary>
    /// Default error class.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Tag of the given error.
        /// </summary>
        public string ErrorTag { get; internal set; }

        /// <summary>
        /// Details of the error.
        /// </summary>
        public string ErrorMessage { get; internal set; }

        /// <summary>
        /// Exception if code-side error.
        /// </summary>
        public Exception Exception { get; internal set; }
        internal Error()
        {

        }
        internal Error(Exception exception)
        {
            this.ErrorMessage = exception.Message;
            this.ErrorTag = exception.GetBaseException().Message;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets an error thanks to a response.
        /// </summary>
        public static Response GetError(Response response)
        {
            // This has to be fill
            switch (response.Code)
            {
                case HttpStatusCode.NoContent:
                    {
                        return new Response(response)
                        {
                            IsSuccess = false,
                            Error = new Error()
                            {
                                ErrorMessage = "Response has no content",
                                ErrorTag = "NoContent"
                            }
                        };
                    }

                case HttpStatusCode.UnsupportedMediaType:
                    {
                        return new Response(response)
                        {
                            IsSuccess = false,
                            Error = new Error()
                            {
                                ErrorMessage = "Post contents must not be well formatted",
                                ErrorTag = "UnsupportedMediaType"
                            }
                        };
                    }

                default:
                    {
                        return new Response(response)
                        {
                            IsSuccess = false,
                            Error = new Error()
                            {
                                ErrorMessage = response.Code.ToString(),
                                ErrorTag = response.Code.ToString()
                            }
                        };
                    }
            }
        }
    }
}
