using UnityEngine;
using System.Collections;
using GodCommon.Models;

public enum InfoType
{
    Name,
    Head,
    Level,
    Power,
    Hp,
    Damage,
    Exp,
    Diamond,
    Coin,
    Energy,
    Toughen,
    Equip,
    All
}

public enum PlayerType
{
    Boy,
    Girl
}

public class PlayerInfomation : MonoBehaviour
{
    public static PlayerInfomation instance;
    //姓名
    //头像
    //等级
    //战斗力
    //经验数
    //钻石数
    //金币数
    //体力数
    //历练数
    //生命值
    //伤害值
    //8个装备（Id）
    #region property
    private string _name;
    private string _head;
    private int _level;
    private int _power;
    private int _exp;
    private int _diamond;
    private int _coin;
    private int _energy;
    private int _toughen;
    private int _hp;
    private int _damage;
    public PlayerType _mType;
    public InventoryItem _mHelm;
    public InventoryItem _mCloth;
    public InventoryItem _mWeapon;
    public InventoryItem _mShoes;
    public InventoryItem _mNecklace;
    public InventoryItem _mBracelet;
    public InventoryItem _mRing;
    public InventoryItem _mWing;
    public bool isFighting = false;
    #endregion

    public float energy_timer = 0f;
    public float toughen_timer = 0f;

    #region get set
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
    public string Head
    {
        get
        {
            return _head;
        }
        set
        {
            _head = value;
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
    public int Exp
    {
        get
        {
            return _exp;
        }
        set
        {
            _exp = value;
        }
    }
    public int Diamond
    {
        get
        {
            return _diamond;
        }
        set
        {
            _diamond = value;
        }
    }
    public int Coin
    {
        get
        {
            return _coin;
        }
        set
        {
            _coin = value;
        }
    }
    public int Energy
    {
        get
        {
            return _energy;
        }
        set
        {
            _energy = value;
        }
    }
    public int Toughen
    {
        get
        {
            return _toughen;
        }
        set
        {
            _toughen = value;
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
    #endregion

    public delegate void OnPlayerInfoChangedEvent(InfoType type);
    public event OnPlayerInfoChangedEvent OnPlayerInfoChanged;
    private RoleController roleController;
    private InventoryDBController inventoryDBController;

    void Awake()
    {
        instance = this;
        OnPlayerInfoChanged += OnPlayerInfoChange;  //注意将方法名字写对，该委托用于同步更新到服务器   
        roleController = GetComponent<RoleController>();
        inventoryDBController = GetComponent<InventoryDBController>();
    }

    // Use this for initialization
    void Start()
    {
        Init();  //根据从服务器获得信息将角色初始化
        InventoryManager.instance.OnSyncInventoryFinished += OnSyncInventoryFinished;  //装备发生变化的时候调用
    }

    // Update is called once per frame
    void Update()
    {
        if (isFighting)  //进入地下城不自动回复
        {
            return;
        }
        if (Energy < 100)  //体力小于100，每60秒自动回复1点
        {
            energy_timer += Time.deltaTime;
            if (energy_timer >= 60f)
            {
                Energy++;
                energy_timer = 0f;
                PhotonEngine.Instance.role.Energy = Energy;  //同步更新到服务器
                OnPlayerInfoChanged(InfoType.Energy);
            }
        }
        else
        {
            energy_timer = 0f;
        }

        if (Toughen < 50) //历练小于50，每60秒自动回复1点
        {
            toughen_timer += Time.deltaTime;
            if (toughen_timer >= 60f)
            {
                Toughen++;
                toughen_timer = 0f;
                PhotonEngine.Instance.role.Toughen = Toughen;  //同步更新到服务器
                OnPlayerInfoChanged(InfoType.Toughen);
            }
        }
        else
        {
            toughen_timer = 0f;
        }
    }

    void OnDestroy()
    {
        OnPlayerInfoChanged -= OnPlayerInfoChange;
        InventoryManager.instance.OnSyncInventoryFinished -= OnSyncInventoryFinished;
    }

    public void OnPlayerInfoChange(InfoType infoType)  //向服务器更改角色信息
    {
        if (infoType == InfoType.All || infoType == InfoType.Name || infoType == InfoType.Energy || infoType == InfoType.Toughen)
        {
            Debug.Log("OnPlayerInfoChange:" + infoType);
            roleController.UpdateRole(PhotonEngine.Instance.role);
        }
    }

    void Init()  //初始化从服务器获得的角色信息
    {
        Role role = PhotonEngine.Instance.role;
        Name = role.Name;
        if (role.IsMan)
        {
            Head = "头像底板男性";
            MType = PlayerType.Boy;
        }
        else
        {
            Head = "头像底板女性";
            MType = PlayerType.Girl;
        }
        Level = role.Level;
        Power =
        Exp = role.Exp;
        Coin = role.Coin;
        Diamond = role.Diamond;
        Energy = role.Energy;
        Toughen = role.Toughen;

        InitHpDamagePower();

        OnPlayerInfoChanged(InfoType.All);
    }

    public void ChangeName(string newName)
    {
        Name = newName;
        PhotonEngine.Instance.role.Name = newName;  //同步更新到服务器
        OnPlayerInfoChanged(InfoType.Name);
    }

    void InitHpDamagePower()  //初始化
    {
        Hp = Level * 100;
        Damage = Level * 50;
        Power = Hp + Damage;
    }

    public void PutOnEquip(InventoryItem it, bool isSync = true)
    {
        it.IsDress = true;
        //首先检测是否已经穿上相同类型的装备
        //没有->直接穿戴
        //有->先把已经穿戴的装备放回背包，再穿上选择的装备
        bool isDress = false;
        InventoryItem itDress = null;  //当前身上穿戴的装备
        switch (it.Inventory.EquipType)
        {
            case EquipType.Helm:
                if (_mHelm != null)
                {
                    isDress = true;
                    itDress = _mHelm;  //如果身上有穿戴装备，则先将装备赋值给itDress
                }
                _mHelm = it;
                break;
            case EquipType.Cloth:
                if (_mCloth != null)
                {
                    isDress = true;
                    itDress = _mCloth;
                }
                _mCloth = it;
                break;
            case EquipType.Weapon:
                if (_mWeapon != null)
                {
                    isDress = true;
                    itDress = _mWeapon;
                }
                _mWeapon = it;
                break;
            case EquipType.Shoes:
                if (_mShoes != null)
                {
                    isDress = true;
                    itDress = _mShoes;
                }
                _mShoes = it;
                break;
            case EquipType.Necklace:
                if (_mNecklace != null)
                {
                    isDress = true;
                    itDress = _mNecklace;
                }
                _mNecklace = it;
                break;
            case EquipType.Bracelet:
                if (_mBracelet != null)
                {
                    isDress = true;
                    itDress = _mBracelet;
                }
                _mBracelet = it;
                break;
            case EquipType.Ring:
                if (_mRing != null)
                {
                    isDress = true;
                    itDress = _mRing;
                }
                _mRing = it;
                break;
            case EquipType.Wing:
                if (_mWing != null)
                {
                    isDress = true;
                    itDress = _mWing;
                }
                _mWing = it;
                break;
        }
        if (isDress)  //原本有穿戴装备
        {
            Hp -= itDress.Inventory.Hp;  //先减去之前装备的属性
            Damage -= itDress.Inventory.Damage;

            itDress.IsDress = false;  //之前穿戴的装备设置为未穿戴
            KnapsackInventory.instance.AddItem(itDress);  //将装备放回物品栏里
            Debug.Log(isDress + "  " + it.Inventory.Name);
        }
        if (isSync)  //需要同步到服务器的时候才调用，刚进入游戏时不需要更新到服务器
        {
            if (isDress)
            {
                inventoryDBController.ChangeEquipment(it.InventoryDB, itDress.InventoryDB);  //换装更新到服务器
            }
            else
            {
                inventoryDBController.UpgradeEquipment(it.InventoryDB);  //isDress为true更新到服务器
            }
        }

        Hp += it.Inventory.Hp;  //再加上现在装备的属性
        Damage += it.Inventory.Damage;
        Power = Hp + Damage;
        OnPlayerInfoChanged(InfoType.Equip);
    }

    public void PutOffEquip(InventoryItem it)
    {
        it.IsDress = false;
        switch (it.Inventory.EquipType)
        {
            case EquipType.Helm:
                if (_mHelm != null)
                {
                    _mHelm = null;
                }
                break;
            case EquipType.Cloth:
                if (_mCloth != null)
                {
                    _mCloth = null;
                }
                break;
            case EquipType.Weapon:
                if (_mWeapon != null)
                {
                    _mWeapon = null;
                }
                break;
            case EquipType.Shoes:
                if (_mShoes != null)
                {
                    _mShoes = null;
                }
                break;
            case EquipType.Necklace:
                if (_mNecklace != null)
                {
                    _mNecklace = null;
                }
                break;
            case EquipType.Bracelet:
                if (_mBracelet != null)
                {
                    _mBracelet = null;
                }
                break;
            case EquipType.Ring:
                if (_mRing != null)
                {
                    _mRing = null;
                }
                break;
            case EquipType.Wing:
                if (_mWing != null)
                {
                    _mWing = null;
                }
                break;
        }
        KnapsackInventory.instance.AddItem(it);
        inventoryDBController.UpgradeEquipment(it.InventoryDB);  //isDress为false更新到服务器
        Hp -= it.Inventory.Hp;
        Damage -= it.Inventory.Damage;
        Power = Hp + Damage;
        OnPlayerInfoChanged(InfoType.Equip);
    }

    public bool CostCoin(int coin)  //花费金币
    {
        if (Coin >= coin)
        {
            Coin -= coin;
            PhotonEngine.Instance.role.Coin = Coin;
            OnPlayerInfoChanged(InfoType.Coin);
            return true;
        }
        return false;
    }

    public void GetCoin(int coin)  //获得金币
    {
        Coin += coin;
        PhotonEngine.Instance.role.Coin = Coin;
        OnPlayerInfoChanged(InfoType.Coin);
    }

    public bool CostEnergy(int energy)  //花费体力
    {
        if (Energy >= energy)
        {
            Energy -= energy;
            PhotonEngine.Instance.role.Energy = Energy;
            OnPlayerInfoChanged(InfoType.Energy);
            return true;
        }
        return false;
    }

    public bool Exchange(int coinChange, int diamondChange)  //金币和钻石交换
    {
        if ((Coin + coinChange >= 0) && (Diamond + diamondChange >= 0))
        {
            Coin += coinChange;
            Diamond += diamondChange;
            PhotonEngine.Instance.role.Coin = Coin;
            PhotonEngine.Instance.role.Diamond = Diamond;
            OnPlayerInfoChanged(InfoType.All);  //更新UI显示并且更新到服务器
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseItem(InventoryItem it, int num)
    {
        it.Num -= num;
        if (it.Num == 0)
        {
            InventoryManager.instance.mList.Remove(it);
        }
        Energy += it.Inventory.ApplyValue * num;
        if (Energy > 100)
        {
            Energy = 100;
        }
        OnPlayerInfoChanged(InfoType.Energy);
        KnapsackInventory.instance.UpdateGrid();
    }

    public int GetPower()  //返回角色当前战斗力 自身+装备
    {
        int power = Power;
        if (_mHelm != null)
        {
            power += (int)(_mHelm.Inventory.Power * (1 + (_mHelm.Level - 1) * 0.1));
        }
        if (_mCloth != null)
        {
            power += (int)(_mCloth.Inventory.Power * (1 + (_mCloth.Level - 1) * 0.1));
        }
        if (_mWeapon != null)
        {
            power += (int)(_mWeapon.Inventory.Power * (1 + (_mWeapon.Level - 1) * 0.1));
        }
        if (_mShoes != null)
        {
            power += (int)(_mShoes.Inventory.Power * (1 + (_mShoes.Level - 1) * 0.1));
        }
        if (_mNecklace != null)
        {
            power += (int)(_mNecklace.Inventory.Power * (1 + (_mNecklace.Level - 1) * 0.1));
        }
        if (_mBracelet != null)
        {
            power += (int)(_mBracelet.Inventory.Power * (1 + (_mBracelet.Level - 1) * 0.1));
        }
        if (_mRing != null)
        {
            power += (int)(_mRing.Inventory.Power * (1 + (_mRing.Level - 1) * 0.1));
        }
        if (_mWing != null)
        {
            power += (int)(_mWing.Inventory.Power * (1 + _mWing.Level * 0.1));
        }
        return power;
    }

    public void OnSyncInventoryFinished()
    {
        foreach (var temp in InventoryManager.instance.mList)
        {
            if (temp.IsDress)
            {
                Debug.Log("OnInventoryChanged():" + temp.Inventory.Name);
                PutOnEquip(temp, false);  //由于是从服务器获得的信息，所以不需要同步到服务器
            }
        }
    }
}
