using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotonHostRuntimeInterfaces;
using GodServer.Handlers;
using ExitGames.Logging;
using GodCommon.Models;

namespace GodServer
{
    public class ClientPeer : PeerBase
    {
        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
        public User LoginUser { get; set; }  //保存登录账户
        public Role LoginRole { get; set; }  //保存登录角色
        public Team Team { get; set; }  //用户的队伍 

        public ClientPeer(IRpcProtocol protocol, IPhotonPeer peer)
            : base(protocol, peer)
        {

        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)  //当玩家断开连接的时候调用
        {
            log.Debug("A client peer is disconnected");
            if (GodApplication.Instance.peerListForTeam.Contains(this))  //队伍中是否包含自身
            {
                GodApplication.Instance.peerListForTeam.Remove(this);  //将自身从队伍中移出
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            HandlerBase handler;
            GodApplication.Instance.handlers.TryGetValue(operationRequest.OperationCode, out handler);
            OperationResponse response = new OperationResponse();
            response.OperationCode = operationRequest.OperationCode;
            response.Parameters = new Dictionary<byte, object>();
            if (handler != null)
            {
                handler.OnHandlerMessage(operationRequest, response, this);
                SendOperationResponse(response, sendParameters);
            }
            else
            {
                log.Debug("Can't find handler from operationCode:" + operationRequest.OperationCode);
            }
        }
    }
}
