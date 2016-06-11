using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnapsackInventory : MonoBehaviour
{
    public static KnapsackInventory instance;
    public List<KnapsackItem> mList = new List<KnapsackItem>();  //所有的物品格子

    private UILabel countLabel;
    private UILabel priceLabel;
    private UIButton sellBtn;
    private UIButton clearBtn;

    private int gridNum = 0;

    void Awake()
    {
        instance = this;
        countLabel = transform.Find("count").GetComponent<UILabel>();
        priceLabel = transform.Find("price_bg/price").GetComponent<UILabel>();
        sellBtn = transform.Find("sell_btn").GetComponent<UIButton>();
        clearBtn = transform.Find("clear_btn").GetComponent<UIButton>();

        InventoryManager.instance.OnInventoryChanged += OnInventoryChanged;
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnSellBtnClick");
        sellBtn.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "OnClearBtnClick");
        clearBtn.onClick.Add(ed2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Destroy()
    {
        InventoryManager.instance.OnInventoryChanged -= OnInventoryChanged;
    }

    void OnInventoryChanged()
    {
        UpdateShow();
    }

    public void UpdateShow()  //从InventoryManager的mList获取已有物品信息
    {
        int count = InventoryManager.instance.mList.Count;
        int mCount = mList.Count;
        int temp = 0;  //没有放置物品的格子
        for (int i = 0; i < count; i++)  //设置显示
        {
            InventoryItem it = InventoryManager.instance.mList[i];
            if (it.IsDress == false && it.Num != 0)  //如果装备没有正在穿戴，才会显示
            {
                mList[temp].SetItem(it);
                temp++;
            }
        }
        gridNum = temp;
        for (int i = temp; i < mCount; i++)  //清空显示
        {
            mList[i].ClearItem();
        }
        countLabel.text = gridNum + "/32";
    }

    public void AddItem(InventoryItem it)  //增加装备
    {
        foreach (KnapsackItem ki in mList)
        {
            if (ki.it == null)
            {
                ki.SetItem(it);
                gridNum++;
                break;
            }
            Debug.Log(ki.it.Inventory.Name);
        }
        countLabel.text = gridNum + "/32";
    }

    public void OnSellBtnClick()  //点击出售按钮
    {
        int price = int.Parse(priceLabel.text);
        PlayerInfomation.instance.GetCoin(price);
        transform.parent.SendMessage("CloseSell");  //上传到Knapsack处理
    }

    public void OnClearBtnClick()  //点击整理按钮
    {
        UpdateShow();
    }

    public void UpdateGrid()
    {
        gridNum = 0;
        foreach (KnapsackItem ki in mList)
        {
            if (ki.it != null)
            {
                gridNum++;  //不需要break
            }
        }
        countLabel.text = gridNum + "/32";
    }
}
