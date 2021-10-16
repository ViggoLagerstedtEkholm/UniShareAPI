using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace UniShareAPI.Models.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if(httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "Id").Value;
        }

        public static bool IsAuthenticated(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return false;
}

            return httpContext.User.Identity.IsAuthenticated;
        }

    }
}
