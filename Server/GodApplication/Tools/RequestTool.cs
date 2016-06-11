using GodCommon;
using GodCommon.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.Tools
{
    public class RequestTool
    {
        public static void TransmitRequest(ClientPeer peer, OperationRequest request, OperationCode opCode, SubCode subCode)  //转发请求
        {
            foreach (ClientPeer temp in peer.Team.clientPeers)
            {
                if (temp != peer)  //说明这个temp是其他客户端，需要同步
                {
                    EventData data = new EventData();
                    data.Parameters = request.Parameters;
                    ParameterTool.AddEventToPeer(data.Parameters, opCode, subCode, temp.LoginRole.Id);
                    temp.SendEvent(data, new SendParameters());
                }
            }
        }
    }
}
