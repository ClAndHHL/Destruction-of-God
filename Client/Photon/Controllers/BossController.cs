using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using GodCommon.Tools;
using System.Collections.Generic;

public class BossController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.Boss;
        }
    }

    public event OnSyncBossAnimEvent OnSyncBossAnim;

    public override void OnEventData(EventData data)
    {
        SubCode subCode = ParameterTool.GetSubCode(data.Parameters);
        switch (subCode)
        {
            case SubCode.SyncBossAnim:
                //Debug.Log("SubCode.SyncBossAnim");
                BossAnimModel model = ParameterTool.GetParameter<BossAnimModel>(data.Parameters, ParameterCode.BossAnim);
                OnSyncBossAnim(model);
                break;
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {

    }

    public void SyncBossAnim(BossAnimModel model)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.BossAnim, model);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SyncBossAnim, parameters);
    }
}
