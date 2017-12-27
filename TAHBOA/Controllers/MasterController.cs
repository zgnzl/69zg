using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace TAHBOA.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }
        protected void InitPages(int modelscount, int currentpage=1, int pagesize=10, string controller=null, string action=null)
        {
            if (action == null)
            {
                var ca = GetControllerActionName();
                controller = ca.Item1;
                action = ca.Item2;
            }
            ViewBag.modelscount = modelscount;
            ViewBag.pagesize = pagesize;
            ViewBag.currentpage = currentpage;
            ViewBag.controller = controller;
            ViewBag.action = action;
        }
        public ActionResult Pages(int pagesize, int pageindex, string controller, string action)
        {
            return RedirectToAction(action,controller,new { pagesize = pagesize , pageindex = pageindex });
        }
        protected Tuple<string,string> GetControllerActionName()
        {
            StackTrace stacktrace = new StackTrace();
            StackFrame stackframe = stacktrace.GetFrame(2);
            MethodBase methodbase = stackframe.GetMethod();
            return Tuple.Create(methodbase.ReflectedType.Name.Replace("Controller", ""), methodbase.Name);
        }
       
    }
}