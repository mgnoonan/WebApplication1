using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Serilog;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            bool isMobile = base.IsMobile();  // HttpContext.Request.Headers.ContainsKey("X-Mobile-Device");
            ViewBag.IsMobile = isMobile;

            // Create document with incoming parameter values
            var agent = new Agent
            {
                id = Guid.NewGuid().ToString(),
                BrowserAgent = HttpContext.Request.Headers[HeaderNames.UserAgent].ToString() ?? "",
                IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                Referrer = HttpContext.Request.Headers[HeaderNames.Referer].ToString() ?? "",
                PageType = isMobile ? "Mobile" : "Desktop",
                Timestamp = DateTime.Now
            };

            Log.Information("Detected user info: {@Agent}", agent);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
