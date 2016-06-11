using UnityEngine;
using System.Collections;
using GodCommon.Models;

public class InventoryItem
{
    //Inventory
    //数量
    //物品等级
    private Inventory _inventory;
    private int _num;
    private int _level;
    private bool isDress = false;  //是否已经穿戴上
    private InventoryDB inventoryDB;

    public InventoryDB InventoryDB
    {
        get
        {
            return inventoryDB;
        }
    }

    public InventoryItem() { }

    public InventoryItem(InventoryDB inventoryDB)  //重载
    {
        this.inventoryDB = inventoryDB;
        Inventory inventoryTemp;
        InventoryManager.instance.Dict.TryGetValue(inventoryDB.InventoryId, out inventoryTemp);
        _inventory = inventoryTemp;
        _num = inventoryDB.Num;
        _level = inventoryDB.Level;
        isDress = inventoryDB.IsDress;
    }

    public Inventory Inventory
    {
        get
        {
            return _inventory;
        }
        set
        {
            _inventory = value;
        }
    }
    public int Num
    {
        get
        {
            return _num;
        }
        set
        {
            _num = value;
            inventoryDB.Num = Num;
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
            inventoryDB.Level = _level;
        }
    }
    public bool IsDress
    {
        get
        {
            return isDress;
        }
        set
        {
            isDress = value;
            inventoryDB.IsDress = isDress;
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

    public InventoryDB NewInventoryDB()
    {
        InventoryDB inventoryDB = new InventoryDB();
        inventoryDB.InventoryId = Inventory.Id;
        inventoryDB.Num = Num;
        inventoryDB.Level = Level;
        inventoryDB.IsDress = IsDress;
        return inventoryDB;
    }
}
