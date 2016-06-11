using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase
{
    public class NHibernateHelper
    {
        private static ISessionFactory factory = null;

        private static void InitFoctory()
        {
            factory = FluentNHibernate.Cfg.Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                .ConnectionString(db => db.Server("localhost")
                .Database("god")
                .Username("root")
                .Password("Zhangbincheng0")))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
                .BuildSessionFactory();
        }

        public static ISessionFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    InitFoctory();
                }
                return factory;
            }
        }

        public static ISession OpenSession()
        {
            return Factory.OpenSession();
        }
    }
}
