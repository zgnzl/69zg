using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAHBOA.Controllers
{
    public class MarketController : Controller
    {
        // GET: Market
        public ActionResult Index()
        {
            return View();
        }
        #region 市场业务
        #region 合同编码设置
        public ActionResult HTBMSE()
        {
            return Content("合同编码设置");
        }
        #endregion
        #region 报价系统设置
        public ActionResult BJXTSZ()
        {
            return Content("报价系统设置");
        }
        #endregion
        #region 危废查询设置
        public ActionResult WFCXSZ()
        {
            return Content("危废查询设置");
        }
        #endregion
        #region 业务审批权限设置
        public ActionResult YWSPQXSZ()
        {
            return Content("业务审批权限设置");
        }
        #endregion
        #region 设置合同到期提醒
        public ActionResult HTDQTX()
        {
            return Content("设置合同到期提醒");
        }
        #endregion
        #endregion
    }
}