using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public string guid = "";
    public GameObject bloodEffect;
    public int hp_max = 2000;  //最大血量
    private int hp_now = 2000;  //当前血量
    public float viewAngle = 20f;  //攻击视野范围
    public float attackDistance = 20f;  //敌人可以进行攻击的距离
    public float moveDistance = 100f;  //敌人可以朝向主角移动的距离
    public float speed = 4f;
    public int[] powerArray = { 100, 200, 400 };  //敌人攻击力
    public GameObject[] effectArray;
    public float smooth = 2f;  //转动时的缓动效果
    public int attackRate = 2;  //攻击速率，表示多少秒攻击一次  
    private float attackTimer = 0f;
    private float distance = 0f;  //主角和敌人的距离
    private Transform player;
    private bool isAttacking = false;  //表示是否敌人正在攻击
    private int attackIndex = 0;  //攻击方式轮换索引值 

    public GameObject bullet;
    private Transform hpPoint;
    private GameObject textGo;
    private HUDText hudText;
    private Renderer renderer;

    public int targetRoleId = -1;
    private GameObject targetGo;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 lastEulerAngles = Vector3.zero;
    private bool lastIsIdle = true;
    private bool lastIsWalk = false;
    private bool lastIsAttack01 = false;
    private bool lastIsAttack02 = false;
    private bool lastIsAttack03 = false;
    private bool lastIsTakeDamage = false;
    private bool lastIsDie = false;
    private BossController bossController;

    // Use this for initialization
    void Start()
    {
        targetGo = GameController.Instance.GetPlayerById(targetRoleId);
        player = targetGo.transform;
        hp_now = hp_max;
        hpPoint = transform.Find("hpPoint").transform;
        renderer = transform.Find("Object01").GetComponent<Renderer>();
        textGo = UIManager.instance.GetHudText(hpPoint);  //添加伤害显示
        hudText = textGo.GetComponent<HUDText>();
        BossHpBar.instance.ShowHp(hp_max);  //添加血条显示

        if (GameController.Instance.type == FightType.Team)
        {
            bossController = TranscriptManager.instance.GetComponent<BossController>();
            bossController.OnSyncBossAnim += OnSyncBossAnim;
        }

        if (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster)
        {
            InvokeRepeating("CheckPositionAndRotation", 0f, 1f / 30);  //只有团队战斗下的master才可以同步boss
            InvokeRepeating("CheckAnimation", 0f, 1f / 30);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hp_now <= 0)
        {
            return;
        }

        renderer.material.color = Color.Lerp(renderer.material.color, Color.white, Time.deltaTime);  //让受伤后的boss从红色慢慢变回白色

        if (GameController.Instance.type == FightType.Person || (GameController.Instance.type == FightType.Team && GameController.Instance.isMaster))
        {
            if (isAttacking)  //如果正在攻击就不进行其他动画
            {
                return;
            }
            Vector3 playerPos = player.position;
            playerPos.y = transform.position.y;  //保证y轴一致
            float angle = Vector3.Angle(playerPos - transform.position, transform.forward);  //计算敌人和主角的角度
            if (angle <= viewAngle / 2)  //攻击视野之内
            {
                float distance = Vector3.Distance(player.position, transform.position);
                if (distance <= attackDistance)  //攻击范围之内
                {
                    if (isAttacking == false)
                    {
                        GetComponent<Animation>().CrossFade("idle");  //没有攻击，处于idle状态          
                        attackTimer += Time.deltaTime;  //没有在攻击，开始计时
                        if (attackTimer >= attackRate)  //开始攻击
                        {
                            Attack();  //按顺序释放3种技能
                        }
                    }
                }
                else if (distance <= moveDistance)  //移动范围之内
                {
                    //行走到主角身边
                    GetComponent<Animation>().Play("walk");  //播放行走的动画
                    GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * speed * Time.deltaTime);  //利用刚体移动
                }
            }
            else  //攻击视野之外，进行转向
            {
                GetComponent<Animation>().Play("walk");  //播放行走的动画
                Quaternion targetRotation = Quaternion.LookRotation(playerPos - transform.position);  //目标朝向主角
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smooth * Time.deltaTime);
            }
        }
    }

    void OnDestroy()
    {
        if (GameController.Instance.type == FightType.Team)
        {
            bossController.OnSyncBossAnim -= OnSyncBossAnim;
        }
    }

    void Attack()
    {
        isAttacking = true;  //此时计时器不会计时
        attackIndex++;
        if (attackIndex > 3)
        {
            attackIndex = 1;
        }
        GetComponent<Animation>().Play("attack0" + attackIndex);
        //if (attackIndex == 1)
        //{
        //    GetComponent<Animation>().Play("attack01");
        //}
        //else  if(attackIndex == 2)
        //{
        //    GetComponent<Animation>().Play("attack02");
        //}
        //else if(attackIndex == 3)
        //{
        //    GetComponent<Animation>().Play("attack03");
        //}
    }

    void BackToNormal()  //在动画快结束时自动调用
    {
        isAttacking = false;  //此时计算器重新计时
        attackTimer = 0f;
    }

    void PlayAttack01()
    {
        effectArray[0].gameObject.SetActive(true);  //显示技能一特效
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", powerArray[0]);
        }
    }

    void PlayAttack02()
    {
        effectArray[1].gameObject.SetActive(true);  //显示技能三特效
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", powerArray[1]);
        }
    }

    void PlayAttack03()
    {
        Transform bulletPos = transform.Find("bullet").transform;
        //GameObject go = NGUITools.AddChild(gameObject, bullet);
        //go.transform.localPosition = bullet.transform.localPosition;
        //go.transform.localRotation = bullet.transform.localRotation;
        //go.transform.localScale = bullet.transform.localScale;
        GameObject go = (GameObject)Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
        //go.transform.localPosition = bullet.transform.localPosition;
        //go.transform.localRotation = bullet.transform.localRotation;
        go.transform.localScale = bullet.transform.localScale;
        if (distance < attackDistance)
        {
            player.SendMessage("TakeDamage", powerArray[2]);
        }
    }

    void TakeDamage(string args)  //受到伤害
    {
        if (hp_now == 0)  //死亡
        {
            return;
        }
        Combo.instance.ShowCombo();  //显示连击数
        //受到伤害的动画
        if (Random.Range(0, 10) >= 8)  //概率为8 9的时候触发受伤效果
        {
            GetComponent<Animation>().Play("takedamage");
        }
        string[] proArray = args.Split(',');
        //0 伤害值
        int damage = int.Parse(proArray[0]);
        hp_now -= damage;
        hudText.Add("-" + damage, Color.red, 0.1f);  //更新伤害显示
        BossHpBar.instance.UpdateHp(hp_now);  //更新血条显示  
        if (hp_now <= 0)
        {
            hp_now = 0;
        }
        if (hp_now == 0)  //死亡
        {
            Dead();
        }
        if (Random.Range(0, 10) >= 7)  //概率为7 8 9的时候触发后退和浮空效果
        {
            //1 后退的距离
            //2 浮空的高度 
            float back = float.Parse(proArray[1]);  //敌人后退的方向为主角的前方向
            float height = float.Parse(proArray[2]);
            Vector3 pos = transform.InverseTransformDirection(TranscriptManager.instance.player.transform.forward);  //将主角前方向坐标转换为敌人的局部坐标 
            iTween.MoveBy(gameObject, pos * back + Vector3.up * height, 0.2f);  //后退和浮空
        }
        renderer.material.color = Color.red;

    }

    void Dead()  //两种死亡方式，概率产生
    {
        TranscriptManager.instance.RemoveEnemy(gameObject);
        GetComponent<Animation>().Play("die");
        BossHpBar.instance.HideHp();
        GameOver.instance.OnBossDie();
        Destroy(textGo, 3f);
        Destroy(gameObject, 3f);
    }

    public void CheckPositionAndRotation()  //检查位置和旋转的变化
    {
        Vector3 position = transform.position;
        Vector3 eulerAngles = transform.eulerAngles;
        if (position.x != lastPosition.x || position.y != lastPosition.y || position.z != lastPosition.z ||
            eulerAngles.x != lastEulerAngles.x || eulerAngles.y != lastEulerAngles.y || eulerAngles.z != lastEulerAngles.z)
        {
            TranscriptManager.instance.AddSyncBoss(this);
            lastPosition = position;
            lastEulerAngles = eulerAngles;
        }
    }

    public void CheckAnimation()  //检查动画的播放
    {
        Animation anim = GetComponent<Animation>();
        if (lastIsIdle != anim.IsPlaying("idle") || lastIsWalk != anim.IsPlaying("walk") || lastIsAttack01 != anim.IsPlaying("attack01") ||
            lastIsAttack02 != anim.IsPlaying("attack02") || lastIsAttack03 != anim.IsPlaying("attack03") ||
            lastIsTakeDamage != anim.IsPlaying("takedamage") || lastIsDie != anim.IsPlaying("die"))
        {
            BossAnimModel model = new BossAnimModel()
            {
                idle = anim.IsPlaying("idle"),
                walk = anim.IsPlaying("walk"),
                attack01 = anim.IsPlaying("attack01"),
                attack02 = anim.IsPlaying("attack02"),
                attack03 = anim.IsPlaying("attack03"),
                takeDamage = anim.IsPlaying("takedamage"),
                die = anim.IsPlaying("die")
            };
            bossController.SyncBossAnim(model);  //同步boss动画
            lastIsIdle = anim.IsPlaying("idle");
            lastIsWalk = anim.IsPlaying("walk");
            lastIsAttack01 = anim.IsPlaying("attack01");
            lastIsAttack02 = anim.IsPlaying("attack02");
            lastIsAttack03 = anim.IsPlaying("attack03");
            lastIsTakeDamage = anim.IsPlaying("takedamage");
            lastIsDie = anim.IsPlaying("die");
        }
    }

    public void OnSyncBossAnim(BossAnimModel model)
    {
        Animation anim = GetComponent<Animation>();
        if (model.idle)
        {
            anim.Play("idle");
        }
        if (model.walk)
        {
            anim.Play("walk");
        }
        if (model.attack01)
        {
            anim.Play("attack01");
        }
        if (model.attack02)
        {
            anim.Play("attack02");
        }
        if (model.attack03)
        {
            anim.Play("attack03");
        }
        if (model.takeDamage)
        {
            anim.Play("takedamage");
        }
        if (model.die)
        {
            anim.Play("die");
        }
    }
}