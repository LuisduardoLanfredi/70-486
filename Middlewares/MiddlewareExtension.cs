using Microsoft.AspNetCore.Builder;

namespace exam70486.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }

        public static IApplicationBuilder UseMyStandartMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyStandartMiddleware>();
        }
    }
}
