using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class StatusHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationBuilder _app;

        public StatusHandlerMiddleWare(RequestDelegate next, IApplicationBuilder app)
        {
            _next = next;
            _app = app;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            _app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception)
                {
                    var statusCode = context.Response.StatusCode;

                    if (statusCode == 500)
                    {
                        context.Response.StatusCode = 400;
                    }
                }
            });

             await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class StatusHandlerMiddleWareExtensions
    {
        public static IApplicationBuilder UseStatusHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatusHandlerMiddleWare>();
        }
    }
}
