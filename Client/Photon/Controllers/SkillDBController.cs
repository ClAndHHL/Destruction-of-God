using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using System.Collections.Generic;
using GodCommon.Models;
using GodCommon.Tools;

public class SkillDBController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.SkillDB;
        }
    }

    public event OnGetSkillDBListEvent OnGetSkillList;
    public event OnAddSkillDBEvent OnAddSkillDB;
    public event OnUpgradeSkillDBEvent OnUpgradeSkillDB;

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        SubCode subCode = ParameterTool.GetSubCode(operationResponse.Parameters);     
        switch (subCode)
        {
            case SubCode.GetSkillDBList:
                //Debug.Log("OnGetSkillList");
                List<SkillDB> skillList = ParameterTool.GetParameter<List<SkillDB>>(operationResponse.Parameters,ParameterCode.SkillDBList);
                OnGetSkillList(skillList);           
                break;
            case SubCode.AddSkillDB:
                //Debug.Log("OnAddSkillDB");
                SkillDB skillAdd = ParameterTool.GetParameter<SkillDB>(operationResponse.Parameters, ParameterCode.SkillDB);
                OnAddSkillDB(skillAdd);
                break;
            case SubCode.UpgradeSkillDB:
                //Debug.Log("OnUpgradeSkillDB");
                SkillDB skillUpgrade = ParameterTool.GetParameter<SkillDB>(operationResponse.Parameters, ParameterCode.SkillDB);
                OnUpgradeSkillDB(skillUpgrade);
                break;
        }
    }

    public void GetSkillList()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.GetSkillDBList, new Dictionary<byte, object>());
    }

    public void AddSkill(SkillDB skill)
    {
        skill.Role = null;
        Role role = PhotonEngine.Instance.role;
        role.User = null;
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.SkillDB, skill);
        ParameterTool.AddParameter(parameters, ParameterCode.Role, role);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.AddSkillDB, parameters);
    }

    public void UpgradeSkill(SkillDB skill)
    {
        skill.Role = null;
        Role role = PhotonEngine.Instance.role;
        role.User = null;
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.SkillDB, skill);
        ParameterTool.AddParameter(parameters, ParameterCode.Role, role);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpgradeSkillDB, parameters);
    }
}
