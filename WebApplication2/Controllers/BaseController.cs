using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace WebApplication2.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ((Controller)(filterContext.Controller)).ViewBag.CurrentAssemblyVersion = $"v. {this.CurrentAssemblyVersion}";
        }

        /// <summary>
        /// Returns the current assembly version
        /// </summary>
        protected virtual string CurrentAssemblyVersion
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        protected virtual bool IsMobile()
        {
            string agent = HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();

            return
                Regex.IsMatch(agent, @"Android(?!.*\sMobile[\s;])", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(agent, @"Android(?=.*\sNexus (7|9|10)\s)", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(agent, @"Android|AU-MIC|AUDIOVOX-|Alcatel|BlackBerry|BB10|Blazer|Danger hiptop|DoCoMo/|Ericsson|Googlebot-Mobile|MSN Mobile Proxy|Handheld|iPad|iPod|iPhone|Klondike|LG-|LGE-|LGE|MOT-|NetFront|Nokia|Novarra-Vision|Opera Mini|PalmOS|PalmSource|Panasonic-|SAGEM-|SAMSUNG|Smartphone|Sony|Symbian OS|webOS|Windows CE|Windows Mobile|Windows Phone|nokia|portalmmm|Profile/MIDP-|UP.Link|UP.Browser|XV6875|BlackBerry 8300|Xoom|SCH-I800", RegexOptions.IgnoreCase);
        }

        protected virtual string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = SplitCsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            if (string.IsNullOrWhiteSpace(ip))
                ip = HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        private T GetHeaderValueAs<T>(string headerName)
        {
            if (HttpContext.Request.Headers.TryGetValue(headerName, out StringValues values))
            {
                // writes out as Csv when there are multiple
                string rawValues = values.ToString();

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }

            return default;
        }

        private List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim().Split(":")[0])
                .ToList();
        }
    }
}
