using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public GameObject[] npcArray;
    public GameObject transcript;

    private Dictionary<int, GameObject> npcDict = new Dictionary<int, GameObject>();

    void Awake()
    {
        instance = this;
        InitNpc();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitNpc()
    {
        foreach (GameObject npc in npcArray)
        {
            int id = int.Parse(npc.name.Substring(0, 4));  //截取前四个字符
            npcDict.Add(id, npc);
        }
    }

    public GameObject GetNpcById(int id)
    {
        GameObject go = null;
        npcDict.TryGetValue(id, out go);
        return go;
    }
}
