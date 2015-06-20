using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
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

            Repository<Agent>.SaveDocument(agent);

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

            Repository<Agent>.SaveDocument(agent);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}