using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GodCommon.Models;

public enum FightType
{
    Person,
    Team,
    None
}

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }
    private FightController fightController;
    Transform playerPos;
    string playerPrefab;
    public FightType type = FightType.None;
    public int transcriptId = -1;
    public bool isMaster = false;
    public List<Role> roleList = new List<Role>();
    public HashSet<int> roleDieSet = new HashSet<int>();  //存储所以已经死亡的角色id
    public Dictionary<int, GameObject> playerDict = new Dictionary<int, GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);  //不自动销毁，保存到下一个场景
        instance = this;
        playerPos = GameObject.Find("player_pos").transform;
        if (PhotonEngine.Instance.role.IsMan)
        {
            playerPrefab = "man_player";
        }
        else
        {
            playerPrefab = "girl_player";
        }
        GameObject go = (GameObject)Instantiate(Resources.Load("Player_village/" + playerPrefab));  //加载角色
        go.transform.position = playerPos.position;
        fightController = GetComponent<FightController>();
        fightController.OnSyncPositionAndRotation += OnSyncPositionAndRotation;
        fightController.OnSyncMoveAnimation += OnSyncMoveAnimation;
        fightController.OnSyncPlayerAnimation += OnSyncPlayerAnimation;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Destroy()
    {
        fightController.OnSyncPositionAndRotation -= OnSyncPositionAndRotation;
        fightController.OnSyncMoveAnimation -= OnSyncMoveAnimation;
        fightController.OnSyncPlayerAnimation -= OnSyncPlayerAnimation;
    }

    public static int ExpByLevel(int level)
    {
        //1 - 0
        //2 - 0 + 100
        //3 - 0 + 100 + 110
        //4 - 0 + 100 + 110 + 120
        //.
        //.
        //.
        //n - 0 + 100 +......+ (100 + 10(n - 2))
        return 5 * level * level + 85 * level - 90;
    }

    public int GetRandomRoleId()  //随机获得角色id
    {
        if (type == FightType.Team)
        {
            int index = Random.Range(0, roleList.Count);
            return roleList[index].Id;
        }
        else
        {
            return PhotonEngine.Instance.role.Id;
        }
    }

    public GameObject GetPlayerById(int roleId)
    {
        if (type == FightType.Team)
        {
            GameObject go = null;
            playerDict.TryGetValue(roleId, out go);
            return go;
        }
        else
        {
            return TranscriptManager.instance.player;
        }
    }

    public void OnSyncPositionAndRotation(int roleId, Vector3 position, Vector3 eulerAngles)  //最终同步团队角色
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleId, out go);
        if (isHave)
        {
            go.GetComponent<PlayerMove2>().SetPositionAndRotation(position, eulerAngles);
        }
        else
        {
            Debug.Log("Failed : SyncPositionAndRotation");
        }
    }

    public void OnSyncMoveAnimation(int roleId, PlayerMoveAnimationModel model)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleId, out go);
        if (isHave)
        {
            go.GetComponent<PlayerMove2>().SetMoveAnimation(model);
        }
        else
        {
            Debug.Log("Failed : SyncMoveAnimation");
        }
    }

    public void OnSyncPlayerAnimation(int roleId, PlayerAnimationModel model)
    {
        GameObject go = null;
        bool isHave = playerDict.TryGetValue(roleId, out go);
        if (isHave)
        {
            go.GetComponent<PlayerAnimation2>().SyncPlayerAnimation(model);
            if (model.die)
            {
                GameOver.instance.OnPlayerDie(roleId);
            }
        }
        else
        {
            Debug.Log("Failed : SyncPlayerAnimation");
        }
    }
}
