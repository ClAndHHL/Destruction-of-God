using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodCommon;
using Photon.SocketServer;
using GodCommon.Tools;
using GodCommon.Models;
using GodServer.Tools;

namespace GodServer.Handlers
{
    class FightHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.Fight;
            }
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            SubCode subCode = ParameterTool.GetSubCode(request.Parameters);
            ParameterTool.AddSubCode(response.Parameters, subCode);
            switch (subCode)
            {
                case SubCode.TeamFight:
                    if (GodApplication.Instance.peerListForTeam.Count >= 2)  //三个人可以组队
                    {
                        //取得待组队list中的前两个peer和当前的peer组队
                        ClientPeer peer1 = GodApplication.Instance.peerListForTeam[0];
                        ClientPeer peer2 = GodApplication.Instance.peerListForTeam[1];

                        Team team = new Team(peer1, peer2, peer);
                        GodApplication.Instance.peerListForTeam.RemoveRange(0, 2);  //将0 1 2号peer移出待组队集合
                        List<Role> roleList = new List<Role>();
                        foreach (var clientPeer in team.clientPeers)
                        {
                            roleList.Add(clientPeer.LoginRole);
                        }
                        ParameterTool.AddParameter(response.Parameters, ParameterCode.RoleList, roleList);
                        ParameterTool.AddParameter(response.Parameters, ParameterCode.MasterRoleId, team.masterRoleId, false);
                        response.ReturnCode = (short)ReturnCode.HavingTeam;

                        SendMasterIdToPeer(peer1, (OperationCode)request.OperationCode, SubCode.ConfirmTeam, team.masterRoleId, roleList);
                        SendMasterIdToPeer(peer2, (OperationCode)request.OperationCode, SubCode.ConfirmTeam, team.masterRoleId, roleList);
                    }
                    else  //不足三人
                    {
                        //当前服务器可供组队的客户端不足的时候，将自身添加到集合中等待组队
                        GodApplication.Instance.peerListForTeam.Add(peer);
                        response.ReturnCode = (short)ReturnCode.WaitingTeam;
                    }
                    break;
                case SubCode.CancelTeam:
                    GodApplication.Instance.peerListForTeam.Remove(peer);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.SyncPositionAndRotation:
                    object position = null;
                    request.Parameters.TryGetValue((byte)ParameterCode.Position, out position);
                    object eulerAngles = null;
                    request.Parameters.TryGetValue((byte)ParameterCode.EulerAngles, out eulerAngles);
                    foreach (var peerTemp in peer.Team.clientPeers)
                    {
                        if (peerTemp != peer)  //其他客户端需要同步
                        {
                            SendPositionAndRotationToPeer(peerTemp, (OperationCode)request.OperationCode, SubCode.SyncPositionAndRotation, peer.LoginRole.Id, position, eulerAngles);
                        }
                    }
                    break;
                case SubCode.SyncMoveAnimation:
                    foreach (var peerTemp in peer.Team.clientPeers)
                    {
                        if (peerTemp != peer)  //其他客户端需要同步
                        {
                            SendMoveAnimationToPeer(peerTemp, (OperationCode)request.OperationCode, SubCode.SyncAnimation, peer.LoginRole.Id, request.Parameters);
                        }
                    }
                    break;
                case SubCode.SyncAnimation:
                    request.Parameters.Add((byte)ParameterCode.RoleId, peer.LoginRole.Id);
                    RequestTool.TransmitRequest(peer, request, opCode, subCode);
                    break;
                case SubCode.SyncGameState:
                    RequestTool.TransmitRequest(peer, request, opCode, subCode);
                    peer.Team.Dismiss();  //解散队伍
                    break;
            }
        }

        //发送给客户端masterId
        public void SendMasterIdToPeer(ClientPeer peer, OperationCode opCode, SubCode subCode, int masterRoleId, List<Role> roleList)
        {
            EventData data = new EventData();
            data.Parameters = new Dictionary<byte, object>();
            ParameterTool.AddOperationCode(data.Parameters, opCode);
            ParameterTool.AddSubCode(data.Parameters, subCode);
            ParameterTool.AddParameter(data.Parameters, ParameterCode.MasterRoleId, masterRoleId, false);
            ParameterTool.AddParameter(data.Parameters, ParameterCode.RoleList, roleList);
            peer.SendEvent(data, new SendParameters());
        }

        //发送团队角色位置和旋转进行同步
        public void SendPositionAndRotationToPeer(ClientPeer peer, OperationCode opCode, SubCode subCode, int roleId, object position, object eulerAngles)
        {
            EventData data = new EventData();
            data.Parameters = new Dictionary<byte, object>();
            ParameterTool.AddEventToPeer(data.Parameters, opCode, subCode, roleId);
            ParameterTool.AddParameter(data.Parameters, ParameterCode.Position, position.ToString(), false);
            ParameterTool.AddParameter(data.Parameters, ParameterCode.EulerAngles, eulerAngles.ToString(), false);
            peer.SendEvent(data, new SendParameters());
        }

        //发送团队角色动画进行同步
        public void SendMoveAnimationToPeer(ClientPeer peer, OperationCode opCode, SubCode subCode, int roleId, Dictionary<byte, object> parameters)
        {
            EventData data = new EventData();
            data.Parameters = parameters;
            ParameterTool.AddEventToPeer(data.Parameters, opCode, subCode, roleId);
            peer.SendEvent(data, new SendParameters());
        }
    }
}
