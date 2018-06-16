using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam70486.Filters
{
    public class SampleAuthorizationFilterAsync : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine("OnAuthorizationAsync");

            return Task.FromResult(0);
        }
    }
}
