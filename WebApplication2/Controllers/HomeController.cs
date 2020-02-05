﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
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
            bool isMobile = HttpContext.Request.Headers.ContainsKey("X-Mobile-Device");
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