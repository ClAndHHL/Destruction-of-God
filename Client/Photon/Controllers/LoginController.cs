using GodCommon;
using ExitGames.Client.Photon;
using GodCommon.Models;
using LitJson;
using System.Collections.Generic;

public class LoginController : ControllerBase
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
            return OperationCode.Login;
        }
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.ReturnCode)
        {
            case (short)ReturnCode.Success:
                //根据登录用户加载用户角色信息
                StartMenu.instance.OnLoginCloseClick();
                StartMenu.instance.usernameLabel_start.text = user.Username;
                MessageManager.instance.ShowMessage("登录成功", 2f);
                StartMenu.instance.isLogin = IsLogin.Yes;
                roleController.GetRole();
                break;
            case (short)ReturnCode.Failure:
                //提示登录失败信息
                MessageManager.instance.ShowMessage(operationResponse.DebugMessage, 2f);
                StartMenu.instance.isLogin = IsLogin.No;
                break;
        }
    }

    public void Login(string username, string password)
    {
        user = new User() { Username = username, Password = password };
        string json = JsonMapper.ToJson(user);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters.Add((byte)ParameterCode.Login, json);
        PhotonEngine.Instance.SendRequest(OperationCode.Login, parameters);  //发起登录请求
    }
}
