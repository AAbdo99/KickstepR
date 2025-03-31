using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace CvManagementApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"Request:{context.Request.Method} {context.Request.Path}");
            await _next(context);
            Console.WriteLine($"Response: {context.Response.StatusCode}");
        }
    }

}