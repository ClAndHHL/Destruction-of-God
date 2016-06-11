using GodServer.DataBase.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodCommon;
using Photon.SocketServer;
using GodCommon.Tools;
using GodCommon.Models;

namespace GodServer.Handlers
{
    public class SkillDBHandler:HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.SkillDB;
            }
        }

        private SkillDBManager manager;

        public SkillDBHandler()
        {
            manager = new SkillDBManager();
        }       

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            SubCode subCode = ParameterTool.GetSubCode(request.Parameters);
            ParameterTool.AddSubCode(response.Parameters, subCode);
            switch (subCode)
            {
                case SubCode.GetSkillDBList:
                    List<SkillDB> listGet = manager.GetSkillDBList(peer.LoginRole);
                    foreach(var temp in listGet)
                    {
                        temp.Role = null;
                    }
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.SkillDBList, listGet);
                    break;
                case SubCode.AddSkillDB:
                    SkillDB skillAdd = ParameterTool.GetParameter<SkillDB>(request.Parameters, ParameterCode.SkillDB);
                    Role roleAdd = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    roleAdd.User = peer.LoginUser;
                    peer.LoginRole = roleAdd;
                    skillAdd.Role = roleAdd;
                    manager.AddSkill(skillAdd, roleAdd);
                    skillAdd.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.SkillDB, skillAdd);
                    break;
                case SubCode.UpgradeSkillDB:
                    SkillDB skillUpgrade = ParameterTool.GetParameter<SkillDB>(request.Parameters, ParameterCode.SkillDB);
                    Role roleUpgrade = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    roleUpgrade.User = peer.LoginUser;
                    peer.LoginRole = roleUpgrade;
                    skillUpgrade.Role = roleUpgrade;
                    manager.UpgradeSkill(skillUpgrade, roleUpgrade);
                    skillUpgrade.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.SkillDB, skillUpgrade);
                    break;
            }
        }
    }
}
