using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodCommon;
using Photon.SocketServer;
using GodCommon.Tools;
using GodCommon.Models;
using GodServer.DataBase.Managers;

namespace GodServer.Handlers
{
    public class TaskDBHandler : HandlerBase
    {
        public override OperationCode opCode
        {
            get
            {
                return OperationCode.TaskDB;
            }
        }

        private TaskDBManager manager;

        public TaskDBHandler()
        {
            manager = new TaskDBManager();
        }

        public override void OnHandlerMessage(OperationRequest request, OperationResponse response, ClientPeer peer)
        {
            SubCode subCode = ParameterTool.GetParameter<SubCode>(request.Parameters, ParameterCode.SubCode, false);
            Dictionary<byte, object> parameters = response.Parameters;
            parameters.Add((byte)ParameterCode.SubCode, subCode);
            switch (subCode)
            {
                case SubCode.GetTaskDBList:
                    List<TaskDB> taskDBList = manager.GetTaskDBList(peer.LoginRole);
                    foreach (var taskDBTemp in taskDBList)
                    {
                        taskDBTemp.Role = null;  //防止json解析错误
                    }
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.TaskDBList, taskDBList);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.AddTaskDB:
                    TaskDB taskDBAdd = ParameterTool.GetParameter<TaskDB>(request.Parameters, ParameterCode.TaskDB);
                    taskDBAdd.Role = peer.LoginRole;
                    manager.AddTaskDB(taskDBAdd);
                    taskDBAdd.Role = null;
                    ParameterTool.AddParameter(response.Parameters, ParameterCode.TaskDB, taskDBAdd);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
                case SubCode.UpdateTaskDB:
                    TaskDB taskDBUpdate = ParameterTool.GetParameter<TaskDB>(request.Parameters, ParameterCode.TaskDB);
                    taskDBUpdate.Role = peer.LoginRole;
                    manager.UpdateTaskDB(taskDBUpdate);
                    response.ReturnCode = (short)ReturnCode.Success;
                    break;
            }
        }

    }
}
