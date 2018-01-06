using _69zg.Common;
using _69zg.DataContent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TAHB.Model;

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

        public ActionResult GetArea()
        {
            string parentcode= ResquestUtil.GetValue("parentcode");
            string selectcode= ResquestUtil.GetValue("selectcode");
            List<Area> area = DB.Context.From<Area>().Where(c => c.ParentCode == parentcode).ToList();
            StringBuilder areastr = new StringBuilder();
            string format = "<option value= '{0}' {1} regioncode='{2}'>{0}</option>";
            foreach (var Area in area)
            {
                if (!string.IsNullOrEmpty(selectcode)&&Area.RegionCode == selectcode)
                { areastr.AppendFormat(format, Area.RegionName, "selected", Area.RegionCode); }
                else
                { areastr.AppendFormat(format, Area.RegionName, "", Area.RegionCode); }
            }
            return Content(areastr.ToString());
        }
       
    }
}