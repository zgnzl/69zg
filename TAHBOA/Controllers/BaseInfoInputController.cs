using _69zg.DataContent;
using _69zg.Filter;
using Dos.ORM;
using System.Collections.Generic;
using System.Web.Mvc;
using TAHB.Model;

namespace TAHBOA.Controllers
{
    [LoginFilter]
    public class BaseInfoInputController : MasterController
    {
       

        #region 国家危险品名录录入
        public ActionResult ListDirectory(string keyword, int pagesize = 10, int pageindex = 1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.Or(new WhereClip(_directories._.type, keyword, QueryOperator.Like));
                where.Or(new WhereClip(_directories._.source, keyword, QueryOperator.Like));
                where.Or(new WhereClip(_directories._.explain, keyword, QueryOperator.Like));
            }
            int count = DB.Context.Count<_directories>(where);
            List<_directories> models = DB.Context.From<_directories>().Where(where).OrderByDescending(c => c.id).Page(pagesize, pageindex).ToList();       
            ViewBag.models = models;
            InitPages(count, pageindex, pagesize);
            return View();
        }

        public ActionResult AddDirectory(_directories model)
        {
            if (string.IsNullOrEmpty(model.type))
            {
                return Json(new { title = "失败", message = "名录类别不能为空："});
            }
                if (DB.Context.Insert<_directories>(model) > 0)
                    return Json(new { title = "成功", message = "成功添加角色：" + model.type });
            return Json(new { title = "失败", message = "未能添加角色：" + model.type });
        }

        public ActionResult UpdateDirectory(_directories model)
        {
            if (string.IsNullOrEmpty(model.type))
            { 
                _directories oldmodel = DB.Context.From<_directories>().Where(c => c.id == model.id).First();
                return View(oldmodel);
            }

                if (DB.Context.Update<_directories>(model) > 0)
                    return Json(new { title = "成功", message = "成功修改名录：" + model.type });
            return Json(new { title = "失败", message = "修改失败名录：" + model.type });
        }

        public ActionResult DelDirectory(int id)
        {
            if (DB.Context.Delete<_directories>(id) > 0)
            {
                return Json(new { title = "成功", message = "名录删除成功" });
            }
            else
            {
                return Json(new { title = "失败", message = "名录删除失败" });
            }
        }
        #endregion


    }
}