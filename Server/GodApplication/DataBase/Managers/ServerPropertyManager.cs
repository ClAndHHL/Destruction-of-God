using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Managers
{
    public class ServerPropertyManager
    {
        public List<ServerProperty> GetServerList()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transcation = session.BeginTransaction())
                {
                    var list = session.QueryOver<ServerProperty>();
                    return (List<ServerProperty>)list.List<ServerProperty>();
                }
            }
        }
    }
}
