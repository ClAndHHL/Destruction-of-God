using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject hpBar;
    public GameObject hudText;

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

    public GameObject GetHpBar(Transform target)
    {
        GameObject go = NGUITools.AddChild(gameObject, hpBar);
        go.GetComponent<UIFollowTarget>().target = target;
        return go;
    }

    public GameObject GetHudText(Transform target)
    {
        GameObject go = NGUITools.AddChild(gameObject, hudText);
        go.GetComponent<UIFollowTarget>().target = target;
        return go;
    }
}
