using UnityEngine;
using System.Collections;

public class PlayerAnimation1 : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 1f)
        {
            anim.SetBool("Move", true);  //开始移动
        }
        else
        {
            anim.SetBool("Move", false);  //保持站立
        }
    }
}
