using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _69zg.Controllers
{
    public class CompanyController : Controller
    {
        private readonly string viewpath = "~/views/Company/{0}partial.cshtml"; 
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NaviClick(string id)
        {
            return View(string.Format(viewpath,id));
        }
    }
}