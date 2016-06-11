using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using GodServer.DataBase.Managers;
using GodCommon.Models;
using GodCommon.Tools;
using GodCommon;
using GodServer.Tools;

namespace GodServer.Handlers
{
    public class RegisterHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.Register;
            }
        }

        private UserManager manager;

        public RegisterHandler()
        {
            manager = new UserManager();
        }      

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            User userClient = ParameterTool.GetParameter<User>(request.Parameters, ParameterCode.Register);
            User userServer = manager.GetUserByUsername(userClient.Username);

            if (userServer != null)  //用户名重复
            {
                response.ReturnCode = (short)ReturnCode.Failure;
                response.DebugMessage = "用户名重复";
            }
            else  //存入数据库
            {
                userClient.Password = Md5Tool.GetMd5(userClient.Password);  //md5加密
                manager.AddUser(userClient);
                peer.LoginUser = userClient;
                response.ReturnCode = (short)ReturnCode.Success;
            }
        }
    }
}
