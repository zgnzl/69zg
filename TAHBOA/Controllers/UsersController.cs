using _69zg.Common;
using _69zg.DataContent;
using _69zg.Filter;
using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TAHB.Model;

namespace TAHBOA.Controllers
{
    [LoginFilter]
    public class UsersController : MasterController
    {

        #region 用户管理
        public ActionResult UsersList(string keyword,int pagesize=10,int pageindex=1)
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

        public ActionResult DelUser(int id)
        {
            if (DB.Context.Delete<_userinfo>(id) > 0)
            {
                return Json(new { title = "成功", message = "用户删除成功" });
            }
            else {
                return Json(new { title = "失败", message = "用户删除失败" });
            }
        }

        public ActionResult UpdateUser(_userinfo model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                _userinfo oldmodel = DB.Context.From<_userinfo>().Where(c => c.id == model.id).First();
                List<_roleinfo> roles = DB.Context.From<_roleinfo>().ToList();
                ViewBag.roles = roles;
                return View(oldmodel);
            }
            if (!string.IsNullOrEmpty(model.pwd))
            {
                model.pwd = model.pwd.ToMd5();
            }
            else
            {
                model.pwd = DB.Context.From<_userinfo>().Where(c => c.id == model.id).First().pwd;
            }
            if (DB.Context.Update<_userinfo>(model) > 0)
            {
                return Json(new { title = "成功", message = "成功修改用户：" + model.name });
            }
            else
            {
                return Json(new { title = "失败", message = "修改失败用户：" + model.name });
            }
        }
        #endregion

        #region 角色管理
        public ActionResult RolesList(string keyword, int pagesize = 10, int pageindex = 1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.And(new WhereClip(_roleinfo._.name, keyword, QueryOperator.Like));
                where.And(new WhereClip(_roleinfo._.description, keyword, QueryOperator.Like));
            }
            int count = DB.Context.Count<_roleinfo>(where);
            List<_roleinfo> models = DB.Context.From<_roleinfo>().Where(where).OrderByDescending(c => c.id).Page(pagesize, pageindex).ToList();
            List<_functioninfo> functions = DB.Context.From<_functioninfo>().ToList();
            ViewBag.functions = functions;
            ViewBag.models = models;
            InitPages(count, pageindex, pagesize);
            return View();
        }
        public ActionResult AddRole(_roleinfo model)
        {
            if (!string.IsNullOrEmpty(model.functionids) &&DB.Context.Insert<_roleinfo>(model) > 0)
            {
                return Json(new { title = "成功", message = "成功添加用户：" + model.name });
            }
            else
            {
                return Json(new { title = "失败", message = "未能添加用户：" + model.name });
            }
        }
        public ActionResult DelRole(int id)
        {
            if (DB.Context.Delete<_roleinfo>(id) > 0)
            {
                return Json(new { title = "成功", message = "角色删除成功" });
            }
            else
            {
                return Json(new { title = "失败", message = "角色删除失败" });
            }
        }
        #endregion
    }
}