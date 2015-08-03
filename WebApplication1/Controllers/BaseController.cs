using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentAssemblyVersion = string.Format("v. {0}", this.CurrentAssemblyVersion);
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