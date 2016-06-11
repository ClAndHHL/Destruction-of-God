using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using GodServer.DataBase.Managers;
using GodCommon.Models;
using LitJson;
using GodCommon;

namespace GodServer.Handlers
{
    class ServerProperyHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.ServerList;
            }
        }

        private ServerPropertyManager manager;

        public ServerProperyHandler()
        {
            manager = new ServerPropertyManager();
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            List<ServerProperty> list = manager.GetServerList();
            string json = JsonMapper.ToJson(list);
            Dictionary<byte, object> parameters = response.Parameters;
            parameters.Add((byte)ParameterCode.ServerList, json);
            response.ReturnCode = (short)ReturnCode.Success;
        }
    }
}
