using _69zg.DataContent;
using _69zg.Filter;
using Dos.ORM;
using System.Collections.Generic;
using System.Web.Mvc;
using TAHB.Model;

namespace TAHBOA.Controllers
{
    [LoginFilter]
    public class MarketController : MasterController
    {

        #region 市场业务
        #region 合同列表
        public ActionResult ContractList(string keyword, int pagesize = 10, int pageindex = 1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.Or(new WhereClip(合同记录._.合同内容, keyword, QueryOperator.Like));
                where.Or(new WhereClip(合同记录._.合同区域, keyword, QueryOperator.Like));
                where.Or(new WhereClip(合同记录._.甲方, keyword, QueryOperator.Like));
            }
            int count = DB.Context.Count<合同记录>(where);
            List<合同记录> models = DB.Context.From<合同记录>().Where(where).OrderByDescending(c => c.id).Page(pagesize, pageindex).ToList();
            List<合同类别> contracttypes = DB.Context.From<合同类别>().ToList();
            List<区域代码> contracttareas = DB.Context.From<区域代码>().ToList();
            ViewBag.models = models;
            ViewBag.contracttypes = contracttypes;
            ViewBag.contracttareas = contracttareas;
            InitPages(count, pageindex, pagesize);
            return View();
        }


        public ActionResult AddContract(合同记录 model)
        {
            if (string.IsNullOrEmpty(model.甲方))
            {
                List<合同类别> contracttypes = DB.Context.From<合同类别>().ToList();
                List<区域代码> contracttareas = DB.Context.From<区域代码>().ToList();
                ViewBag.contracttypes = contracttypes;
                ViewBag.contracttareas = contracttareas;
                return View();
            }
            if (DB.Context.Insert<合同记录>(model) > 0)
                return Json(new { title = "成功", message = "成功添加角色：" + model.甲方 });
            return Json(new { title = "失败", message = "未能添加角色：" + model.甲方 });
        }

        public ActionResult UpdateContract(合同记录 model)
        {
            if (string.IsNullOrEmpty(model.甲方))
            {
                合同记录 oldmodel = DB.Context.From<合同记录>().Where(c => c.id == model.id).First();
                List<合同类别> contracttypes = DB.Context.From<合同类别>().ToList();
                List<区域代码> contracttareas = DB.Context.From<区域代码>().ToList();
                ViewBag.contracttypes = contracttypes;
                ViewBag.contracttareas = contracttareas;
                return View(oldmodel);
            }

            if (DB.Context.Update<合同记录>(model) > 0)
                return Json(new { title = "成功", message = "成功修改合同甲方：" + model.甲方 });
            return Json(new { title = "失败", message = "修改失败合同甲方：" + model.甲方 });
        }

        public ActionResult DelContract(int id)
        {
            if (DB.Context.Delete<合同记录>(id) > 0)
            {
                return Json(new { title = "成功", message = "合同删除成功" });
            }
            else
            {
                return Json(new { title = "失败", message = "合同删除失败" });
            }
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