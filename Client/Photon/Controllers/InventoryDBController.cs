using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using GodCommon;
using System;
using System.Collections.Generic;
using GodCommon.Tools;
using GodCommon.Models;

public class InventoryDBController : ControllerBase
{
    public override OperationCode OpCode
    {
        get
        {
            return OperationCode.InventoryDB;
        }
    }

    public event OnGetInventoryDBListEvent OnGetInventoryDBList;
    public event OnAddInventoryDBEvent OnAddInventoryDB;
    public event OnChangeEquipmentEvent OnChangeEquipment;
    public event OnUpgradeEquipmentEvent OnUpgradeEquipment;

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        SubCode subCode = ParameterTool.GetParameter<SubCode>(operationResponse.Parameters, ParameterCode.SubCode, false);
        switch (subCode)
        {
            case SubCode.GetInventoryDBList:
                //Debug.Log("SubCode.GetInventoryDBList");
                List<InventoryDB> list = ParameterTool.GetParameter<List<InventoryDB>>(operationResponse.Parameters, ParameterCode.InventoryDBList);
                OnGetInventoryDBList(list);
                break;
            case SubCode.AddInventoryDB:
                //Debug.Log("SubCode.AddInventoryDB");
                if (operationResponse.ReturnCode == (short)ReturnCode.Success)
                {
                    InventoryDB inventoryDB = ParameterTool.GetParameter<InventoryDB>(operationResponse.Parameters, ParameterCode.InventoryDB);
                    OnAddInventoryDB(inventoryDB);
                }
                break;
            case SubCode.ChangeEquipment:
                //Debug.Log("SubCode.ChangeEquipmen");
                if (operationResponse.ReturnCode == (short)ReturnCode.Success)
                {
                    if (OnChangeEquipment != null)
                    {
                        OnChangeEquipment();
                    }
                }
                break;
            case SubCode.UpgradeEquipment:
                //Debug.Log("SubCode.UpgradeEquipment");
                if (operationResponse.ReturnCode == (short)ReturnCode.Success)
                {
                    if (OnUpgradeEquipment != null)
                    {
                        OnUpgradeEquipment();
                    }
                }
                break;
        }
    }

    public void GetInventoryDBList()
    {
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.GetInventoryDBList, new Dictionary<byte, object>());
    }

    public void AddInventoryDB(InventoryDB inventoryDB)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.InventoryDB, inventoryDB);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.AddInventoryDB, parameters);
    }

    public void ChangeEquipment(InventoryDB DBOn, InventoryDB DBOff)
    {
        DBOn.Role = null;
        DBOff.Role = null;
        List<InventoryDB> list = new List<InventoryDB>();
        list.Add(DBOn);
        list.Add(DBOff);
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.InventoryDBList, list);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.ChangeEquipment, parameters);
    }

    public void UpgradeEquipment(InventoryDB inventoryDB)
    {
        inventoryDB.Role = null;
        Role role = PhotonEngine.Instance.role;
        role.User = null;
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        ParameterTool.AddParameter(parameters, ParameterCode.InventoryDB, inventoryDB);
        ParameterTool.AddParameter(parameters, ParameterCode.Role, role);
        PhotonEngine.Instance.SendRequest(OpCode, SubCode.UpgradeEquipment, parameters);
    }
}
