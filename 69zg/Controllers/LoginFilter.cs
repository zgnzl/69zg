using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _69zg.Controllers
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["currentuser"] != null)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                object routevalue = new { controller = "Login", action = "Index", url = filterContext.HttpContext.Request.Url.AbsolutePath };
                filterContext.HttpContext.Response.RedirectToRoute(routevalue);
            }
        }
    }
}