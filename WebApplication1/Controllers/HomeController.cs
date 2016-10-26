using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            bool isMobile = HttpContext.Request.Headers.AllKeys.Contains("X-Mobile-Device");
            ViewBag.IsMobile = isMobile;

            // Create document with incoming parameter values
            var agent = new Agent
            {
                id = Guid.NewGuid().ToString(),
                BrowserAgent = HttpContext.Request.UserAgent,
                IpAddress = HttpContext.Request.UserHostAddress,
                Referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.ToString(),
                PageType = isMobile ? "Mobile" : "Desktop",
                Timestamp = DateTime.Now
            };

#if !DEBUG
            Repository<Agent>.SaveDocument(agent);
#endif
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your useless application description page. Just for kicks.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your irritating contact page.";

            return View();
        }
    }
}
