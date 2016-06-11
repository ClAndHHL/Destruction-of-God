using FluentNHibernate.Mapping;
using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Mappings
{
    public class TaskDBMap : ClassMap<TaskDB>
    {
        public TaskDBMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.TaskId).Column("taskid");
            Map(x => x.Type).Column("type");
            Map(x => x.State).Column("state");
            Map(x => x.LastUpdateTime).Column("lastupdatetime");
            References(x => x.Role).Column("roleid").Cascade.None();
            Table("task");
        }
    }
}
