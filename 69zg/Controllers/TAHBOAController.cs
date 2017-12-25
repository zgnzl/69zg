using _69zg.Common;
using _69zg.DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using _69zg.Models;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
/// <summary>
/// niezl add 2017年3月2日
/// </summary>

namespace _69zg.Controllers
{
    public class TAHBOAController: Controller 
    {
        // GET: TAHBOA
        //[LoginFilter]
        public ActionResult Index()
        {
            if (!Request.IsLocal)
                return Content("您的请求被禁止；<br/>原因：非法IP访问：" + Request.UserHostAddress+"<br/>请联系管理员：nzlwan@126.com");
            C_userinfo users = Session["currentuser"] as C_userinfo;
            if (users != null)
            {
                ViewBag.username = users.name;
            }
            else
            {
                ViewBag.username = "游客";
            }
            return View();
        }

        #region 功能列表
        public ActionResult AddC_functioninfo(C_functioninfo model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.name))
            {
                model.controllername = "TAHBOA";
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.displayname);
            }
            return PartialView();
        }
        public ActionResult DelC_functioninfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView();
            }
            int cid = int.Parse(id);
            C_functioninfo funtioninfo = MSSQLManager.GetDmodel().C_functioninfo.FirstOrDefault(c => c.id == cid);
            MSSQLManager.GetDmodel().C_functioninfo.Remove(funtioninfo);
            return EditAndDel(funtioninfo.displayname); ;
        }
        public ActionResult EditC_functioninfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView();
            }

            int cid = int.Parse(id);
            C_functioninfo funtioninfo = MSSQLManager.GetDmodel().C_functioninfo.FirstOrDefault(c => c.id == cid);
            funtioninfo.fatherid = int.Parse(ResquestUtil.GetValue(Request, "fatherid", funtioninfo.fatherid.ToString()));
            funtioninfo.displayname = ResquestUtil.GetValue(Request, "displayname");
            funtioninfo.description = ResquestUtil.GetValue(Request, "description");
            funtioninfo.actionname = ResquestUtil.GetValue(Request, "actionname");
            MSSQLManager.GetDmodel().Entry(funtioninfo).State = EntityState.Modified;
            return EditAndDel(funtioninfo.displayname);
        }
        private ActionResult EditAndDel(string name)
        {
            JsonResult JS = new JsonResult();
            var resultjson = new
            {
                name = name,
                result = "SUCCESS"
            };
            C_userinfo user = Session["currentuser"] as C_userinfo;
            if (user != null && user.name == "聂智林")
            {
                try
                {
                    if (MSSQLManager.GetDmodel().SaveChanges() < 1)
                    {
                        resultjson = new
                        {
                            name = string.Format("2000".GetMessage(), name),
                            result = "FILED"
                        };
                    }
                }
                catch (Exception e)
                {
                    resultjson = new
                    {
                        name = e.Message,
                        result = "FILED"
                    };
                }
            }
            else
            {
                resultjson = new
                {
                    name = "1000".GetMessage(),
                    result = "FILED"
                };
            }
            JS.Data = resultjson;
            JS.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return JS;
        }
        public ActionResult UpdateC_contract()
        {
            string typ = ResquestUtil.GetValue(HttpContext.Request, "type", "");
            switch (typ)
            {
                case "list":

                    return PartialView("listC_contract", GetIEList<合同记录>());
                default: return PartialView();

            }
        }
        private IEnumerable<T> GetIEList<T>()
        {
            string contract_type = ResquestUtil.GetValue(HttpContext.Request, "contract_type", "");
            string contract_area = ResquestUtil.GetValue(HttpContext.Request, "contract_area", "");
            string contract_id = ResquestUtil.GetValue(HttpContext.Request, "contract_id", "");
            string contract_yf = ResquestUtil.GetValue(HttpContext.Request, "contract_yf", "");
            if (!string.IsNullOrEmpty(contract_id))
            {

            }
            return null;
        }
        #endregion

        #region 合同记录
        public ActionResult AddC_contract(合同记录 model)
        {

            if (model.签订时间 != null && ModelState.IsValid)
            {
                model.合同编号 = GetContractId(model);
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.合同编号);
            }
            else
            {
                return PartialView();
            }
        }
        private string GetContractId(合同记录 model)
        {
            StringBuilder id = new StringBuilder();
            id.Append(MSSQLManager.GetDmodel().合同类别.FirstOrDefault(c => c.id == model.合同类别).code.Trim());
            id.Append(MSSQLManager.GetDmodel().区域代码.FirstOrDefault(c => c.id == model.合同区域).code.Trim());
            id.Append(model.签订时间.Value.Year);
            int count = 1;
            List<合同记录> list = MSSQLManager.GetDmodel().合同记录.Where(c => c.合同类别 == model.合同类别 && c.合同区域 == model.合同区域).ToList<合同记录>();
            if (list != null && list.Count > 0)
            {
                count = list.Count + 1;
            }
            id.Append(count.ToString().PadLeft(4, '0'));
            return id.ToString();
        }
        public ActionResult AlertContract(long id)
        {
            return View(MSSQLManager.GetDmodel().合同记录.FirstOrDefault(c => c.id == id));
        }
        public ActionResult ListContracts()
        {
            return View(MSSQLManager.GetDmodel().合同记录.ToList());
        }
        public ActionResult DetailContracts(long id)
        {
            return View(MSSQLManager.GetDmodel().合同记录.FirstOrDefault(c => c.id == id));
        }
        public ActionResult DelContracts(long id)
        {
            合同记录 model = MSSQLManager.GetDmodel().合同记录.FirstOrDefault(c => c.id == id);
            return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Deleted, IEnumMessageCode.DeleteSuccess, model.合同编号, IEnumMessageCode.back, "ListContracts");
        }
        public ActionResult UpdateCcontract(合同记录 model)
        {
            if (!ModelState.IsValid)
            {
                return Content(string.Format("2004".GetMessage(), model.合同编号));
            }
            合同记录 updmodel = MSSQLManager.GetDmodel().合同记录.FirstOrDefault(c => c.id == model.id);
            return MSSQLManager.getMsslManager().UpdateModel(model, updmodel, IEnumMessageCode.uppdateSuccess, model.合同编号);
        }

        #endregion

        #region 区域代码
        public ActionResult AddAreaCode(区域代码 model)
        {
            if (string.IsNullOrEmpty(model.code))
            {
                return View();
            }
            else
            {
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.area);
            }

        }
        public ActionResult ListAreaCode()
        {
            return View(MSSQLManager.GetDmodel().区域代码.ToList());
        }
        public ActionResult DetailAreaCode(int id)
        {
            return View(MSSQLManager.GetDmodel().区域代码.FirstOrDefault(c => c.id == id));
        }
        public ActionResult EditAreaCode(int id)
        {

            return View(MSSQLManager.GetDmodel().区域代码.FirstOrDefault(c => c.id == id));
        }
        public ActionResult UpdateAreaCode(区域代码 model)
        {
            if (ModelState.IsValid)
            {
                区域代码 updmodel = MSSQLManager.GetDmodel().区域代码.FirstOrDefault(c => c.id == model.id);
                return MSSQLManager.getMsslManager().UpdateModel(model, updmodel, IEnumMessageCode.uppdateSuccess, model.area);
            }
            else
            {
                return Content(string.Format("2004".GetMessage(), model.area));
            }
        }
        public ActionResult DelAreaCode(int id)
        {
            区域代码 model = MSSQLManager.GetDmodel().区域代码.FirstOrDefault(C => C.id == id);
            return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Deleted, IEnumMessageCode.DeleteSuccess, model.area, IEnumMessageCode.back, "ListAreaCode");
        }

        #endregion

        #region 合同类别
        public ActionResult AddContratType(合同类别 model)
        {
            if (string.IsNullOrEmpty(model.code))
            {
                return View();
            }
            else
            {
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.name);
            }
        }
        public ActionResult ListContratType()
        {
            return View(MSSQLManager.GetDmodel().合同类别.ToList());
        }
        public ActionResult EditContratType(int id)
        {

            return View(MSSQLManager.GetDmodel().合同类别.FirstOrDefault(c => c.id == id));
        }
        public ActionResult UpdateContratType(合同类别 model)
        {
            if (ModelState.IsValid)
            {
                合同类别 updmodel = MSSQLManager.GetDmodel().合同类别.FirstOrDefault(c => c.id == model.id);
                return MSSQLManager.getMsslManager().UpdateModel(model, updmodel, IEnumMessageCode.uppdateSuccess, model.name);
            }
            else
            {
                return Content(string.Format("2004".GetMessage(), model.name));
            }
        }
        public ActionResult DelContratType(int id)
        {
            合同类别 model = MSSQLManager.GetDmodel().合同类别.FirstOrDefault(C => C.id == id);
            return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Deleted, IEnumMessageCode.addSucess, model.name, IEnumMessageCode.back, "ListContratType");
        }
        #endregion

        #region 仓库管理
        public ActionResult AddStorageType(仓库转移类别 model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                return View();
            }
            else
            {
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.name);
            }
        }
        public ActionResult ListStorageType()
        {
            return View(MSSQLManager.GetDmodel().仓库转移类别.ToList());
        }
        public ActionResult EditStorageType(int id)
        {

            return View(MSSQLManager.GetDmodel().仓库转移类别.FirstOrDefault(c => c.id == id));
        }
        public ActionResult UpdateStorageType(仓库转移类别 model)
        {
            if (ModelState.IsValid)
            {
                仓库转移类别 updmodel = MSSQLManager.GetDmodel().仓库转移类别.FirstOrDefault(c => c.id == model.id);
                return MSSQLManager.getMsslManager().UpdateModel(model, updmodel, IEnumMessageCode.uppdateSuccess, model.name);
            }
            else
            {
                return Content(string.Format("2004".GetMessage(), model.name));
            }
        }
        public ActionResult DelStorageType(int id)
        {
            仓库转移类别 model = MSSQLManager.GetDmodel().仓库转移类别.FirstOrDefault(C => C.id == id);
            return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Deleted, IEnumMessageCode.addSucess, model.name, IEnumMessageCode.back, "ListStorageType");
        }
        #endregion

        #region 国家危险废物名录
        public ActionResult AddDirectory(C_directories model)
        {
            if (string.IsNullOrEmpty(model.type))
            {
                return View();
            }
            else
            {
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.type);
            }
        }
        public ActionResult ListDirectory()
        {
            return View(MSSQLManager.GetDmodel().C_directories.ToList());
        }
        public ActionResult EditDirectory(int id)
        {

            return View(MSSQLManager.GetDmodel().C_directories.FirstOrDefault(c => c.id == id));
        }
        public ActionResult DetailDirectory(int id)
        {
            return View(MSSQLManager.GetDmodel().C_directories.FirstOrDefault(c => c.id == id));
        }
        public ActionResult UpdateDirectory(C_directories model)
        {
            if (ModelState.IsValid)
            {
                C_directories updmodel = MSSQLManager.GetDmodel().C_directories.FirstOrDefault(c => c.id == model.id);
                return MSSQLManager.getMsslManager().UpdateModel(model, updmodel, IEnumMessageCode.uppdateSuccess, model.type);
            }
            else
            {
                return Content(string.Format("2004".GetMessage(), model.type));
            }
        }
        public ActionResult DelDirectory(int id)
        {
            C_directories model = MSSQLManager.GetDmodel().C_directories.FirstOrDefault(C => C.id == id);
            return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Deleted, IEnumMessageCode.addSucess, model.type , IEnumMessageCode.back, "ListDirectory");
        }
        #endregion

        #region 仓库记录
        public ActionResult AddStorage(仓库记录 model)
        {
            if (string.IsNullOrEmpty(model.source))
            {
                return View();
            }
            else
            {
                return MSSQLManager.getMsslManager().SaveModel(model, EntityState.Added, IEnumMessageCode.addSucess, model.source);
            }
        }
        #endregion


    }
}