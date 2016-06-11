using UnityEngine;
using System.Collections;

public class TranscriptItem : MonoBehaviour
{
    public int id;
    public int levelNeed;
    public int eneryNeed;
    public string sceneName;
    public string describe = "这里是一个恐怖的地下城，勇士，你敢来吗？！";

    void Awake()
    {
        eneryNeed = levelNeed * 2;  //花费体力为进入等级的两倍
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        transform.parent.SendMessage("OnItemBtnClick", this);
    }
}
