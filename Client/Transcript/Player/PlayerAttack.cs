using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttackRange
{
    Forward,
    Around
}

public class PlayerAttack : MonoBehaviour
{
    private Dictionary<string, PlayerEffect> mDict = new Dictionary<string, PlayerEffect>();
    public PlayerEffect[] effectArray;
    public float forwardDistance = 100f;  //前面的敌人
    public float aroundDistance = 200f;  //周围的敌人
    private int[] powerArray = new int[] { 100, 200, 300, 400 };
    private int hp_max = 10000;
    private int hp_now = 10000;
    private Animator anim;
    private Transform hpPoint;
    private GameObject textGo;
    private HUDText hudText;
    public bool isDead = false;  //默认未死亡，死亡后设置为true，PlayerMove不再生效
    public PlayerId player;
    private FightController fightController;
    private bool isSyncAnim = false;

    public event OnPlayerHpChangeEvent OnPlayerHpChange;  //血量改变时候调用

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerId>();
        if (GameController.Instance.type == FightType.Team && player.playerId == PhotonEngine.Instance.role.Id)
        {
            fightController = GameController.Instance.GetComponent<FightController>();
            isSyncAnim = true;
        }

        PlayerEffect[] peArray = GetComponentsInChildren<PlayerEffect>();
        foreach (PlayerEffect pe in peArray)
        {
            mDict.Add(pe.gameObject.name, pe);
        }
        foreach (PlayerEffect pe in effectArray)  //面板指定的特效
        {
            mDict.Add(pe.gameObject.name, pe);
        }
        anim = GetComponent<Animator>();
        hpPoint = transform.Find("hpPoint").transform;
        textGo = UIManager.instance.GetHudText(hpPoint);  //添加伤害显示
        hudText = textGo.GetComponent<HUDText>();
        hp_max = PlayerInfomation.instance.Hp;
        hp_now = hp_max;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Attack(string args)
    {
        string[] proArray = args.Split(',');  //分割参数       
        //1 effect name
        string effect = proArray[1];
        ShowEffect(effect);

        //2 sound name
        string sound = proArray[2];
        PlaySound(sound);

        //3 move forward
        float forward = float.Parse(proArray[3]);
        if (forward > 0.1f)
        {
            iTween.MoveBy(gameObject, Vector3.forward * forward, 0.2f);
        }

        //0 basic one two three
        string posType = proArray[0];
        if (posType == "basic")  //普通攻击
        {
            List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Forward);
            foreach (GameObject enemy in mList)  //3 move forward 4 jump height
            {
                enemy.SendMessage("TakeDamage", powerArray[0] + "," + proArray[3] + "," + proArray[4]);  //通知敌人受到伤害
            }
        }
        else if (posType == "one")  //技能一
        {
            List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject enemy in mList)  //3 move forward 4 jump height
            {
                enemy.SendMessage("TakeDamage", powerArray[1] + "," + proArray[3] + "," + proArray[4]);  //通知敌人受到伤害
            }
        }
        else if (posType == "two")  //技能二
        {
            List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Around);
            foreach (GameObject enemy in mList)  //3 move forward 4 jump height
            {
                enemy.SendMessage("TakeDamage", powerArray[2] + "," + proArray[3] + "," + proArray[4]);  //通知敌人受到伤害
            }
        }
        else if (posType == "three")  //技能三
        {
            List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Forward);
            foreach (GameObject enemy in mList)  //3 move forward 4 jump height
            {
                enemy.SendMessage("TakeDamage", powerArray[3] + "," + proArray[3] + "," + proArray[4]);  //通知敌人受到伤害
            }
        }
    }

    void TakeDamage(int damage)  //受到伤害
    {
        if (hp_now == 0)
        {
            return;
        }

        hp_now -= damage;
        if (hp_now < 0)
        {
            hp_now = 0;
        }
        if (hp_now == 0)  //死亡
        {
            Dead();
        }
        if (OnPlayerHpChange != null)
        {
            OnPlayerHpChange(hp_now);  //更新血条显示
        }
        //受到伤害的动画，有一定的概率触发，伤害越大，概率越大
        int random = Random.Range(0, 99);  //伤害超过100一定触发动画
        if (random < damage)
        {
            MessageManager.instance.ShowMessage("啊啊啊~~~", 0.5f);
            anim.SetTrigger("TakeDamage");
            if (isSyncAnim)
            {
                PlayerAnimationModel model = new PlayerAnimationModel() { takedamage = true };
                fightController.SyncPlayerAnimation(model);
            }
        }
        hudText.Add("-" + damage, Color.red, 0.1f);  //更新伤害显示
        BloodScreen.instance.Show();  //屏幕变红        
    }

    void Dead()
    {
        GameOver.instance.OnPlayerDie(PhotonEngine.Instance.role.Id);
        isDead = true;
        anim.SetTrigger("Die");
        if (isSyncAnim)
        {
            PlayerAnimationModel model = new PlayerAnimationModel() { die = true };
            fightController.SyncPlayerAnimation(model);
        }
        MessageManager.instance.ShowMessage("哈哈哈，你逃不出我的魔掌的！！！", 2f);
    }

    void ShowEffect(string effect)
    {
        PlayerEffect pe = null;
        if (mDict.TryGetValue(effect, out pe))
        {
            pe.ShowEffect();
        }
    }

    void PlaySound(string sound)
    {
        SoundManager.instance.PlaySound(sound);
    }

    void ShowEffectHand()  //第三次攻击的时候出现恶魔之手特效
    {
        string effect = "DevilHandMobile";
        PlayerEffect pe = null;
        mDict.TryGetValue(effect, out pe);
        List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Forward);  //forward
        foreach (GameObject enemy in mList)  //在可攻击的敌人出实例化特效
        {
            RaycastHit hit;
            bool isCollider = Physics.Raycast(enemy.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));  //实例化处在地面
            if (isCollider)
            {
                Instantiate(pe, hit.point, Quaternion.identity);
            }
        }
    }

    void ShowEffectBird()  //攻击结束的时候出现火鸟特效
    {
        string effect = "FirePhoenixMobile";
        PlayerEffect pe = null;
        mDict.TryGetValue(effect, out pe);
        List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Around);  //around
        foreach (GameObject enemy in mList)  //在可攻击的敌人出实例化特效
        {
            GameObject go = Instantiate(pe).gameObject;
            go.transform.position = transform.position + Vector3.up;
            go.GetComponent<EffectSettings>().Target = enemy;
        }
    }

    void ShowEffectFire()  //攻击结束的时候出现火焰柱特效
    {
        string effect = "HolyFireStrike";
        PlayerEffect pe = null;
        mDict.TryGetValue(effect, out pe);
        List<GameObject> mList = GetEnemyInAttackRange(AttackRange.Around);  //around
        foreach (GameObject enemy in mList)  //在可攻击的敌人出实例化特效
        {
            RaycastHit hit;
            bool isCollider = Physics.Raycast(enemy.transform.position + Vector3.up, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));  //实例化处在地面
            if (isCollider)
            {
                Instantiate(pe, hit.point, Quaternion.identity);
            }
        }
    }

    List<GameObject> GetEnemyInAttackRange(AttackRange range)
    {
        List<GameObject> mList = new List<GameObject>();

        if (range == AttackRange.Forward)  //得到主角前方的敌人
        {
            foreach (GameObject enemy in TranscriptManager.instance.GetEnemyList())
            {
                Vector3 pos = transform.InverseTransformPoint(enemy.transform.position);  //将敌人坐标转换成主角的局部坐标
                if (pos.z >= -0.5f)  //前方的敌人
                {
                    float distance = Vector3.Distance(Vector3.zero, pos);  //计算主角和敌人的距离
                    if (distance < forwardDistance)
                    {
                        mList.Add(enemy);
                    }
                }
            }
        }
        else  //得到主角攻击范围的敌人
        {
            foreach (GameObject enemy in TranscriptManager.instance.GetEnemyList())
            {
                Vector3 pos = transform.InverseTransformPoint(enemy.transform.position);  //将敌人坐标转换成主角的局部坐标
                float distance = Vector3.Distance(Vector3.zero, pos);  //计算主角和敌人的距离
                if (distance < aroundDistance)
                {
                    mList.Add(enemy);
                }
            }
        }
        return mList;
    }

    public void Hp()
    {
        hp_now = hp_max;
    }
}
