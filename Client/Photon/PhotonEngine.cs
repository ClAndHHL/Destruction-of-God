using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System;
using GodCommon;
using GodCommon.Models;
using GodCommon.Tools;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    private static PhotonEngine _instance;
    private PhotonPeer peer;
    public Role role;  //保存当前角色
    public string serverAddress = "127.0.0.1:4530";
    public string applicationName = "GodServer";
    private Dictionary<byte, ControllerBase> controllers = new Dictionary<byte, ControllerBase>();

    public static PhotonEngine Instance
    {
        get
        {
            return _instance;
        }
    }

    public delegate void OnConnectedToServerEvent();
    public event OnConnectedToServerEvent OnConnectedToServer;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);  //不自动销毁
    }

    // Use this for initialization
    void Start()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
        peer.Connect(serverAddress, applicationName);
    }

    // Update is called once per frame
    void Update()
    {
        if (peer != null)
        {
            peer.Service();  //一直向服务端发出请求
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugReturn: " + level + "  " + message);
    }

    public void OnOperationResponse(OperationResponse operationResponse)  //客户端响应操作
    {
        ControllerBase controller;
        controllers.TryGetValue(operationResponse.OperationCode, out controller);  //根据操作码从字典里获得相应的controller
        //Debug.Log("Response:" + operationResponse.OperationCode);
        if (controller != null)
        {
            controller.OnOperationResponse(operationResponse);  //交给相应的controller处理
        }
        else
        {
            Debug.Log("Unknown Response:" + operationResponse.OperationCode);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)  //当连接状态改变时调用
    {
        Debug.Log("StatusCode:" + statusCode);
        switch (statusCode)
        {
            case StatusCode.Connect:
                OnConnectedToServer();
                break;
            default:
                Debug.Log("Failure");
                break;
        }
    }

    public void OnEvent(EventData eventData)
    {
        ControllerBase controller;
        OperationCode opCode = ParameterTool.GetParameter<OperationCode>(eventData.Parameters, ParameterCode.OperationCode, false);
        controllers.TryGetValue((byte)opCode, out controller);
        //Debug.Log("Response:" + opCode);
        if (controller != null)
        {
            controller.OnEventData(eventData);
        }
        else
        {
            Debug.Log("Unknown Event:" + opCode);
        }
    }

    public void SendRequest(OperationCode opCode, Dictionary<byte, object> parameters)  //向服务器发起请求
    {
        peer.OpCustom((byte)opCode, parameters, true);
        //Debug.Log("SendRequest:" + opCode);
    }

    public void SendRequest(OperationCode opCode, SubCode subCode, Dictionary<byte, object> parameters)  //重载
    {
        parameters.Add((byte)ParameterCode.SubCode, subCode);
        peer.OpCustom((byte)opCode, parameters, true);
        //Debug.Log("SendRequest:" + opCode + " _ " + subCode);
    }

    public void RegisterController(OperationCode opCode, ControllerBase controller)  //注册controller
    {
        controllers.Add((byte)opCode, controller);
    }

    public void UnRegisterController(OperationCode opCode)  //注销controller
    {
        controllers.Remove((byte)opCode);
    }
}
