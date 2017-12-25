using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAHBOA.Controllers
{
    public class CustomErrorsController : Controller
    {
        // GET: CustomErrors
        public ActionResult Index(String ID)
        {
            return View(ID);
        }
    }
}