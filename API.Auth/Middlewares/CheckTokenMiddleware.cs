using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace API.Auth.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckTokenMiddleware>();
        }
    }
}
