using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodCommon;
using Photon.SocketServer;
using GodCommon.Tools;
using GodServer.Tools;

namespace GodServer.Handlers
{
    public class BossHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.Boss;
            }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            SubCode subCode = ParameterTool.GetSubCode(request.Parameters);
            switch (subCode)
            {
                case SubCode.SyncBossAnim:
                    RequestTool.TransmitRequest(peer, request, opCode, subCode);
                    break;
            }
        }
    }
}
