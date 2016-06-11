using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System;
using GodCommon;
using GodCommon.Models;
using LitJson;

public class ServerController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.ServerList;
        }
    }

    public override void Start()
    {
        base.Start();
        PhotonEngine.Instance.OnConnectedToServer += GetServerList;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        PhotonEngine.Instance.OnConnectedToServer -= GetServerList;
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        Dictionary<byte, object> parameters = operationResponse.Parameters;
        object jsonObject = null;
        parameters.TryGetValue((byte)ParameterCode.ServerList, out jsonObject);
        List<ServerProperty> serverList = JsonMapper.ToObject<List<ServerProperty>>(jsonObject.ToString());  //将json格式转换成string类型

        int index = 0;
        ServerProperty serverDefalut = null;
        GameObject goDefalut = null;
        foreach (ServerProperty sp in serverList)
        {
            string ip = sp.Ip;
            string name = sp.Name;
            int count = sp.Count;
            GameObject go = null;
            if (count > 50)
            {
                go = NGUITools.AddChild(StartMenu.instance.serverGrid.gameObject, StartMenu.instance.serverBtn_red);  //火爆
            }
            else
            {
                go = NGUITools.AddChild(StartMenu.instance.serverGrid.gameObject, StartMenu.instance.serverBtn_green);  //流畅
            }
            ServerInfomation info = go.GetComponent<ServerInfomation>();
            info.serverIp = ip;
            info.serverName = name;
            info.count = count;
            if (index == 0)  //设置默认服务器
            {
                serverDefalut = sp;
                goDefalut = go;
            }
            index++;
        }
        StartMenu.instance.serverLabel_start.text = serverDefalut.Name;
        StartMenu.instance.server_select.transform.Find("Label").GetComponent<UILabel>().text = serverDefalut.Name;
        StartMenu.instance.server_select.GetComponent<UISprite>().spriteName = goDefalut.GetComponent<UISprite>().spriteName;
    }

    public void GetServerList()
    {
        PhotonEngine.Instance.SendRequest(OpCode, new Dictionary<byte, object>());
    }
}
