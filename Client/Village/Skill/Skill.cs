using UnityEngine;
using System.Collections;
using GodCommon.Models;

public enum SkillType
{
    Basic,
    Skill
}

public enum PosType
{
    Basic,
    One = 1,
    Two = 2,
    Three = 3
}

public class Skill
{
    //Id
    //名称
    //图标
    //角色类型(战士，女刺客 Boy, Girl)
    //技能类型（基础攻击， 技能 Basic, Skill）
    //位置(基础位置，一号技能位置，二号技能位置，三号技能位置 Basic, One, Two, Three)
    //冷却时间
    //基础攻击力
    //等级(默认为1级)
    #region property
    private int _id;
    private string _name;
    private string _icon;
    private PlayerType _mType;
    private SkillType _skillType;
    private PosType _posType;
    private int _coldTime;
    private int _damage;
    private int _level = 1;
    #endregion

    #region get set
    public int Id
    {
        get
        {
            return _id;
        }

        set
        {
            _id = value;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
        }
    }

    public string Icon
    {
        get
        {
            return _icon;
        }

        set
        {
            _icon = value;
        }
    }

    public PlayerType MType
    {
        get
        {
            return _mType;
        }

        set
        {
            _mType = value;
        }
    }

    public SkillType SkillType
    {
        get
        {
            return _skillType;
        }

        set
        {
            _skillType = value;
        }
    }

    public PosType PosType
    {
        get
        {
            return _posType;
        }

        set
        {
            _posType = value;
        }
    }

    public int ColdTime
    {
        get
        {
            return _coldTime;
        }

        set
        {
            _coldTime = value;
        }
    }

    public int Damage
    {
        get
        {
            return _damage;
        }

        set
        {
            _damage = value;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }

        set
        {
            _level = value;
        }
    }
    #endregion

    private SkillDB skillDB;
    public SkillDB SkillDB
    {
        get
        {
            return skillDB;
        }
        set
        {
            skillDB = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Upgrade()
    {
        Level++;
        if (skillDB == null)  //第一次还没有技能
        {
            skillDB = new SkillDB();
            skillDB.SkillId = Id;
            skillDB.Level = Level;
        }
        else
        {
            skillDB.Level = Level;
        }
    }

    public void SyncSkill(SkillDB skill)
    {
        SkillDB = skill;
        Level = skill.Level;
    }
}
