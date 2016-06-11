using UnityEngine;
using System.Collections;
using GodCommon;
using ExitGames.Client.Photon;

public abstract class ControllerBase : MonoBehaviour
{
    public abstract OperationCode OpCode { get; }

    // Use this for initialization
    public virtual void Start()
    {
        PhotonEngine.Instance.RegisterController(OpCode, this);  //注册controller
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.UnRegisterController(OpCode);  //注销controller
    }

    public abstract void OnOperationResponse(OperationResponse operationResponse);  //抽象函数，一定要重写

    public virtual void OnEventData(EventData data) { }  //虚函数，不一定要重写 
}
