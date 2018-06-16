using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam70486.Filters
{
    public class SampleResourceFilterAsync : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine("OnResourceExecutionAsync before next Async");

            await next();

            Console.WriteLine("OnResourceExecutionAsync after next Async");
        }
    }
}
