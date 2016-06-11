using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodCommon;
using Photon.SocketServer;
using GodCommon.Tools;

namespace GodServer.Handlers
{
    class EnemyHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.Enemy;
            }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            SubCode subCode = ParameterTool.GetSubCode(request.Parameters);
            switch (subCode)
            {
                case SubCode.SyncEnemyCreat:
                    TransmitRequest(peer, request, subCode);
                    break;
                case SubCode.SyncPositionAndRotation:
                    TransmitRequest(peer, request, subCode);
                    break;
                case SubCode.SyncEnemyAnim:
                    TransmitRequest(peer, request, subCode);
                    break;
            }
        }

        public void TransmitRequest(ClientPeer peer, OperationRequest request, SubCode subCode)  //转发请求
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
