﻿namespace WebApplication2.Controllers;

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
