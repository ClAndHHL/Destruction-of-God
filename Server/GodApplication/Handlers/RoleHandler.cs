using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using GodCommon;
using GodCommon.Tools;
using GodServer.DataBase.Managers;
using GodCommon.Models;

namespace GodServer.Handlers
{
    public class RoleHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.Role;
            }
        }

        private RoleManager manager;

        public RoleHandler()
        {
            manager = new RoleManager();
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            //根据子操作代码分别进行不同的处理
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            Dictionary<byte, object> parameters = response.Parameters;
            parameters.Add((byte)ParameterCode.SubCode, subCode);
            switch (subCode)
            {
                case SubCode.GetRole:
                    List<Role> roleList = manager.GetRoleListByUser(peer.LoginUser);
                    foreach (Role roleTemp in roleList)
                    {
                        roleTemp.User = null;  //防止json解析错误
                    }
                    ParameterTool.AddParameter(parameters, ParameterCode.RoleList, roleList);
                    break;
                case SubCode.AddRole:
                    Role role = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    role.User = peer.LoginUser;
                    manager.AddRole(role);
                    role.User = null;  //防止json解析错误，实际上role对应的User已经添加到loginUser里了
                    ParameterTool.AddParameter(parameters, ParameterCode.Role, role);
                    break;
                case SubCode.SelectRole:
                    peer.LoginRole = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    break;
                case SubCode.UpdateRole:
                    Role roleUpdate = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    roleUpdate.User = peer.LoginUser;
                    manager.UpdateRole(roleUpdate);  //更新当前角色
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
            }
        }
    }
}
