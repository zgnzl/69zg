using _69zg.DataContent;
using _69zg.Filter;
using Dos.ORM;
using System.Collections.Generic;
using System.Text;
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
            model.合同编号 = GetContractId(model);
            if (DB.Context.Insert<合同记录>(model) > 0)
                return Json(new { title = "成功", message = "成功添加角色：" + model.甲方 });
            return Json(new { title = "失败", message = "未能添加角色：" + model.甲方 });
        }
        private string GetContractId(合同记录 model)
        {
            StringBuilder id = new StringBuilder();
            id.Append(DB.Context.From<合同类别>().Where(c => c.id == model.合同类别).First().code.Trim());
            id.Append(DB.Context.From<区域代码>().Where(c => c.id == model.合同区域).First().code.Trim());
            id.Append(model.签订时间.Value.Year);
            int count = 1;
            List<合同记录> list = DB.Context.From<合同记录>().Where(c => c.合同类别 == model.合同类别 && c.合同区域 == model.合同区域).ToList<合同记录>();
            if (list != null && list.Count > 0)
            {
                count = list.Count + 1;
            }
            id.Append(count.ToString().PadLeft(4, '0'));
            return id.ToString();
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

        #region 合同类别

        public ActionResult ContractTypeList(string keyword, int pagesize = 10, int pageindex = 1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.Or(new WhereClip(合同类别._.name, keyword, QueryOperator.Like));
            }
            int count = DB.Context.Count<合同类别>(where);
            List<合同类别> models = DB.Context.From<合同类别>().Where(where).OrderByDescending(c => c.id).Page(pagesize, pageindex).ToList();
            ViewBag.models = models;
            InitPages(count, pageindex, pagesize);
            return View();
        }

        public ActionResult AddContractType(合同类别 model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                return View();
            }
            if (DB.Context.Insert<合同类别>(model) > 0)
                return Json(new { title = "成功", message = "成功添加合同类别：" + model.name });
            return Json(new { title = "失败", message = "未能添加合同类别：" + model.name });
        }

        public ActionResult UpdateContractType(合同类别 model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                合同类别 oldmodel = DB.Context.From<合同类别>().Where(c => c.id == model.id).First();
                return View(oldmodel);
            }

            if (DB.Context.Update<合同类别>(model) > 0)
                return Json(new { title = "成功", message = "成功修改合同类别：" + model.name });
            return Json(new { title = "失败", message = "修改失败合同类别：" + model.name });
        }

        public ActionResult DelContractType(int id)
        {
            if (DB.Context.Delete<合同类别>(id) > 0)
            {
                return Json(new { title = "成功", message = "合同类别删除成功" });
            }
            else
            {
                return Json(new { title = "失败", message = "合同类别删除失败" });
            }
        }

        #endregion

        #region 区域代码 

        public ActionResult AreaCodeList(string keyword, int pagesize = 10, int pageindex = 1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.Or(new WhereClip(区域代码._.area, keyword, QueryOperator.Like));
            }
            int count = DB.Context.Count<区域代码>(where);
            List<区域代码> models = DB.Context.From<区域代码>().Where(where).OrderByDescending(c => c.id).Page(pagesize, pageindex).ToList();
            ViewBag.models = models;
            InitPages(count, pageindex, pagesize);
            return View();
        }

        public ActionResult AddAreaCode(区域代码 model)
        {
            if (string.IsNullOrEmpty(model.area))
            {
                List<Area> area = DB.Context.From<Area>().Where(c => c.ParentCode == "000000").ToList();
                ViewBag.area = area;
                return View();
            }
            if (DB.Context.Insert<区域代码>(model) > 0)
                return Json(new { title = "成功", message = "成功添加区域代码：" + model.area });
            return Json(new { title = "失败", message = "未能添加区域代码：" + model.area });
        }

        public ActionResult UpdateAreaCode(区域代码 model)
        {
            if (string.IsNullOrEmpty(model.area))
            {
                List<Area> area = DB.Context.From<Area>().Where(c => c.ParentCode == "000000").ToList();
                ViewBag.area = area;
                区域代码 oldmodel = DB.Context.From<区域代码>().Where(c => c.id == model.id).First();
                return View(oldmodel);
            }

            if (DB.Context.Update<区域代码>(model) > 0)
                return Json(new { title = "成功", message = "成功修改区域代码：" + model.area });
            return Json(new { title = "失败", message = "修改失败区域代码：" + model.area });
        }

        public ActionResult DelAreaCode(int id)
        {
            if (DB.Context.Delete<区域代码>(id) > 0)
            {
                return Json(new { title = "成功", message = "区域代码删除成功" });
            }
            else
            {
                return Json(new { title = "失败", message = "区域代码删除失败" });
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