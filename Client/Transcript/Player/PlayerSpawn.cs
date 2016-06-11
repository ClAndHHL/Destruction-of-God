using UnityEngine;
using System.Collections;
using GodCommon.Models;

public class PlayerSpawn : MonoBehaviour
{
    public Transform[] posArray;
    public string playerPrefab;

    void Awake()
    {
        SpawnPlayer();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnPlayer()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInfomation>().isFighting = true;
        if (GameController.Instance.type == FightType.Person)  //个人战斗
        {
            MessageManager.instance.ShowMessage("勇士你来了！", 5f);
            if (PhotonEngine.Instance.role.IsMan)
            {
                playerPrefab = "man_transcript";
            }
            else
            {
                playerPrefab = "girl_transcript";
            }
            GameObject go = (GameObject)Instantiate(Resources.Load("Player_transcript/" + playerPrefab), posArray[0].position, Quaternion.identity);  //加载角色
            //go.transform.position = posArray[0].position;
            TranscriptManager.instance.player = go;
            go.GetComponent<PlayerId>().playerId = PhotonEngine.Instance.role.Id;
        }
        else if (GameController.Instance.type == FightType.Team)  //团队战斗
        {
            MessageManager.instance.ShowMessage("勇敢的少年，你们来了！", 5f);
            for (int i = 0; i < 3; i++)
            {
                Role role = GameController.Instance.roleList[i];
                if (PhotonEngine.Instance.role.IsMan)
                {
                    playerPrefab = "man_transcript";
                }
                else
                {
                    playerPrefab = "girl_transcript";
                }
                GameObject go = (GameObject)Instantiate(Resources.Load("Player_transcript/" + playerPrefab), posArray[i].position, Quaternion.identity);  //加载角色
                GameController.Instance.playerDict.Add(role.Id, go);
                go.GetComponent<PlayerId>().playerId = role.Id;
                if (role.Id == PhotonEngine.Instance.role.Id)
                {
                    //当前创建的角色是当前客户端控制的
                    TranscriptManager.instance.player = go;  //将当前角色赋值给manager
                }
                else
                {
                    //当前创建的角色是其他客户端控制的
                    go.GetComponent<PlayerMove2>().isCanMove = false;  //不能控制移动其他客户端的角色
                }
            }
        }
    }
}
