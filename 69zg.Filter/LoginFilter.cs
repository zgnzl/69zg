using System.Web.Mvc;

namespace _69zg.Filter
{
    public class LoginFilter : AuthorizeAttribute
    {
        private string action;
        private string controller;

        public LoginFilter()
        {
            action = "index";
            controller = "login";
        }
        public LoginFilter(string recontroller, string reaction)
        {
            action = reaction;
            controller = recontroller;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["currentuser"] == null)
            {
                object routevalue = new { controller = controller, action = action, url = filterContext.HttpContext.Request.Url.AbsolutePath };
                filterContext.HttpContext.Response.RedirectToRoute(routevalue);
                filterContext.HttpContext.Response.End();
            }
           // base.OnAuthorization(filterContext);
        }
    }
}
