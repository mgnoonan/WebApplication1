using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Repository repository = new Repository();

            repository.SaveBrowserAgent(HttpContext.Request.UserAgent,
                HttpContext.Request.UserHostAddress,
                HttpContext.Request.UrlReferrer,
                "Desktop");

            return View();
        }

        public ActionResult Mobile()
        {
            Repository repository = new Repository();

            repository.SaveBrowserAgent(HttpContext.Request.UserAgent,
                HttpContext.Request.UserHostAddress,
                HttpContext.Request.UrlReferrer,
                "Mobile");

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