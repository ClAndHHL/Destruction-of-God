using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodServer
{
    public class Team
    {
        public List<ClientPeer> clientPeers = new List<ClientPeer>();
        public int masterRoleId = 0;  //主机角色id

        public Team()
        {

        }

        public Team(ClientPeer peer1, ClientPeer peer2, ClientPeer peer3)  //将三个人存入集合
        {
            clientPeers.Add(peer1);
            clientPeers.Add(peer2);
            clientPeers.Add(peer3);
            peer1.Team = this;
            peer2.Team = this;
            peer3.Team = this;
            masterRoleId = peer3.LoginRole.Id;  //默认角色3为主机角色
        }

        public void Dismiss()  //解散队伍
        {
            masterRoleId = 0;
            foreach (ClientPeer peer in clientPeers)
            {
                peer.Team = null;
            }
            clientPeers = null;
        }
    }
}
