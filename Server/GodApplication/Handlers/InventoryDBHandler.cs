using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodCommon;
using Photon.SocketServer;
using GodCommon.Tools;
using GodServer.DataBase.Managers;
using GodCommon.Models;

namespace GodServer.Handlers
{
    public class InventoryDBHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.InventoryDB;
            }
        }

        private InventoryDBManager manager;

        public InventoryDBHandler()
        {
            manager = new InventoryDBManager();
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            Dictionary<byte, object> parameters = response.Parameters;
            parameters.Add((byte)ParameterCode.SubCode, subCode);
            switch (subCode)
            {
                case SubCode.GetInventoryDBList:
                    List<InventoryDB> listGet = manager.GetInventoryList(peer.LoginRole);
                    foreach (var temp in listGet)
                    {
                        temp.Role = null;  //防止json解析错误
                    }
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.InventoryDBList, listGet);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.AddInventoryDB:
                    InventoryDB inventoryDBAdd = ParameterTool.GetParameter<InventoryDB>(request.Parameters, ParameterCode.InventoryDB);
                    inventoryDBAdd.Role = peer.LoginRole;
                    manager.AddInventoryDB(inventoryDBAdd);
                    inventoryDBAdd.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.InventoryDB, inventoryDBAdd);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.ChangeEquipment:
                    List<InventoryDB> listChange = ParameterTool.GetParameter<List<InventoryDB>>(request.Parameters, ParameterCode.InventoryDBList);
                    foreach (var temp in listChange)
                    {
                        temp.Role = peer.LoginRole;
                    }
                    manager.ChangeEquipment(listChange);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.UpgradeEquipment:
                    InventoryDB inventoryDBUpgrade = ParameterTool.GetParameter<InventoryDB>(request.Parameters, ParameterCode.InventoryDB);
                    Role role = ParameterTool.GetParameter<Role>(request.Parameters, ParameterCode.Role);
                    role.User = peer.LoginUser;
                    peer.LoginRole = role;
                    inventoryDBUpgrade.Role = role;
                    manager.UpgradeEquipment(inventoryDBUpgrade, role);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
            }
        }
    }
}
