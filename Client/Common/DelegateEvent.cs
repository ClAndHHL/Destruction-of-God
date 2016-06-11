using GodCommon.Models;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnSyncTaskFinishedEvent();  //任务同步完成调用

public delegate void OnSyncInventoryFinishedEvent();  //装备同步完成调用

public delegate void OnSyncSkillFinishedEvent();  //技能同步完成调用

public delegate void OnGetRoleEvent(List<Role> roleList);

public delegate void OnAddRoleEvent(Role role);

public delegate void OnSelectRoleEvent();

public delegate void OnGetTaskDBListEvent(List<TaskDB> taskList);

public delegate void OnAddTaskDBEvent(TaskDB taskDB);

public delegate void OnUpdateTaskDBEvent();

public delegate void OnGetInventoryDBListEvent(List<InventoryDB> inventoryList);

public delegate void OnAddInventoryDBEvent(InventoryDB inventoryDB);

public delegate void OnChangeEquipmentEvent();

public delegate void OnUpgradeEquipmentEvent();

public delegate void OnGetSkillDBListEvent(List<SkillDB> skillLsit);

public delegate void OnAddSkillDBEvent(SkillDB skillDB);

public delegate void OnUpgradeSkillDBEvent(SkillDB skillDB);

public delegate void OnPlayerHpChangeEvent(int hp_now);

public delegate void OnHavingTeamEvent(List<Role> roleList, int masterRoleId);  //组队成功

public delegate void OnWaitingTeamEvent();  //等待组队

public delegate void OnCancelTeamEvent();  //取消组队

public delegate void OnSyncPositionAndRotationEvent(int roleId, Vector3 position, Vector3 eulerAngles);  //同步团队角色位置和旋转

public delegate void OnSyncMoveAnimationEvent(int roleId, PlayerMoveAnimationModel model);  //同步角色移动动画

public delegate void OnSyncPlayerAnimationEvent(int roleId, PlayerAnimationModel model);  //同步角色动画

public delegate void OnSyncEnemyCreatEvent(EnemyCreatModel model);  //同步敌人的创建

public delegate void OnSyncEnemyPositionEvent(EnemyPositionModel model);  //同步敌人位置和旋转

public delegate void OnSyncEnemyAnimEvent(EnemyAnimModel model);  //同步敌人动画

public delegate void OnSyncBossAnimEvent(BossAnimModel model);  //同步boss动画

public delegate void OnSyncGameStateEvent(GameStateModel model);  //同步游戏状态
