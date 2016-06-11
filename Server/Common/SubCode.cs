using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon
{
    public enum SubCode : byte  //子操作
    {
        GetRole,
        AddRole,
        SelectRole,
        UpdateRole,

        GetTaskDBList,
        AddTaskDB,
        UpdateTaskDB,

        GetInventoryDBList,
        AddInventoryDB,
        ChangeEquipment,
        UpgradeEquipment,

        GetSkillDBList,
        AddSkillDB,
        UpgradeSkillDB,

        TeamFight,
        CancelTeam,  //取消组队，组队失败
        ConfirmTeam,  //确认组队，组队成功
        SyncPositionAndRotation,  //同步位置和旋转，包括角色和敌人
        SyncMoveAnimation, //同步移动
        SyncAnimation,  //同步动画

        SyncEnemyCreat,
        SyncEnemyAnim,
        SyncBossAnim,

        SyncGameState
    }
}
