using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject[] enemyArray;
    public Transform[] posArray;
    public float time = 0f;  //表示多少秒后开始产生敌人
    public float rate = 10f;  //产生敌人的速率
    private bool isTrigger;
    private EnemyController enemyController;

    // Use this for initialization
    void Start()
    {
        if (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster)
        {
            enemyController = TranscriptManager.instance.GetComponent<EnemyController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (GameController.Instance.type == FightType.Person)  //个人战斗
        {
            if (EnemyNum.instance.enemyNum != 0)  //一定要杀完当前敌人才可到下一房间
            {
                return;
            }
            if (isTrigger == false)  //只在第一次触发，防止多次触发
            {
                GetComponent<BoxCollider>().isTrigger = true;
                GetComponent<MeshRenderer>().enabled = false;
                EnemyNum.instance.enemyNum = 8;
                isTrigger = true;
                StartCoroutine(NewEnemy());
            }
        }
        else if (GameController.Instance.type == FightType.Team) //团队战斗
        {
            if (EnemyNum.instance.enemyNum != 0)  //一定要杀完当前敌人才可到下一房间
            {
                return;
            }
            if (isTrigger == false)
            {
                GetComponent<BoxCollider>().isTrigger = true;
                GetComponent<MeshRenderer>().enabled = false;
                EnemyNum.instance.enemyNum = 8;
                isTrigger = true;
                if (GameController.Instance.isMaster)  //团队战斗只需要master触发
                {
                    StartCoroutine(NewEnemy());
                }
            }
        }
    }

    IEnumerator NewEnemy()
    {
        yield return new WaitForSeconds(time);
        //发送消息，让其他客户端产生相应的敌人
        foreach (GameObject enemy in enemyArray)
        {
            List<EnemyProperty> propertyList = new List<EnemyProperty>();
            foreach (Transform pos in posArray)
            {
                GameObject go = (GameObject)Instantiate(enemy, pos.position, Quaternion.identity);

                string GUID = Guid.NewGuid().ToString();
                int targetRoleId = GameController.Instance.GetRandomRoleId();
                if (go.GetComponent<Enemy>() != null)  //普通敌人
                {
                    Enemy enemytemp = go.GetComponent<Enemy>();
                    enemytemp.guid = GUID;  //为每个生成的敌人生成唯一的GUID
                    enemytemp.targetRoleId = targetRoleId;
                }
                else if (go.GetComponent<Boss>() != null)  //Boss
                {
                    Boss bossTemp = go.GetComponent<Boss>();
                    bossTemp.guid = GUID;
                    bossTemp.targetRoleId = targetRoleId;
                }
                EnemyProperty property = new EnemyProperty()
                {
                    guid = GUID,
                    perfabName = go.name.Substring(0, go.name.LastIndexOf("(")),
                    targetRoleId = targetRoleId,
                    position = new Vector3Object(pos.position)
                };
                //Debug.Log(GUID + "  " + property.perfabName);
                propertyList.Add(property);
                TranscriptManager.instance.AddEnemy(go);  //加入敌人集合
            }

            if (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster)
            {
                EnemyCreatModel model = new EnemyCreatModel() { list = propertyList };
                enemyController.SendCreatEnemy(model);  //发起敌人创建的请求
            }

            yield return new WaitForSeconds(rate);
        }
    }
}
