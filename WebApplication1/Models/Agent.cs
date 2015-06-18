using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Agent
    {
        public string id { get; set; }
        public string BrowserAgent { get; set; }
        public string IpAddress { get; set; }
        public string Referrer { get; set; }
        public string PageType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}