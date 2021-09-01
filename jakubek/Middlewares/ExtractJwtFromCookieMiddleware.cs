using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Middlewares
{
    public class ExtractJwtFromCookieMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Cookies["jwt"];
            if (token != null)
                context.Request.Headers.Append("Authorization", "Bearer " + token);

            await next.Invoke(context);
        }
    }
}
