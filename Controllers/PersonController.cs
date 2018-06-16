using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using exam70486.Filters;

namespace exam70486.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly IDependencyInjectionTester _dependencyInjectionTester;
        public PersonController(IDependencyInjectionTester dependencyInjectionTester)
        {
            _dependencyInjectionTester = dependencyInjectionTester;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Person person)
        {
            return Ok(ModelState.IsValid);
        }

        //Location = Client / Cache-Control = private
        //Location = Any    / Cache-Control = public
        //Location = None   / Cache-Control = no-cache
        //[ResponseCache(Duration = 20, Location = ResponseCacheLocation.Client, VaryByHeader = "Content-Type", VaryByQueryKeys)]
        [Authorize(Policy = "Administrator")]
        [HttpGet("getadmin")]
        //[ResponseCache(CacheProfileName = "mycache")]
        public IActionResult GetAdministrator()
        {
            return Ok($"Administrator {DateTime.Now}");
        }

        [Authorize(Policy = "Member")]
        [HttpGet("getmember")]
        [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Client, VaryByHeader = "Content-Type")]
        public IActionResult GetMember()
        {
            return Ok($"Member {DateTime.Now}");
        }

        [HttpGet("get")]
        [SampleActionFilter]
        [ResponseCache(CacheProfileName = "mycache")]
        public IActionResult Get([FromServices]IDependencyInjectionTester dependencyInjectionTester, [FromServices]IHttpContextAccessor ctx)
        {
            IDependencyInjectionTester test = (IDependencyInjectionTester)ctx.HttpContext.RequestServices.GetService(typeof(IDependencyInjectionTester));

            var aaa = test.Testing();


            return Ok(dependencyInjectionTester.Testing());
        }
    }
}
