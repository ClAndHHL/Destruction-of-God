using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using System.Collections.Generic;
using GodCommon.Tools;
using GodCommon.Models;

public class FightController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.Fight;
        }
    }

    public event OnHavingTeamEvent OnHavingTeam;
    public event OnWaitingTeamEvent OnWaitingTeam;
    public event OnCancelTeamEvent OnCancelTeam;
    public event OnSyncPositionAndRotationEvent OnSyncPositionAndRotation;
    public event OnSyncMoveAnimationEvent OnSyncMoveAnimation;
    public event OnSyncPlayerAnimationEvent OnSyncPlayerAnimation;
    public event OnSyncGameStateEvent OnSyncGameState;

    public override void OnEventData(EventData data)
    {
        SubCode subCode = ParameterTool.GetSubCode(data.Parameters);
        switch (subCode)
        {
            case SubCode.ConfirmTeam:
                //Debug.Log("SubCode.ConfimTeam");
                List<Role> roleList = ParameterTool.GetParameter<List<Role>>(data.Parameters, ParameterCode.RoleList);
                int masterRoleId = ParameterTool.GetParameter<int>(data.Parameters, ParameterCode.MasterRoleId, false);
                OnHavingTeam(roleList, masterRoleId);
                break;
            case SubCode.SyncPositionAndRotation:
                //Debug.Log("SubCode.SyncPlayer");
                int roleId = ParameterTool.GetParameter<int>(data.Parameters, ParameterCode.RoleId, false);
                Vector3 position = ParameterTool.GetParameter<Vector3Object>(data.Parameters, ParameterCode.Position).ToVector3();
                Vector3 eulerAngles = ParameterTool.GetParameter<Vector3Object>(data.Parameters, ParameterCode.EulerAngles).ToVector3();
                OnSyncPositionAndRotation(roleId, position, eulerAngles);
                break;
            case SubCode.SyncMoveAnimation:
                //Debug.Log("SubCode.SyncMoveAnimation");
                int roleId2 = ParameterTool.GetParameter<int>(data.Parameters, ParameterCode.RoleId, false);
                PlayerMoveAnimationModel model2 = ParameterTool.GetParameter<PlayerMoveAnimationModel>(data.Parameters, ParameterCode.MoveAnimation);
                OnSyncMoveAnimation(roleId2, model2);
                break;
            case SubCode.SyncAnimation:
                //Debug.Log("SubCode.SyncAnimation");
                int roleId3 = ParameterTool.GetParameter<int>(data.Parameters, ParameterCode.RoleId, false);
                PlayerAnimationModel model3 = ParameterTool.GetParameter<PlayerAnimationModel>(data.Parameters, ParameterCode.PlayerAnim);
                OnSyncPlayerAnimation(roleId3, model3);
                break;
            case SubCode.SyncGameState:
                //Debug.Log("SubCode.SyncGameState");
                GameStateModel model4 = ParameterTool.GetParameter<GameStateModel>(data.Parameters, ParameterCode.GameState);
                OnSyncGameState(model4);
                break;
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        SubCode subCode = ParameterTool.GetSubCode(operationResponse.Parameters);
        switch (subCode)
        {
            case SubCode.TeamFight:
                if (operationResponse.ReturnCode == (short)ReturnCode.HavingTeam)  //组队成功
                {
                    //Debug.Log("SubCode.TeamFight");
                    List<Role> roleList = ParameterTool.GetParameter<List<Role>>(operationResponse.Parameters, ParameterCode.RoleList);
                    int masterRoleId = ParameterTool.GetParameter<int>(operationResponse.Parameters, ParameterCode.MasterRoleId, false);
                    OnHavingTeam(roleList, masterRoleId);
                }
                else if (operationResponse.ReturnCode == (short)ReturnCode.WaitingTeam)  //组队失败
                {
                    //Debug.Log("SubCode.TeamFight");
                    OnWaitingTeam();
                }
                break;
            case SubCode.CancelTeam:
                //Debug.Log("SubCode.CancelFight");
                OnCancelTeam();
                break;
        }
    }

    public void SendFight()  //发起组队请求
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.TeamFight, new Dictionary<byte, object>());
    }

    public void SendCancel()  //取消组队请求
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.CancelTeam, new Dictionary<byte, object>());
    }

    public void SyncPositionAndRotation(Vector3 position, Vector3 eulerAngles)  //发起同步团队角色请求
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.Position, new Vector3Object(position));
        ParameterTool.AddParameter(parameters, ParameterCode.EulerAngles, new Vector3Object(eulerAngles));
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncPositionAndRotation, parameters);
    }

    public void SyncMoveAnimation(PlayerMoveAnimationModel model)  //发送同步移动状态请求
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.MoveAnimation, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncMoveAnimation, parameters);
    }

    public void SyncPlayerAnimation(PlayerAnimationModel model)  //发送同步动画状态请求
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.PlayerAnim, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncAnimation, parameters);
    }

    public void SendGameState(GameStateModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.GameState, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncGameState, parameters);
    }
}
