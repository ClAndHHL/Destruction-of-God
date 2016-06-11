using UnityEngine;
using System.Collections;
using System;

public class PlayerMove2 : MonoBehaviour
{
    private FightController fightController;
    private Animator anim;
    public float speed = 20f;
    public float downSpeed = -5f;
    private PlayerAttack playerAttack;
    public bool isCanMove = true;  //表示是否可以控制角色移动
    public bool isMove = false;  //表示是否正在移动
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;
    private DateTime lastUpdateTime = DateTime.Now;  //最后状态更新时间

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Use this for initialization
    void Start()
    {
        if (GameController.Instance.type == FightType.Team && isCanMove)  //团队战斗才需要同步
        {
            fightController = GameController.Instance.GetComponent<FightController>();
            InvokeRepeating("SyncPositionAndRotation", 0f, 1f / 30);  //每秒30次调用
            InvokeRepeating("SyncMoveAnimation", 0f, 1f / 30);  //每秒30次调用
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove == false)  //当前角色不是客户端控制的
        {
            return;
        }

        if (playerAttack.isDead == true)  //死亡后不允许移动
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
        {
            if (anim.GetCurrentAnimatorStateInfo(1).IsName("Empty State"))  //没有释放技能的时候才可以行走
            {
                anim.SetBool("Move", true);
                GetComponent<Rigidbody>().velocity = new Vector3(h * speed, downSpeed, v * speed);  //保持y轴方向速度不变
                transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
            }
            else
            {
                anim.SetBool("Move", false);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else
        {
            anim.SetBool("Move", false);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void SyncPositionAndRotation()  //同步团队角色位置和旋转
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z
            || eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y || eulerAngles.z != lastEulerAngles.z)
        {
            fightController.SyncPositionAndRotation(position, eulerAngles);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    public void SetPositionAndRotation(Vector3 position, Vector3 eulerAngles)
    {
        transform.position = position;
        transform.eulerAngles = eulerAngles;
    }

    void SyncMoveAnimation()  //同步移动的动画
    {
        if (isMove != anim.GetBool("Move"))  //当前状态发生了改变，需要同步
        {
            PlayerMoveAnimationModel model = new PlayerMoveAnimationModel() { IsMove = anim.GetBool("Move") };  //应该改为当前最新状态
            model.SetTime(DateTime.Now);
            fightController.SyncMoveAnimation(model);
            isMove = anim.GetBool("Move");
        }
    }

    public void SetMoveAnimation(PlayerMoveAnimationModel model)
    {
        DateTime dateTime = model.GetTime();
        if (dateTime > lastUpdateTime)  //状态是最新的，需要更新
        {
            anim.SetBool("Move", model.IsMove);
            lastUpdateTime = dateTime;
        }
    }
}
