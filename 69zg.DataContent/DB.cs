using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace _69zg.DataContent
{
    public  class DB<T> where  T:Entity
    {
        public static   DbSession Context = new DbSession("connstr");
        DB(string connstr = null)
        {
            if(string.IsNullOrEmpty(connstr))
            Context = new DbSession("connstr");
            else
                Context = new DbSession(connstr);
        }
        public static Tuple<int, List<T>> GetModels(Where<T> where,)
        {
            return null;
        }
    }
}
