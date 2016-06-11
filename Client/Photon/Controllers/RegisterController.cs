using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using GodCommon.Models;
using LitJson;
using System.Collections.Generic;

public class RegisterController : ControllerBase
{
    private RoleController roleController;
    private User user;

    public override void Start()
    {
        base.Start();
        roleController = GetComponent<RoleController>();
    }

    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.Register;
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.ReturnCode)
        {
            case (short)ReturnCode.Success:
                //注册成功
                StartMenu.instance.OnRegisterCloseClick();
                StartMenu.instance.usernameLabel_start.text = user.Username;
                MessageManager.instance.ShowMessage("注册成功", 2f);
                StartMenu.instance.isLogin = IsLogin.Yes;
                roleController.GetRole();  //获得角色列表
                break;
            case (short)ReturnCode.Failure:
                //提示注册失败信息
                MessageManager.instance.ShowMessage(operationResponse.DebugMessage, 2f);
                break;
        }
    }

    public void Register(string username, string password)
    {
        user = new User() { Username = username, Password = password };
        string json = JsonMapper.ToJson(user);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Register, json);
        PhotonEngine.Instance.SendRequest(OperationCode.Register, parameters);  //发起注册请求
    }
}
