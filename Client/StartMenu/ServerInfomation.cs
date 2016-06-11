using UnityEngine;
using System.Collections;

public class ServerInfomation : MonoBehaviour
{
    public string serverIp;
    public string _serverName;
    public string serverName
    {
        get
        {
            return _serverName;
        }
        set
        {
            _serverName = value;
            transform.Find("Label").GetComponent<UILabel>().text = value;
        }
    }
    public int count;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPress(bool isPress)
    {
        transform.root.SendMessage("OnServerSelector", gameObject);
    }
}
