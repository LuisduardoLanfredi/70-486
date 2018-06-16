using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace exam70486.Middlewares
{
    public class MyMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(!context.Response.Headers.Where(x => x.Key == "BLAH").Any())
                context.Response.Headers.Add("BLAH", "ALOHA");

            return next.Invoke(context);
        }
    }
}
