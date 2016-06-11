using GodCommon;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer.Handlers
{
    public abstract class HandlerBase
    {
        public abstract void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer);
        public abstract OperationCode opCode { get; }
    }
}
