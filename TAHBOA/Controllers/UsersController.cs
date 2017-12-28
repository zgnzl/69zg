using _69zg.Common;
using _69zg.DataContent;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TAHB.Model;

namespace TAHBOA.Controllers
{
    public class UsersController : MasterController
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        #region 用户管理
        public ActionResult UsersList(string keyword,int pagesize=2,int pageindex=1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.And(new WhereClip(_userinfo._.name,keyword,QueryOperator.Like));
                where.And(new WhereClip(_userinfo._.phone, keyword, QueryOperator.Like));
            }
           int count= DB.Context.Count<_userinfo>(where);
            List<_userinfo> models = DB.Context.From<_userinfo>().Where(where).OrderByDescending(c=>c.id).Page(pagesize, pageindex).ToList();
            ViewBag.models = models;
            InitPages(count, pageindex, pagesize);
            return View();
        }
        public ActionResult AddUser(_userinfo model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                return View();
            }
            if (string.IsNullOrEmpty(model.pwd))
            {
                return Json(new { title = "提示", message = "密码不能为空" });
            }
            else
            {
                model.pwd = model.pwd.ToMd5();
                model.regtime = DateTime.Now;
            }
            if (DB.Context.Insert<_userinfo>(model) > 0)
            {
                return Json(new { title = "成功", message = "成功添加用户：" + model.name });
            }
            else
            {
                return Json(new { title = "失败", message = "未能添加用户：" + model.name });
            }
        }
        public ActionResult UpdateUserRole()
        {
            return Content("用户角色修改");
        }
        #endregion

        #region 角色管理
        public ActionResult RolesList()
        {
            return Content("角色列表");
        }
        public ActionResult AddRole()
        {
            return Content("新增角色");
        }
        #endregion
    }
}