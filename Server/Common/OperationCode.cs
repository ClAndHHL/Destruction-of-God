using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon
{
    public enum OperationCode : byte
    {
        Login,
        Register,
        ServerList,
        Role,
        TaskDB,
        InventoryDB,
        SkillDB,
        Fight,
        Enemy,
        Boss
    }
}
