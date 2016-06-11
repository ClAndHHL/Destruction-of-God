using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon
{
    public enum ParameterCode : byte
    {
        Login,
        Register,
        ServerList,
        SubCode,  //子操作
        OperationCode,  //操作码
        Role,
        RoleList,  //角色列表
        TaskDB,
        TaskDBList,  //任务列表
        InventoryDB,
        InventoryDBList,  //物品列表
        SkillDB,
        SkillDBList,  //技能列表
        MasterRoleId,  //主机角色id
        Position,  //同步位置
        EulerAngles,  //同步旋转
        RoleId,  //表示更新的是哪个客户端
        MoveAnimation,
        CreatEnemy,
        EnemyPosition,
        EnemyAnim,
        PlayerAnim,
        BossAnim,
        GameState
    }
}
