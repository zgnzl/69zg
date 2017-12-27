using _69zg.DataContent;
using Dos.ORM;
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
        public ActionResult UsersList(string keyword,int pagesize=1,int pageindex=1)
        {
            WhereClip where = new WhereClip();
            if (!string.IsNullOrEmpty(keyword))
            {
                where.And(new WhereClip(_userinfo._.name,keyword,QueryOperator.Like));
                where.And(new WhereClip(_userinfo._.phone, keyword, QueryOperator.Like));
            }
           int count= DB.Context.Count<_userinfo>(where);
            List<_userinfo> models = DB.Context.From<_userinfo>().Where(where).Page(pagesize, pageindex).OrderByDescending(c=>c.regtime).ToList();
            ViewBag.models = models;
            InitPages(count, pageindex, pagesize);
            return View();
        }
        public ActionResult AddUser()
        {
            return Content("新增用户");
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