using UnityEngine;
using System.Collections;

public enum InventoryType
{
    Equip,
    Drug,
    Box
}

public enum EquipType
{
    Helm,
    Cloth,
    Weapon,
    Shoes,
    Necklace,
    Bracelet,
    Ring,
    Wing
}

public class Inventory
{
    //ID
    //名称
    //图标
    //类型（Equip，Drug）
    //装备类型（Helm，Cloth，Weapon，Shoe，Necklace，Bracelet，Ring，Wing）
    //作用类型（Name，Head，Level，Power，Exp，Diamond，Coin，Energy，Toughen，All）
    //售价
    //星级
    //品质
    //生命
    //伤害
    //战斗力
    //作用值
    //描述信息
    #region property
    private int _id;
    private string _name;
    private string _icon;
    private InventoryType _invenType;
    private EquipType _equipType;
    private InfoType _infoType;
    private int _price;
    private int _star;
    private int _quality;
    private int _hp;
    private int _damage;
    private int _power;
    private int _applyValue;
    private string _info;
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
    public InventoryType InvenType
    {
        get
        {
            return _invenType;
        }
        set
        {
            _invenType = value;
        }
    }
    public EquipType EquipType
    {
        get
        {
            return _equipType;
        }
        set
        {
            _equipType = value;
        }
    }
    public InfoType InfoType
    {
        get
        {
            return _infoType;
        }
        set
        {
            _infoType = value;
        }
    }
    public int Price
    {
        get
        {
            return _price;
        }
        set
        {
            _price = value;
        }
    }
    public int Star
    {
        get
        {
            return _star;
        }
        set
        {
            _star = value;
        }
    }
    public int Quality
    {
        get
        {
            return _quality;
        }
        set
        {
            _quality = value;
        }
    }
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
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
    public int Power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
        }
    }
    public int ApplyValue
    {
        get
        {
            return _applyValue;
        }
        set
        {
            _applyValue = value;
        }
    }
    public string Info
    {
        get
        {
            return _info;
        }
        set
        {
            _info = value;
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
