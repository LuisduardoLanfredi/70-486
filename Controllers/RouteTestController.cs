using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using exam70486.Jwt;

namespace exam70486.Controllers
{
    public class RouteTestController : Controller
    {
        public RouteTestController(IOptions<TokenProperties> tokenProperties, ILogger<RouteTestController> log, IFileProvider fileProvider)
        {
            using (log.BeginScope("Message attached to logs created in the using block"))
            {
                log.LogCritical(1000, "Something devastading happened! {what}", "mil grau");
            }


            var root = fileProvider.GetDirectoryContents("");
            var embededFile = fileProvider.GetFileInfo("request.xml");

        }

        [HttpGet]
        public IEnumerable<string> Get([FromServices]IHttpContextAccessor httpContextAccessor, int id = 0)
        {
            IFileProvider provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var aaa = provider.GetDirectoryContents("");
            IFileInfo fileInfo = provider.GetFileInfo("appsettings.json");

            //SessionState
            //Persists data accross requests
            //HashTable on the server
            //Backed by cache
            //Cookie with an specif session id for the user sent to the server with each request
            //default 20 minutes
            var sessionValue = HttpContext.Session.GetString("sessionTest");
            if(sessionValue == null)
                HttpContext.Session.SetString("sessionTest", "randon value");

            //TempData can be used to store data between two consecutive requests. TempData values will be retained during redirection
            //TempData internaly use Session to store the data. So think of it as a short lived session.
            //Call TempData.Keep() to keep all the values of TempData in a third request.
            //Non sensitive data
            var tempData = TempData["tempData"];
            if (tempData == null)
                TempData["tempData"] = "temp data...";


            //Used to pass data from the controller to the view
            var viewData = ViewData["tempData"];
            if (viewData == null)
                ViewData["tempData"] = "view data...";

            //Used to pass data from the controller to the view, dymanic though
            var viewBag = ViewBag["viewBag"];
            if (viewBag == null)
                ViewData["viewBag"] = "view bag...";

            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public string GetHello([FromServices]IStringLocalizer<RouteTestController> localizer)
        {
            return localizer.WithCulture(new CultureInfo("fi-FI"))["hello"];

            //return localizer["hello"];
        }
    }
}