using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBullet : MonoBehaviour
{
    public float time = 10f;
    public float speed = 10f;
    public float repeat = 1;  //攻击一次的时间
    public int force = 10000;  //子弹推力
    public int damage = 100;
    public List<GameObject> playerList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Attack", 0, repeat);
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "me")
        {
            if (playerList.Contains(col.gameObject) == false)  //判断角色是否已经存在
            {
                playerList.Add(col.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "me")
        {
            if (playerList.Contains(col.gameObject) == true)  //判断角色是否已经存在
            {
                playerList.Remove(col.gameObject);
            }
        }
    }

    void Attack()
    {
        foreach (GameObject player in playerList)
        {
            player.SendMessage("TakeDamage", damage);
            player.GetComponent<Rigidbody>().AddForce(transform.right * force);
        }
    }
}
