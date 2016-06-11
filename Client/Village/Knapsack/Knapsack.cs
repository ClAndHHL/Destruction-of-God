using UnityEngine;
using System.Collections;

public class Knapsack : MonoBehaviour
{
    public static Knapsack instance;
    private TweenPosition tween;
    private EquipmentInfo equipment;
    private InventoryInfo inventory;
    private KnapsackItem ki;

    private UILabel priceLabel;
    private UIButton sellBtn;
    private UIButton closeBtn;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenPosition>();
        equipment = transform.Find("equipment_info/equipment_bg").GetComponent<EquipmentInfo>();
        inventory = transform.Find("inventory_info/inventory_bg").GetComponent<InventoryInfo>();
        priceLabel = transform.Find("inventory/price_bg/price").GetComponent<UILabel>();
        sellBtn = transform.Find("inventory/sell_btn").GetComponent<UIButton>();
        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        HideBtn();
        priceLabel.text = "";
        EventDelegate ed = new EventDelegate(this, "OnCloseBtnClick");
        closeBtn.onClick.Add(ed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInventoryClick(object[] array)  //当装备被点击的时候，显示装备信息面板
    {
        InventoryItem it = (InventoryItem)array[0];
        bool isLeft = (bool)array[1];
        if (it.Inventory.InvenType == InventoryType.Equip)
        {
            KnapsackItem ki = null;
            KnapsackEquip ke = null;
            if (isLeft)
            {
                ki = (KnapsackItem)array[2];
            }
            else
            {
                ke = (KnapsackEquip)array[2];
            }
            inventory.OnCloseBtnClick();  //保证另一个窗口已经关闭
            equipment.ShowInfo(it, ki, ke, isLeft);
        }
        else
        {
            KnapsackItem ki = (KnapsackItem)array[2];
            equipment.OnCloseBtnClick();  //保证另一个窗口已经关闭
            inventory.ShowInfo(it, ki);
        }

        if ((it.Inventory.InvenType == InventoryType.Equip && isLeft == true) || it.Inventory.InvenType != InventoryType.Equip)
        {
            //在背包里点击装备出售或者点击其他物品 人物穿戴的装备不可出售
            ki = (KnapsackItem)array[2];
            ShowBtn();
            priceLabel.text = ki.it.Inventory.Price * ki.it.Num + "";
        }
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void OnCloseBtnClick()
    {
        tween.PlayReverse();
    }

    void HideBtn()
    {
        sellBtn.SetState(UIButtonColor.State.Disabled, true);
        sellBtn.transform.GetComponent<Collider>().enabled = false;
        priceLabel.text = "";
    }

    void ShowBtn()
    {
        sellBtn.SetState(UIButtonColor.State.Normal, true);
        sellBtn.transform.GetComponent<Collider>().enabled = true;
    }

    void CloseSell()
    {
        InventoryManager.instance.RemoveItem(ki.it);  //先删除物品
        ki.ClearItem();  //再删除物品信息
        equipment.OnCloseBtnClick();
        inventory.OnCloseBtnClick();
        HideBtn();
        KnapsackInventory.instance.UpdateGrid();  //更新格子数量
    }
}
