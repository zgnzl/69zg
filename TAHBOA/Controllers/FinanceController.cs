using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAHBOA.Controllers
{
    public class FinanceController : Controller
    {
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }
        #region 财务业务
        #region 合同审批
        public ActionResult HTSP()
        {
            return Content("合同审批");
        }
        #endregion
        #region 合同打印
        public ActionResult HTDY()
        {
            return Content("合同打印");
        }
        #endregion
        #endregion
    }
}