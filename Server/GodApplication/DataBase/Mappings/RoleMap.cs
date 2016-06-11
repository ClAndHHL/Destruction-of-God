using FluentNHibernate.Mapping;
using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Mappings
{
    class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("name");
            Map(x => x.Level).Column("level");
            Map(x => x.IsMan).Column("isman");
            Map(x => x.Exp).Column("exp");
            Map(x => x.Coin).Column("coin");
            Map(x => x.Diamond).Column("diamond");
            Map(x => x.Energy).Column("energy");
            Map(x => x.Toughen).Column("toughen");
            References(x => x.User).Column("userid").Cascade.None();
            Table("role");
        }
    }
}
