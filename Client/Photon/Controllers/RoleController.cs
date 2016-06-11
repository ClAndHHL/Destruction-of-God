using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using System.Collections.Generic;
using GodCommon.Tools;
using GodCommon.Models;
using LitJson;

public class RoleController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.Role;
        }
    }

    public event OnGetRoleEvent OnGetRole;
    public event OnAddRoleEvent OnAddRole;
    public event OnSelectRoleEvent OnSelectRole;

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        //根据子操作代码分别进行不同的处理
        SubCode subCode = ParameterTool.GetParameter<SubCode>(operationResponse.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetRole:
                //Debug.Log("SubCode.GetRole");
                List<Role> list = ParameterTool.GetParameter<List<Role>>(operationResponse.Parameters, ParameterCode.RoleList);
                OnGetRole(list);
                break;
            case SubCode.AddRole:
                //Debug.Log("SubCode.AddRole");
                Role role = ParameterTool.GetParameter<Role>(operationResponse.Parameters, ParameterCode.Role);
                OnAddRole(role);
                break;
            case SubCode.SelectRole:
                //Debug.Log("SubCode.SelectRole");
                OnSelectRole();
                break;
        }
    }

    public void GetRole()  //向服务器获得角色列表，登录成功时候调用
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.SubCode, SubCode.GetRole);  //子操作Get
        PhotonEngine.Instance.SendRequest(OpCode, parameters);

    }

    public void AddRole(Role role)  //向服务器添加角色，创建角色时候调用
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));  //子操作Add
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.AddRole, parameters);  //重载

    }

    public void SelectRole(Role role)   //向服务器保存当前角色，进入游戏时候调用
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));  //子操作Select
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.SelectRole, parameters);  //重载

    }

    public void UpdateRole(Role role)  //向服务器更新当前角色，角色发生变化时候调用
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Role, JsonMapper.ToJson(role));  //子操作Update
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpdateRole, parameters);  //重载
    }
}
