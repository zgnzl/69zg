using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAHBOA.Controllers
{
    public class LogisticsController : Controller
    {
        // GET: Logistics
        public ActionResult Index()
        {
            return View();
        }
        #region 物流运输
        #region 仓储路线规划
        public ActionResult CCLXGH()
        {
            return Content("仓储路线规划");
        }
        #endregion
        #region 仓储区规划
        public ActionResult CCQGH()
        {
            return Content("仓储区规划");
        }
        #endregion
        #endregion
    }
}