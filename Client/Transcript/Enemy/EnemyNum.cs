using UnityEngine;
using System.Collections;

public class EnemyNum : MonoBehaviour
{
    public static EnemyNum instance;
    public int enemyNum = 0;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
