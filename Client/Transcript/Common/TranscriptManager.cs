using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TranscriptManager : MonoBehaviour
{
    public static TranscriptManager instance;
    public GameObject player;  //表示当前客户端的主角
    private List<GameObject> enemyList = new List<GameObject>();
    private Dictionary<string, GameObject> enemyDict = new Dictionary<string, GameObject>();
    private EnemyController enemyController;
    private List<Enemy> syncEnemyList = new List<Enemy>();  //需要同步敌人移动的集合
    private Boss syncBoss = null;
    private List<Enemy> syncEnemyAnimList = new List<Enemy>();  //需要同步敌人动画的集合

    void Awake()
    {
        instance = this;
        //player = GameObject.FindGameObjectWithTag("me").gameObject;
    }

    // Use this for initialization
    void Start()
    {
        if (GameController.Instance.type == FightType.Team)
        {
            enemyController = GetComponent<EnemyController>();
            enemyController.OnSyncEnemyCreat += OnSyncEnemyCreat;
            enemyController.OnSyncEnemyPosition += OnSyncEnemyPosition;
            enemyController.OnSyncEnemyAnim += OnSyncEnemyAnim;
        }
        if (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("SyncEnemyPosition", 0f, 1f / 30);
            InvokeRepeating("SyncEnemyAnim", 0f, 1f / 30);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        if (GameController.Instance.type == FightType.Team)
        {
            enemyController.OnSyncEnemyCreat -= OnSyncEnemyCreat;
            enemyController.OnSyncEnemyPosition -= OnSyncEnemyPosition;
            enemyController.OnSyncEnemyAnim -= OnSyncEnemyAnim;
        }
    }

    public void AddEnemy(GameObject go)
    {
        enemyList.Add(go);
        string guid = null;
        if (go.GetComponent<Enemy>() != null)
        {
            guid = go.GetComponent<Enemy>().guid;
        }
        else if (go.GetComponent<Boss>() != null)
        {
            guid = go.GetComponent<Boss>().guid;
        }
        enemyDict.Add(guid, go);
    }

    public void RemoveEnemy(GameObject go)
    {
        enemyList.Remove(go);
        string guid = null;
        if (go.GetComponent<Enemy>() != null)
        {
            guid = go.GetComponent<Enemy>().guid;
        }
        else if (go.GetComponent<Boss>() != null)
        {
            guid = go.GetComponent<Boss>().guid;
        }
        enemyDict.Remove(guid);
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    public Dictionary<string, GameObject> GetEnemyDict()
    {
        return enemyDict;
    }

    public void AddSyncEnemy(Enemy enemy)
    {
        syncEnemyList.Add(enemy);
    }

    public void AddSyncBoss(Boss boss)
    {
        syncBoss = boss;
    }

    public void AddSyncAnimEnemy(Enemy enemy)
    {
        syncEnemyAnimList.Add(enemy);
    }

    public void OnSyncEnemyCreat(EnemyCreatModel model)
    {
        foreach (EnemyProperty property in model.list)
        {
            GameObject enemyPrefab = (GameObject)Resources.Load("Enemy/" + property.perfabName);
            GameObject go = (GameObject)Instantiate(enemyPrefab, property.position.ToVector3(), Quaternion.identity);
            enemyList.Add(go);
            enemyDict.Add(property.guid, go);

            Enemy enemy = go.GetComponent<Enemy>();
            if (enemy != null)  //普通敌人
            {
                enemy.guid = property.guid;
                enemy.targetRoleId = property.targetRoleId;
            }
            else  //boss
            {
                Boss boss = go.GetComponent<Boss>();
                boss.guid = property.guid;
                boss.targetRoleId = property.targetRoleId;
            }
        }
    }

    public void SyncEnemyPosition()
    {
        if (syncEnemyList != null || syncBoss != null)
        {
            EnemyPositionModel model = new EnemyPositionModel();
            foreach (Enemy enemy in syncEnemyList)
            {
                if (enemy != null)  //同步期间敌人可能被杀死而出现空指针
                {
                    EnemyPositionProperty property = new EnemyPositionProperty { guid = enemy.guid, position = new Vector3Object(enemy.transform.position), eulerAngles = new Vector3Object(enemy.transform.eulerAngles) };
                    model.list.Add(property);
                }
            }
            if (syncBoss != null)
            {
                EnemyPositionProperty property = new EnemyPositionProperty { guid = syncBoss.guid, position = new Vector3Object(syncBoss.transform.position), eulerAngles = new Vector3Object(syncBoss.transform.eulerAngles) };
                model.list.Add(property);
            }

            enemyController.SyncEnemyPosition(model);
            syncEnemyList.Clear();
            syncBoss = null;
        }
    }

    public void OnSyncEnemyPosition(EnemyPositionModel model)
    {
        foreach (EnemyPositionProperty property in model.list)
        {
            GameObject go;
            bool isSuccess = enemyDict.TryGetValue(property.guid, out go);
            if (isSuccess)
            {
                go.transform.position = property.position.ToVector3();
                go.transform.eulerAngles = property.eulerAngles.ToVector3();
            }
        }
    }

    public void SyncEnemyAnim()
    {
        if (syncEnemyAnimList != null && syncEnemyAnimList.Count > 0)
        {
            EnemyAnimModel model = new EnemyAnimModel();
            foreach (Enemy enemy in syncEnemyAnimList)
            {
                Animation anim = enemy.GetComponent<Animation>();
                EnemyAnimProperty property = new EnemyAnimProperty
                {
                    guid = enemy.guid,
                    isIdle = anim.IsPlaying("idle"),
                    isWalk = anim.IsPlaying("walk"),
                    isAttack = anim.IsPlaying("attack01"),
                    isTakeDamage = anim.IsPlaying("takedamage"),
                    isDie = anim.IsPlaying("die")
                };
                model.list.Add(property);
            }

            enemyController.SyncEnemyAnim(model);
            syncEnemyAnimList.Clear();
        }
    }

    public void OnSyncEnemyAnim(EnemyAnimModel model)
    {
        foreach (EnemyAnimProperty property in model.list)
        {
            GameObject go;
            bool isSuccess = enemyDict.TryGetValue(property.guid, out go);
            if (isSuccess)
            {
                Animation anim = go.GetComponent<Animation>();
                if (property.isIdle)
                {
                    anim.Play("idle");
                }
                if (property.isWalk)
                {
                    anim.Play("walk");
                }
                if (property.isAttack)
                {
                    anim.Play("attack01");
                }
                if (property.isTakeDamage)
                {
                    anim.Play("takedamage");
                }
                if (property.isDie)
                {
                    anim.Play("die");
                }
            }
        }
    }
}
