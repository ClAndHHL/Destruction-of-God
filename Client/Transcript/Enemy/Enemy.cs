using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public string guid = "";  //GUID是每个敌人的唯一标识
    public GameObject bloodEffect;
    public int hp_max = 200;  //最大血量
    private int hp_now = 200;  //当前血量
    public float attackDistance = 10f;  //敌人可以进行攻击的距离
    public float moveDistance = 50f;  //敌人可以朝向主角移动的距离
    public float speed = 2f;
    public int power = 20;  //敌人攻击力
    public int attackRate = 2;  //攻击速率，表示多少秒攻击一次
    private float attackTimer = 0f;
    private float distance = 0f;  //主角和敌人的距离

    private Transform bloodPoint;
    private Transform hpPoint;
    private GameObject hpGo;
    private UISlider hpBar;
    private GameObject textGo;
    private HUDText hudText;
    private CharacterController cc;

    public int targetRoleId = -1;  //表示敌人要攻击的目标id
    private GameObject targetGo;  //表示敌人要攻击的游戏物体
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;
    private bool lastIsIdle = true;
    private bool lastIsWalk = false;
    private bool lastIsAttack = false;
    private bool lastIsTakeDamage = false;
    private bool lastIsDie = false;

    // Use this for initialization
    void Start()
    {
        hp_now = hp_max;
        targetGo = GameController.Instance.GetPlayerById(targetRoleId);
        bloodPoint = transform.Find("bloodPoint").transform;
        hpPoint = transform.Find("hpPoint").transform;
        cc = GetComponent<CharacterController>();
        InvokeRepeating("CalcDistance", 0, 0.1f);  //计算距离

        hpGo = UIManager.instance.GetHpBar(hpPoint);  //添加血条显示
        hpBar = hpGo.GetComponent<UISlider>();
        textGo = UIManager.instance.GetHudText(hpPoint);  //添加伤害显示
        hudText = textGo.GetComponent<HUDText>();

        if (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("CheckPositionAndRotation", 0f, 1f / 30);  //只有团队战斗下的master才可以同步敌人
            InvokeRepeating("CheckAnimation", 0f, 1f / 30);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hp_now <= 0)  //死亡
        {
            return;
        }

        if (GameController.Instance.type == FightType.Person || (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster))
        {
            //敌人由master控制移动
            if (distance <= attackDistance)  //在攻击距离之内
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackRate)  //可以进行攻击则播放attack动画
                {
                    Transform player = targetGo.transform;
                    Vector3 targetPos = player.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);
                    GetComponent<Animation>().Play("attack01");
                    attackTimer = 0f;
                }
                if (GetComponent<Animation>().IsPlaying("attack01") == false)  //没有正在攻击则播放idle动画
                {
                    GetComponent<Animation>().CrossFade("idle");
                }
            }
            else if (distance <= moveDistance)  //在移动距离之内
            {
                GetComponent<Animation>().Play("walk");
                AutoMove();  //敌人朝向主角移动
            }
            else  //随机巡逻
            {
                GetComponent<Animation>().Play("walk");
            }
        }
    }

    void CalcDistance()
    {
        Transform player = targetGo.transform;
        distance = Vector3.Distance(player.position, transform.position);
    }

    void TakeDamage(string args)  //受到伤害
    {
        if (hp_now == 0)  //死亡
        {
            return;
        }
        Combo.instance.ShowCombo();  //显示连击数
        //受到伤害的动画
        GetComponent<Animation>().Play("takedamage");
        string[] proArray = args.Split(',');
        //0 伤害值
        int damage = int.Parse(proArray[0]);
        hp_now -= damage;
        hpBar.value = (float)hp_now / hp_max;  //更新血条显示
        hudText.Add("-" + damage, Color.red, 0.1f);  //更新伤害显示
        if (hp_now < 0)
        {
            hp_now = 0;
        }
        if (hp_now == 0)  //死亡
        {
            Dead();
        }
        //1 后退的距离
        //2 浮空的高度
        float back = float.Parse(proArray[1]);  //敌人后退的方向为主角的前方向
        float height = float.Parse(proArray[2]);
        Vector3 pos = transform.InverseTransformDirection(targetGo.transform.forward);  //将主角前方向坐标转换为敌人的局部坐标 
        iTween.MoveBy(gameObject, pos * back + Vector3.up * height, 0.2f);  //后退和浮空
        //出血的特效
        Instantiate(bloodEffect, bloodPoint.position, Quaternion.identity);
    }

    void Attack()  //用来判断是否成功伤害到主角
    {
        Transform player = targetGo.transform;
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", power);
        }
    }

    void AutoMove()
    {
        Transform player = targetGo.transform;
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
        cc.SimpleMove(transform.forward * speed);
    }

    void Dead()  //两种死亡方式，概率产生
    {
        TranscriptManager.instance.RemoveEnemy(gameObject);
        GetComponent<CharacterController>().enabled = false;
        int random = Random.Range(0, 10);
        if (random > 2)
        {
            //播放死亡动画 3456789
            GetComponent<Animation>().Play("die");
            iTween.MoveBy(gameObject, -6.0f * Vector3.up, 3f);
        }
        else
        {
            //使用破碎效果 012
            GetComponentInChildren<MeshExploder>().Explode();
        }
        Destroy(hpGo, 3f);
        Destroy(textGo, 3f);
        Destroy(gameObject, 3f);
        EnemyNum.instance.enemyNum--;
    }

    public void CheckPositionAndRotation()  //检查位置和旋转的变化
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y || eulerAngles.z != lastEulerAngles.z)
        {
            TranscriptManager.instance.AddSyncEnemy(this);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    public void CheckAnimation()  //检查动画的播放
    {
        Animation anim = GetComponent<Animation>();
        if (lastIsIdle != anim.IsPlaying("idle") || lastIsWalk != anim.IsPlaying("walk") || lastIsAttack != anim.IsPlaying("attack01") ||
            lastIsTakeDamage != anim.IsPlaying("takedamage") || lastIsDie != anim.IsPlaying("die"))
        {
            TranscriptManager.instance.AddSyncAnimEnemy(this);  //传递到manager管理同步
            lastIsIdle = anim.IsPlaying("idle");
            lastIsWalk = anim.IsPlaying("walk");
            lastIsAttack = anim.IsPlaying("attack01");
            lastIsTakeDamage = anim.IsPlaying("takedamage");
            lastIsDie = anim.IsPlaying("die");
        }
    }
}
