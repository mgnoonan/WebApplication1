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
            // Create document with incoming parameter values
            var agent = new Agent
            {
                id = Guid.NewGuid().ToString(),
                BrowserAgent = HttpContext.Request.UserAgent,
                IpAddress = HttpContext.Request.UserHostAddress,
                Referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.ToString(),
                PageType = "Desktop",
                Timestamp = DateTime.Now
            };

#if !DEBUG
            Repository<Agent>.SaveDocument(agent);
#endif
            return View();
        }

        public ActionResult Mobile()
        {
            // Create document with incoming parameter values
            var agent = new Agent
            {
                id = Guid.NewGuid().ToString(),
                BrowserAgent = HttpContext.Request.UserAgent,
                IpAddress = HttpContext.Request.UserHostAddress,
                Referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.ToString(),
                PageType = "Mobile",
                Timestamp = DateTime.Now
            };

#if !DEBUG
            Repository<Agent>.SaveDocument(agent);
#endif
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your useless application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your irritating contact page.";

            return View();
        }
    }
}