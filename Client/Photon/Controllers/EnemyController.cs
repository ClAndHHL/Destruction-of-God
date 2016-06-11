using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using System.Collections.Generic;
using GodCommon.Tools;

public class EnemyController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.Enemy;
        }
    }

    public event OnSyncEnemyCreatEvent OnSyncEnemyCreat;
    public event OnSyncEnemyPositionEvent OnSyncEnemyPosition;
    public event OnSyncEnemyAnimEvent OnSyncEnemyAnim;

    public override void OnEventData(EventData data)
    {
        SubCode subCode = ParameterTool.GetSubCode(data.Parameters);
        switch (subCode)
        {
            case SubCode.SyncEnemyCreat:
                //Debug.Log("SubCode.CreatEnemy");
                EnemyCreatModel model = ParameterTool.GetParameter<EnemyCreatModel>(data.Parameters, ParameterCode.CreatEnemy);
                OnSyncEnemyCreat(model);
                break;
            case SubCode.SyncPositionAndRotation:
                //Debug.Log("SubCode.SyncPositionAndRotation");
                EnemyPositionModel model2 = ParameterTool.GetParameter<EnemyPositionModel>(data.Parameters, ParameterCode.EnemyPosition);
                OnSyncEnemyPosition(model2);
                break;
            case SubCode.SyncEnemyAnim:
                //Debug.Log("SubCode.SyncPositionAndRotation");
                EnemyAnimModel model3 = ParameterTool.GetParameter<EnemyAnimModel>(data.Parameters, ParameterCode.EnemyAnim);
                OnSyncEnemyAnim(model3);
                break;
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {

    }

    public void SendCreatEnemy(EnemyCreatModel model)  //创建敌人
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.CreatEnemy, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncEnemyCreat, parameters);
    }

    public void SyncEnemyPosition(EnemyPositionModel model)  //同步敌人位置
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.EnemyPosition, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncPositionAndRotation, parameters);
    }

    public void SyncEnemyAnim(EnemyAnimModel model)  //同步敌人动画
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.EnemyAnim, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncEnemyAnim, parameters);
    }
}
