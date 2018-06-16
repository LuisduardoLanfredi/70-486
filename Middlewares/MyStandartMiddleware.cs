
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace exam70486.Middlewares
{
    public class MyStandartMiddleware
    {
        private readonly RequestDelegate _next;

        public MyStandartMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            return _next.Invoke(context);
        }
    }
}
