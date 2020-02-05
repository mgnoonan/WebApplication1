using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
    }
}
