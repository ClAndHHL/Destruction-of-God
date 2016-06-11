using UnityEngine;
using System.Collections;

public class PlayerAnimation2 : MonoBehaviour
{
    private Animator anim;
    private PlayerAttack playerAttack;
    public PlayerId player;
    private FightController fightController;
    private bool isSyncAnim = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerId>();
        if (GameController.Instance.type == FightType.Team && player.playerId == PhotonEngine.Instance.role.Id)
        {
            fightController = GameController.Instance.GetComponent<FightController>();
            isSyncAnim = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (GetComponent<Rigidbody>().velocity.magnitude > 1f)
        //{
        //    anim.SetBool("Move", true);  //开始移动
        //}
        //else
        //{
        //    anim.SetBool("Move", false);  //保持站立
        //}
    }

    public void OnAttackBtnClick(bool isPress, PosType posType)
    {
        if (playerAttack.isDead == true)  //死亡后不允许移动
        {
            return;
        }

        if (posType == PosType.Basic)  //基础攻击
        {
            if (isPress)  //按钮触发攻击
            {
                anim.SetTrigger("Attack");
                if (isSyncAnim)
                {
                    PlayerAnimationModel model = new PlayerAnimationModel() { attack = true };
                    fightController.SyncPlayerAnimation(model);
                }
            }
        }
        else
        {
            anim.SetBool("Skill0" + (int)posType, isPress);
            //if (isPress)
            //{
            //    anim.SetBool("Skill0" + (int)posType, true);
            //}
            //else
            //{
            //    anim.SetBool("Skill0" + (int)posType, false);
            //}
            if (isSyncAnim)
            {
                if (isPress)
                {
                    switch ((int)posType)
                    {
                        case 1:
                            PlayerAnimationModel model1 = new PlayerAnimationModel() { skill1 = true };
                            fightController.SyncPlayerAnimation(model1);
                            break;
                        case 2:
                            PlayerAnimationModel model2 = new PlayerAnimationModel() { skill2 = true };
                            fightController.SyncPlayerAnimation(model2);
                            break;
                        case 3:
                            PlayerAnimationModel model3 = new PlayerAnimationModel() { skill3 = true };
                            fightController.SyncPlayerAnimation(model3);
                            break;
                    }
                }
                else
                {
                    fightController.SyncPlayerAnimation(new PlayerAnimationModel());
                }
            }
        }
    }

    public void SyncPlayerAnimation(PlayerAnimationModel model)
    {
        if (model.attack)
        {
            anim.SetTrigger("Attack");
        }
        else if (model.die)
        {
            anim.SetTrigger("Die");
        }
        else if (model.takedamage)
        {
            anim.SetTrigger("TakeDamage");
        }
        anim.SetBool("Skill01", model.skill1);
        anim.SetBool("Skill02", model.skill2);
        anim.SetBool("Skill03", model.skill3);
    }
}
