using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _69zg.DataContent
{
    public  class DB
    {
        public static   DbSession Context = new DbSession("connstr");
        DB(string connstr = null)
        {
            if(string.IsNullOrEmpty(connstr))
            Context = new DbSession("connstr");
            else
                Context = new DbSession(connstr);
        }
    }
}
