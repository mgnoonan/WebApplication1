using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    }
}
