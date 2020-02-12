using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Context;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            bool isMobile = base.IsMobile();  // HttpContext.Request.Headers.ContainsKey("X-Mobile-Device");
            ViewBag.IsMobile = isMobile;
            ViewBag.ActualIpAddress = base.GetRequestIP();
            ViewBag.ReportedIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

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
            throw new NotImplementedException("Forget something?");
        }

        public IActionResult Test()
        {
            using (LogContext.PushProperty("TestId", DateTime.Now.Second))
            {
                RunTest();
            }

            return View();
        }

        private static void RunTest()
        {
            Log.Information("Starting test at {ts}", DateTime.Now);

            // TODO: This is for the test, remove before launch
            Log.Information("Executing test method A");
            Thread.Sleep(6000);

            Log.Information("Ending test at {ts}", DateTime.Now);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
