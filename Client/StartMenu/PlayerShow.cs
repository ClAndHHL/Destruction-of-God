using UnityEngine;
using System.Collections;

public class PlayerShow : MonoBehaviour
{
    private bool isMan = true;  //默认选择男性角色
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
        if (!isPress)  //鼠标抬起
        {
            if (gameObject.name.IndexOf("man") >= 0)  //选择了男性角色
            {
                isMan = true;
            }
            else
            {
                isMan = false;
            }
            StartMenu.instance.OnPlayerClick(transform.parent.gameObject, isMan);
        }
    }
}
