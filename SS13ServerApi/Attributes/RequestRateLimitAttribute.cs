using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SS13ServerApi.Services;
using System;
using System.Net;

namespace SS13ServerApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequestRateLimitAttribute : ActionFilterAttribute
    {
        public int MaxAmount { get; set; }
        public int Seconds { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var service = context.HttpContext.RequestServices.GetService<AccessControlService>();
            IPAddress ipAddress = context.HttpContext.Connection.RemoteIpAddress;

            if (!service.Data.ContainsKey(ipAddress))
            {
                service.Data.Add(ipAddress, new AccessInfo());
                return;
            }
            if ((DateTime.Now - service.Data[ipAddress].CreatedTime).TotalSeconds >= Seconds)
            {
                service.Data.Remove(ipAddress);
                return;
            }
            if (service.Data[ipAddress].Counter < MaxAmount)
            {
                service.Data[ipAddress].Counter++;
                return;
            }

            context.Result = new ContentResult
            {
                Content = $"Requests are limited to {MaxAmount}, every {Seconds} seconds."
            };

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
        }
    }
}
