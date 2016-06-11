using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using GodCommon;
using LitJson;
using GodCommon.Models;
using GodServer.DataBase.Managers;
using GodServer.Tools;
using GodCommon.Tools;

namespace GodServer.Handlers
{
    public class LoginHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.Login;
            }
        }

        private UserManager manager;

        public LoginHandler()
        {
            manager = new UserManager();
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            User userClient = ParameterTool.GetParameter<User>(request.Parameters, ParameterCode.Login);
            User userServer = manager.GetUserByUsername(userClient.Username);

            if (userServer != null && Md5Tool.GetMd5(userClient.Password) == userServer.Password)  //登录成功
            {
                //数据库存在用户
                //md5加密过后的客户端密码与数据库的密码比较
                response.ReturnCode = (short)ReturnCode.Success;
                peer.LoginUser = userServer;  //保存当前用户
            }
            else  //登录失败
            {
                response.ReturnCode = (short)ReturnCode.Failure;
                response.DebugMessage = "用户名或者密码错误";
            }
        }
    }
}
