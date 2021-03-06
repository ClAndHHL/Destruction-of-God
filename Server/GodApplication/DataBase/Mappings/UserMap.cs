﻿using FluentNHibernate.Mapping;
using GodCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.DataBase.Mappings
{
    class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).Column("id");
            Map(x => x.Username).Column("username");
            Map(x => x.Password).Column("password");
            Table("user");
        }
    }
}
