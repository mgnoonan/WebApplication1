﻿using System;
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
        public string IpAddressMasked { 
            get
            {
                if (string.IsNullOrWhitespace(this.IpAddress) || this.IpAddress.Contains(":"))
                {
                    return this.IpAddress;
                }
                else
                {
                    return "xxx.xxx." + string.Join(".", this.IpAddress.Split('.').Skip(2).Take(2).ToArray());
                }
            } 
            }
    }
}