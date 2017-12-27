using _69zg.DataContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAHBOA.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        #region 用户管理
        public ActionResult UsersList(int pagesize=1,int pageindex=10)
        {
            DB.Context.FROM
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