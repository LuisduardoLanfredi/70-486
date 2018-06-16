using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam70486.Filters
{
    public class SampleResultFilterAsync : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine("OnResultExecutionAsync before next Async");

            await next();

            Console.WriteLine("OnResultExecutionAsync after next Async");
        }
    }
}
