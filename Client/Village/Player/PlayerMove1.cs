using UnityEngine;
using System.Collections;

public class PlayerMove1 : MonoBehaviour
{
    private NavMeshAgent agent;
    public float speed = 150f;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vel = GetComponent<Rigidbody>().velocity;

        if (Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-h * speed, vel.y, -v * speed);  //保持y轴方向速度不变
            transform.rotation = Quaternion.LookRotation(new Vector3(-h, 0f, -v));
        }
        else if (agent.enabled == false)  //没有自动寻路，速度归零
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
