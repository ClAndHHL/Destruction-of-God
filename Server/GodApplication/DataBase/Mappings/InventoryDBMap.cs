using FluentNHibernate.Mapping;
using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Mappings
{
    public class InventoryDBMap : ClassMap<InventoryDB>
    {
        public InventoryDBMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.InventoryId).Column("inventoryid");
            Map(x => x.Num).Column("num");
            Map(x => x.Level).Column("level");
            Map(x => x.IsDress).Column("isdress");
            References(x => x.Role).Column("roleid").Cascade.None();
            Table("inventory");
        }
    }
}
