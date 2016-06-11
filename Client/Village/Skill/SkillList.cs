using UnityEngine;
using System.Collections;

public class SkillList : MonoBehaviour
{
    public static SkillList instance;
    private TweenPosition tween;
    private Skill skill;
    private UILabel skillName;
    private UILabel skillDescribe;
    private UILabel btnLabel;
    private UIButton closeBtn;
    private UIButton upgradeBtn;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenPosition>();
        skillName = transform.Find("bg/name").GetComponent<UILabel>();
        skillDescribe = transform.Find("bg/describe").GetComponent<UILabel>();
        btnLabel = transform.Find("upgrade_btn/Label").GetComponent<UILabel>();
        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
        upgradeBtn = transform.Find("upgrade_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        skillName.text = "";
        skillDescribe.text = "";
        HideBtn("选择技能");

        EventDelegate ed1 = new EventDelegate(this, "OnCloseBtnClick");
        closeBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnUpgradeBtnClick");
        upgradeBtn.onClick.Add(ed2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HideBtn(string label = "")  //按钮失效
    {
        upgradeBtn.SetState(UIButtonColor.State.Disabled, true);
        upgradeBtn.GetComponent<Collider>().enabled = false;
        if (label != "")
        {
            btnLabel.text = label;  //修改按钮上的字
        }
    }

    void ShowBtn(string label = "")  //按钮显示
    {
        upgradeBtn.SetState(UIButtonColor.State.Normal, true);
        upgradeBtn.GetComponent<Collider>().enabled = true;
        if (label != "")
        {
            btnLabel.text = label;  //修改按钮上的字
        }
    }

    void UpdateShow()
    {
        skillName.text = skill.Name + "  Lv." + skill.Level;
        skillDescribe.text = "当前攻击力：  " + (skill.Damage * skill.Level) + '\n'
            + "下一级攻击力：  " + (skill.Damage * (skill.Level + 1)) + '\n'
            + "升级所需金币：  " + (skill.Level + 1) + " * 500" + '\n';
        ShowBtn("升级");  //显示升级按钮
    }

    public void OnSkillBtnClick(Skill skill)  //显示技能信息
    {
        //耗费的金币数量:所升等级*500
        //所升等级不能超过角色当前等级
        //攻击力加成:基础攻击力* 技能等级
        //基础技能basic的攻击力为:基础攻击力* 角色等级
        this.skill = skill;
        UpdateShow();
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    public void OnCloseBtnClick()
    {
        skillName.text = "";
        skillDescribe.text = "";
        HideBtn("选择技能");
        Hide();
    }

    public void OnUpgradeBtnClick()
    {
        int coin = (skill.Level + 1) * 500;  //下一级所需金币
        int level = skill.Level;  //当前技能等级
        PlayerInfomation info = PlayerInfomation.instance;
        if (info.Level <= level)
        {
            MessageManager.instance.ShowMessage("等级不足");
            return;
        }
        bool isSuccess = PlayerInfomation.instance.CostCoin(coin);
        if (isSuccess)  //金币足够
        {
            skill.Upgrade();
            if (skill.Level == 2)  //第一次升级
            {
                SkillManager.instance.SkillUpgrade(skill);
            }
            else
            {
                SkillManager.instance.SkillUpgrade(skill, false);
            }
            UpdateShow();
        }
        else
        {
            MessageManager.instance.ShowMessage("金币不足");
        }
    }
}
