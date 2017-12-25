using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _69zg.Controllers
{
    public class HCController : Controller
    {
        // GET: HC
        public ActionResult Index(string id)
        {
            
            if(id== "95259c16-c891-4581-96d3-326429dddab3")
            { 
            return View();
            }
            return Content("本次已结束");
        }
    }
}