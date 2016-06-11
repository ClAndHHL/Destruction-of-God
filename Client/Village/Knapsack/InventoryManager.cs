using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GodCommon.Models;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryDBController inventoryDBController;
    public TextAsset text;
    public Dictionary<int, Inventory> Dict = new Dictionary<int, Inventory>();
    public List<InventoryItem> mList = new List<InventoryItem>();

    public delegate void OnInventoryChangedEvent();
    public event OnSyncInventoryFinishedEvent OnSyncInventoryFinished;
    public event OnInventoryChangedEvent OnInventoryChanged;

    void Awake()
    {
        instance = this;
        inventoryDBController = GetComponent<InventoryDBController>();
        inventoryDBController.OnGetInventoryDBList += OnGetInventoryDBList;
        inventoryDBController.OnAddInventoryDB += OnAddInventoryDB;
        InitInfo();  //初始化物品信息
    }

    // Use this for initialization
    void Start()
    {
        //GetInventoryInfo();
        inventoryDBController.GetInventoryDBList();  //从服务器获得物品列表
    }

    // Update is called once per frame
    void Update()
    {
        //模拟捡起物品
        if (Input.GetKeyDown(KeyCode.X))
        {
            PickUp();
        }
    }

    void Destroy()
    {
        inventoryDBController.OnGetInventoryDBList -= OnGetInventoryDBList;
        inventoryDBController.OnAddInventoryDB -= OnAddInventoryDB;
    }

    void InitInfo()
    {
        string str = text.ToString();
        string[] itemArray = str.Split('\n');
        foreach (string item in itemArray)
        {
            //ID 名称 图标 类型（Equip,Drug） 装备类型(Helm,Cloth,Weapon,Shoes,Necklace,Bracelet,Ring,Wing)
            string[] proArray = item.Split('|');
            Inventory inventory = new Inventory();
            inventory.Id = int.Parse(proArray[0]);
            inventory.Name = proArray[1];
            inventory.Icon = proArray[2];
            switch (proArray[3])
            {
                case "Equip":
                    inventory.InvenType = InventoryType.Equip;
                    break;
                case "Drug":
                    inventory.InvenType = InventoryType.Drug;
                    break;
                case "Box":
                    inventory.InvenType = InventoryType.Box;
                    break;
            }
            if (inventory.InvenType == InventoryType.Equip)
            {
                switch (proArray[4])
                {
                    case "Helm":
                        inventory.EquipType = EquipType.Helm;
                        break;
                    case "Cloth":
                        inventory.EquipType = EquipType.Cloth;
                        break;
                    case "Weapon":
                        inventory.EquipType = EquipType.Weapon;
                        break;
                    case "Shoes":
                        inventory.EquipType = EquipType.Shoes;
                        break;
                    case "Necklace":
                        inventory.EquipType = EquipType.Necklace;
                        break;
                    case "Bracelet":
                        inventory.EquipType = EquipType.Bracelet;
                        break;
                    case "Ring":
                        inventory.EquipType = EquipType.Ring;
                        break;
                    case "Wing":
                        inventory.EquipType = EquipType.Wing;
                        break;
                }
            }
            //售价 星级 品质 伤害 生命 战斗力 作用类型 作用值 描述
            inventory.Price = int.Parse(proArray[5]);
            if (inventory.InvenType == InventoryType.Equip)
            {
                inventory.Star = int.Parse(proArray[6]);
                inventory.Quality = int.Parse(proArray[7]);
                inventory.Damage = int.Parse(proArray[8]);
                inventory.Hp = int.Parse(proArray[9]);
                inventory.Power = int.Parse(proArray[10]);
            }
            if (inventory.InvenType == InventoryType.Drug)
            {
                inventory.ApplyValue = int.Parse(proArray[12]);
            }
            inventory.Info = proArray[13];

            Dict.Add(inventory.Id, inventory);
        }
    }

    void GetInventoryInfo()  //测试代码
    {
        //for (int i = 0; i < 20; i++)
        //{
        //    int id = Random.Range(1001, 1020);
        //    Inventory inventory = null;
        //    Dict.TryGetValue(id, out inventory);
        //    if (inventory.InvenType == InventoryType.Equip)
        //    {
        //        InventoryItem it = new InventoryItem();
        //        it.Inventory = inventory;
        //        it.Num = 1;
        //        it.Level = Random.Range(1, 10);
        //        mList.Add(it);
        //    }
        //    else
        //    {
        //        //先判断背包里面是否已经存在物品
        //        InventoryItem it = null;
        //        bool isExit = false;
        //        foreach (InventoryItem temp in mList)
        //        {
        //            if (temp.Inventory.Id == id)
        //            {
        //                isExit = true;
        //                it = temp;
        //                break;
        //            }
        //        }
        //        if (isExit)
        //        {
        //            it.Num++;
        //        }
        //        else
        //        {
        //            it = new InventoryItem();
        //            it.Inventory = inventory;
        //            it.Num = 1;
        //            it.Level = Random.Range(1001, 1020);
        //            mList.Add(it);
        //        }
        //    }
        //}
        //OnInventoryChanged();
    }

    public void RemoveItem(InventoryItem it)
    {
        mList.Remove(it);
    }

    public void OnGetInventoryDBList(List<InventoryDB> list)
    {
        foreach (var inventoryDB in list)
        {
            InventoryItem item = new InventoryItem(inventoryDB);
            mList.Add(item);
        }
        Debug.Log("OnSyncInventoryFinished");
        OnSyncInventoryFinished();
        OnInventoryChanged();
    }

    public void OnAddInventoryDB(InventoryDB inventoryDB)
    {
        InventoryItem item = new InventoryItem(inventoryDB);
        mList.Add(item);
        OnInventoryChanged();
    }

    public void UpgradeEquipment(InventoryItem it)  //升级装备
    {
        inventoryDBController.UpgradeEquipment(it.InventoryDB);
    }

    public void PickUp()
    {
        int id = Random.Range(1001, 1020);  //随机生成物品
        Inventory inventory = null;
        Dict.TryGetValue(id, out inventory);
        if (inventory.InvenType == InventoryType.Equip)  //获得的是装备
        {
            InventoryDB inventoryDB = new InventoryDB();
            inventoryDB.InventoryId = id;
            inventoryDB.Num = 1;
            inventoryDB.Level = Random.Range(1, 11);  //随机生成等级
            inventoryDB.IsDress = false;
            inventoryDBController.AddInventoryDB(inventoryDB);
        }
        else  //获得的不是装备
        {
            InventoryItem it = null;
            bool isExit = false;
            foreach (InventoryItem temp in mList)  //先判断背包里面是否已经存在物品
            {
                if (temp.Inventory.Id == id)
                {
                    isExit = true;
                    it = temp;
                    break;
                }
            }
            if (isExit)  //背包已经存在
            {
                it.Num++;
                inventoryDBController.UpgradeEquipment(it.InventoryDB);  //UpgradeEquipment可以用作UpdataInventoryDB
            }
            else  //背包不存在
            {
                InventoryDB inventoryDB = new InventoryDB();
                inventoryDB.InventoryId = id;
                inventoryDB.Num = 1;
                inventoryDB.Level = 0;  //随机生成等级
                inventoryDB.IsDress = false;
                inventoryDBController.AddInventoryDB(inventoryDB);
            }
        }
    }
}
