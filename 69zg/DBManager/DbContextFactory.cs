using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace _69zg.DBManager
{
    public class DbContextFactory
    {
        public static string  writeConnectString="";
             public static string readConnectString="";
        public DbContext GetWriteDbContext()
        {
            string key = typeof(DbContextFactory).Name + "WriteDbContext";
            DbContext dbContext = CallContext.GetData(key) as DbContext;
            if (dbContext == null)
            {
                dbContext = new DbContext(writeConnectString); //new WriteDbContext();
                CallContext.SetData(key, dbContext);
            }
            return dbContext;
        }
        public DbContext GetReadDbContext()
        {
            string key = typeof(DbContextFactory).Name + "ReadDbContext";
            DbContext dbContext = CallContext.GetData(key) as DbContext;
            if (dbContext == null)
            {
                dbContext = new DbContext(readConnectString); // new ReadDbContext();
                CallContext.SetData(key, dbContext);
            }
            return dbContext;
        }
    }
}