using FluentNHibernate.Mapping;
using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Mappings
{
    public class SkillDBMap : ClassMap<SkillDB>
    {
        public SkillDBMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.SkillId).Column("skillid");
            Map(x => x.Level).Column("level");
            References(x => x.Role).Column("roleid").Cascade.None();
            Table("skill");
        }
    }
}
