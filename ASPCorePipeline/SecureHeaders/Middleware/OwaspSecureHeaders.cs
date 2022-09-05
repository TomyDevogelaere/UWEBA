using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SecureHeaders.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class OwaspSecureHeaders
    {
        private readonly RequestDelegate _next;

        public OwaspSecureHeaders(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-Xss-Protection", "1");
            httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class OwaspSecureHeadersExtensions
    {
        public static IApplicationBuilder UseOwaspSecureHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OwaspSecureHeaders>();
        }
    }
}
