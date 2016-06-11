using FluentNHibernate.Mapping;
using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Mappings
{
    class ServerPropertyMap : ClassMap<ServerProperty>
    {
        public ServerPropertyMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.Ip).Column("ip");
            Map(x => x.Name).Column("name");
            Map(x => x.Count).Column("count");
            Table("serverproperty");
        }
    }
}
