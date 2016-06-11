using UnityEngine;
using System.Collections;

public class PlayerAutoMove : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public float minDistance = 40f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {
            anim.SetBool("Move", true);  //开启自动寻路则播放动画
            if (agent.remainingDistance != 0 && agent.remainingDistance < minDistance)  //小于40可认为到达终点，判断不等于0是避免Update过快导致直接将agent设置为false
            {
                agent.Stop();
                agent.enabled = false;
                TaskManager.instance.Reach();  //自动显示对话框
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)  //寻路过程中按下方向键，停止寻路
        {
            StopMove();
        }
    }

    public void SetDestination(Vector3 targetPos)  //设置目标，自动寻路
    {
        agent.enabled = true;
        agent.SetDestination(targetPos);
    }

    public void StopMove()
    {
        if (agent.enabled)
        {
            agent.Stop();
            agent.enabled = false;
        }
    }
}
