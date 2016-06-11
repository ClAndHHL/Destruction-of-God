using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using GodCommon.Models;
using GodCommon.Tools;

public class TaskDBController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.TaskDB;
        }
    }

    public event OnGetTaskDBListEvent OnGetTaskDBList;
    public event OnAddTaskDBEvent OnAddTaskDB;
    public event OnUpdateTaskDBEvent OnUpdateTaskDB;

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        SubCode subCode = ParameterTool.GetParameter<SubCode>(operationResponse.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetTaskDBList:
                //Debug.Log("SubCode.GetTaskDBList");
                List<TaskDB> list = ParameterTool.GetParameter<List<TaskDB>>(operationResponse.Parameters, ParameterCode.TaskDBList);
                OnGetTaskDBList(list);  //返回任务列表
                break;
            case SubCode.AddTaskDB:
                //Debug.Log("SubCode.AddTaskDB");
                TaskDB taskDB = ParameterTool.GetParameter<TaskDB>(operationResponse.Parameters, ParameterCode.TaskDB);
                OnAddTaskDB(taskDB);  //返回所添加的任务
                break;
            case SubCode.UpdateTaskDB:
                //Debug.Log("SubCode.UpdateTaskDB");
                OnUpdateTaskDB();
                break;
        }
    }

    public void GetTaskDBList()  //向服务器获得任务列表
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.GetTaskDBList, new Dictionary<byte, object>());
    }

    public void AddTaskDB(TaskDB taskDB)  //向服务器添加任务
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.TaskDB, taskDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.AddTaskDB, parameters);
    }

    public void UpdateTaskDB(TaskDB taskDB)  //向服务器更新任务进度
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.TaskDB, taskDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpdateTaskDB, parameters);
    }
}
