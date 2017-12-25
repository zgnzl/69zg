using _69zg.Common;
using _69zg.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _69zg.DBManager
{
    public class MSSQLManager:Controller
    {
        private static EntitiesSqlConnStr dmodel;

        private static MSSQLManager mssqlManager ;

        public static EntitiesSqlConnStr GetDmodel() {
            if (dmodel == null)
            {
                dmodel = new EntitiesSqlConnStr();
            }
            return dmodel;
        }

        public static MSSQLManager getMsslManager() {
            if (mssqlManager == null)
            {
                mssqlManager = new MSSQLManager();
            }
            return mssqlManager;
        }

        public  ActionResult SaveModel(object model, EntityState entityState, IEnumMessageCode messagecode, string dispalyname, IEnumMessageCode javascriptcode = IEnumMessageCode.defaulted, string javascriptfunctionparam = "")
        {
            GetDmodel().Entry(model).State = entityState;
            GetDmodel().SaveChanges();
            return ReturnContent(((int)messagecode).ToString(), dispalyname, ((int)javascriptcode).ToString(), javascriptfunctionparam);
        }

        public  ActionResult UpdateModel(object model, object updmodel, IEnumMessageCode messagecode, string dispalyname, IEnumMessageCode javascriptcode = IEnumMessageCode.defaulted, string javascriptfunctionparam ="" )
        {
            GetDmodel().Entry(updmodel).CurrentValues.SetValues(model);
            GetDmodel().Entry(updmodel).State = EntityState.Modified;
            GetDmodel().SaveChanges();
            return ReturnContent(((int)messagecode).ToString(), dispalyname, ((int)javascriptcode).ToString(), javascriptfunctionparam);
        }

        public  ActionResult ReturnContent(string messagecode, string dispalyname, string javascriptcode, string javascriptfunctionparam)
        {
            return Content(string.Format(messagecode.GetMessage(), dispalyname) + string.Format(javascriptcode.GetMessage(), javascriptfunctionparam));
        }

        private static T GetModelbyLambda<T>()
        {

            return default(T);
        }
        //是否读写分离(可以配置在配置文件中)
        private static readonly bool IsReadWriteSeparation = true;
        #region EF上下文对象(主库)      
        protected DbContext MasterDb => _masterDb.Value;
        private readonly Lazy<DbContext> _masterDb = new Lazy<DbContext>(() => new DbContextFactory().GetWriteDbContext());
        #endregion EF上下文对象(主库)    

        #region EF上下文对象(从库)    
        protected DbContext SlaveDb => IsReadWriteSeparation ? _slaveDb.Value : _masterDb.Value;
        private readonly Lazy<DbContext> _slaveDb = new Lazy<DbContext>(() => new DbContextFactory().GetReadDbContext());
        #endregion EF上下文对象(从库)     


        /// <summary>
        /// 执行存储过程或自定义sql语句--返回集合(自定义返回类型)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public List<TModel> Query<TModel>(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text)
        {            //存储过程（exec getActionUrlId @name,@ID）
            if (cmdType == CommandType.StoredProcedure)
            {
                StringBuilder paraNames = new StringBuilder();
                foreach (var sqlPara in parms)
                {
                    paraNames.Append($" @{sqlPara},");
                }
                sql = paraNames.Length > 0 ? $"exec {sql} {paraNames.ToString().Trim(',')}" : $"exec {sql} ";
            }
            var list = SlaveDb.Database.SqlQuery<TModel>(sql, parms.ToArray()); var enityList = list.ToList(); return enityList;
        }
        /// <summary>
        /// 自定义语句和存储过程的增删改--返回影响的行数  
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public int Execute(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text)
        {            //存储过程（exec getActionUrlId @name,@ID）
            if (cmdType == CommandType.StoredProcedure)
            {
                StringBuilder paraNames = new StringBuilder(); foreach (var sqlPara in parms)
                {
                    paraNames.Append($" @{sqlPara},");
                }
                sql = paraNames.Length > 0 ?
                    $"exec {sql} {paraNames.ToString().Trim(',')}" :
                    $"exec {sql} ";
            }
            int ret = MasterDb.Database.ExecuteSqlCommand(sql, parms.ToArray()); return ret;
        }
    }  
    }