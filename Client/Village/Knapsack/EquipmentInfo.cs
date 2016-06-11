using UnityEngine;
using System.Collections;

public class EquipmentInfo : MonoBehaviour
{
    private InventoryItem it;
    private KnapsackItem ki;
    private KnapsackEquip ke;

    private UISprite icon;
    private UILabel nameLabel;
    private UILabel hpLabel;
    private UILabel damageLabel;
    private UILabel levelLabel;
    private UILabel powerLabel;
    private UILabel info;
    private UILabel equipLabel;
    private UIButton equipBtn;
    private UIButton upgradeBtn;
    private UIButton closeBtn;

    private bool isLeft;

    void Awake()
    {
        icon = transform.Find("icon_bg/icon").GetComponent<UISprite>();
        nameLabel = transform.Find("equip_name").GetComponent<UILabel>();
        hpLabel = transform.Find("hp_label/hp").GetComponent<UILabel>();
        damageLabel = transform.Find("damage_label/damage").GetComponent<UILabel>();
        levelLabel = transform.Find("level_label/level").GetComponent<UILabel>();
        powerLabel = transform.Find("power_label/power").GetComponent<UILabel>();
        info = transform.Find("equip_info").GetComponent<UILabel>();
        equipLabel = transform.Find("equip_btn/Label").GetComponent<UILabel>();
        equipBtn = transform.Find("equip_btn").GetComponent<UIButton>();
        upgradeBtn = transform.Find("upgrade_btn").GetComponent<UIButton>();
        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnEquipBtnClick");
        equipBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnUpgradeBtnClick");
        upgradeBtn.onClick.Add(ed2);
        EventDelegate ed3 = new EventDelegate(this, "OnCloseBtnClick");
        closeBtn.onClick.Add(ed3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInfo(InventoryItem it, KnapsackItem ki, KnapsackEquip ke, bool isLeft = true)
    {
        gameObject.SetActive(true);
        this.it = it;
        this.ki = ki;
        this.ke = ke;
        this.isLeft = isLeft;
        Vector3 pos = transform.localPosition;
        if (isLeft)
        {
            transform.localPosition = new Vector3(-Mathf.Abs(pos.x), pos.y, pos.z);  //靠左显示
            equipLabel.text = "装备";
        }
        else
        {
            transform.localPosition = new Vector3(Mathf.Abs(pos.x), pos.y, pos.z);  //靠右显示
            equipLabel.text = "卸下";
        }
        icon.spriteName = it.Inventory.Icon;
        nameLabel.text = it.Inventory.Name;
        hpLabel.text = it.Inventory.Hp + "";
        damageLabel.text = it.Inventory.Damage + "";
        levelLabel.text = it.Level + "";
        powerLabel.text = it.Inventory.Power + "";
        info.text = it.Inventory.Info;
    }

    public void OnCloseBtnClick()
    {
        ClearItem();
        gameObject.SetActive(false);
        transform.parent.parent.SendMessage("HideBtn");  //通知禁用出售按钮
    }

    public void OnEquipBtnClick()
    {
        int startValue = PlayerInfomation.instance.GetPower();
        if (isLeft)  //穿戴
        {
            ki.ClearItem();  //背包装备格子清空
            PlayerInfomation.instance.PutOnEquip(it);  //先穿上装备，避免马上被销毁
        }
        else  //卸下
        {
            ke.ClearItem();  //人物装备格子清空
            PlayerInfomation.instance.PutOffEquip(it);  //先脱下装备，避免马上被销毁
        }
        OnCloseBtnClick();  //关闭界面，同时清空信息
        int endValue = PlayerInfomation.instance.GetPower();
        KnapsackInventory.instance.SendMessage("UpdateGrid");
        PowerMessage.instance.ShowPowerMessage(startValue, endValue);  //获得开始和结束的战斗力后开始播放动画     
    }

    public void OnUpgradeBtnClick()  //点击升级装备按钮
    {
        int coin = (it.Level + 1) * it.Inventory.Price;  //升级所需金币数
        bool isSuccess = PlayerInfomation.instance.CostCoin(coin);
        if (isSuccess)
        {
            it.Level++;
            levelLabel.text = it.Level + "";
            InventoryManager.instance.UpgradeEquipment(it);
        }
        else
        {
            MessageManager.instance.ShowMessage("金币不足，无法升级");
        }

    }

    void ClearItem()
    {
        it = null;
        ki = null;
    }
}
