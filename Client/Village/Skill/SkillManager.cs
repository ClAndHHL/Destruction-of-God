using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GodCommon.Models;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public SkillDBController skillDBController;
    public TextAsset text;
    private List<Skill> skillList = new List<Skill>();
    private Dictionary<int, Skill> skillDict = new Dictionary<int, Skill>();

    public event OnSyncSkillFinishedEvent OnSyncSkillFinished;

    void Awake()
    {
        instance = this;
        skillDBController = GetComponent<SkillDBController>();
        skillDBController.OnGetSkillList += OnGetSkillDBList;
        skillDBController.OnAddSkillDB += OnAddSkillDB;
        skillDBController.OnUpgradeSkillDB += OnUpgradeSkillDB;
        InitSkill();
        skillDBController.GetSkillList();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitSkill()
    {
        //Id 名称 图标 冷却时间 基础攻击力 等级(默认为1级)
        //角色类型(战士，女刺客 Boy, Girl) 
        //技能类型（基础攻击， 技能 Basic, Skill）
        //位置(基础位置，一号技能位置，二号技能位置，三号技能位置 Basic, One, Two, Three)
        string str = text.ToString();
        string[] arrays = str.Split('\n');
        foreach (string array in arrays)
        {
            string[] proArray = array.Split(',');
            Skill skill = new Skill();
            skill.Id = int.Parse(proArray[0]);
            skill.Name = proArray[1];
            skill.Icon = proArray[2];
            switch (proArray[3])
            {
                case "Boy":
                    skill.MType = PlayerType.Boy;
                    break;
                case "Girl":
                    skill.MType = PlayerType.Girl;
                    break;
            }
            switch (proArray[4])
            {
                case "Basic":
                    skill.SkillType = SkillType.Basic;
                    break;
                case "Skill":
                    skill.SkillType = SkillType.Skill;
                    break;
            }
            switch (proArray[5])
            {
                case "Basic":
                    skill.PosType = PosType.Basic;
                    break;
                case "One":
                    skill.PosType = PosType.One;
                    break;
                case "Two":
                    skill.PosType = PosType.Two;
                    break;
                case "Three":
                    skill.PosType = PosType.Three;
                    break;
            }
            skill.ColdTime = int.Parse(proArray[6]);
            skill.Damage = int.Parse(proArray[7]);
            skill.Level = 1;

            skillList.Add(skill);
            skillDict.Add(skill.Id, skill);
        }
    }

    public Skill GetSkillByPosition(PosType posType)
    {
        PlayerInfomation info = PlayerInfomation.instance;
        foreach (Skill skill in skillList)
        {
            if (skill.MType == info.MType && skill.PosType == posType)  //返回的技能唯一
            {
                return skill;
            }
        }
        return null;
    }

    public void OnGetSkillDBList(List<SkillDB> list)
    {
        foreach (var temp in list)
        {
            Skill skill = null;
            if (skillDict.TryGetValue(temp.SkillId, out skill))
            {
                skill.SyncSkill(temp);
            }
        }
        Debug.Log("OnSyncSkillFinished");
        OnSyncSkillFinished();
    }

    public void OnAddSkillDB(SkillDB skillDB)
    {

    }

    public void OnUpgradeSkillDB(SkillDB skillDB)
    {
        Skill skill = null;
        if (skillDict.TryGetValue(skillDB.SkillId, out skill))
        {
            skill.SyncSkill(skill.SkillDB);
        }
    }

    public void SkillUpgrade(Skill skill, bool isNew = true)  //升级技能
    {
        if (isNew)  //第一次升级
        {
            skillDBController.AddSkill(skill.SkillDB);
        }
        else
        {
            skillDBController.UpgradeSkill(skill.SkillDB);
        }
    }
}
