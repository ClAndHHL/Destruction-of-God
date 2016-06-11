using UnityEngine;
using System.Collections;

public class InventoryInfo : MonoBehaviour
{
    private InventoryItem it;
    private KnapsackItem ki;

    private UISprite icon;
    private UILabel nameLabel;
    private UILabel info;
    private UILabel btnLabel;
    private UIButton useBtn;
    private UIButton usemoreBtn;
    private UIButton closeBtn;

    void Awake()
    {
        icon = transform.Find("icon_bg/icon").GetComponent<UISprite>();
        nameLabel = transform.Find("item_name").GetComponent<UILabel>();
        info = transform.Find("item_info").GetComponent<UILabel>();
        btnLabel = transform.Find("usemore_btn/Label").GetComponent<UILabel>();
        useBtn = transform.Find("use_btn").GetComponent<UIButton>();
        usemoreBtn = transform.Find("usemore_btn").GetComponent<UIButton>();
        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnUseBtnClick");
        useBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnUsemoreBtnClick");
        usemoreBtn.onClick.Add(ed2);
        EventDelegate ed3 = new EventDelegate(this, "OnCloseBtnClick");
        closeBtn.onClick.Add(ed3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInfo(InventoryItem it, KnapsackItem ki)
    {
        gameObject.SetActive(true);
        this.it = it;
        this.ki = ki;
        icon.spriteName = it.Inventory.Icon;
        nameLabel.text = it.Inventory.Name;
        info.text = it.Inventory.Info;
        btnLabel.text = "批量使用(" + it.Num + ")";
    }

    public void OnUseBtnClick()
    {
        ki.UseItem(1);
        PlayerInfomation.instance.UseItem(it, 1);
        OnCloseBtnClick();
    }

    public void OnUsemoreBtnClick()
    {
        ki.UseItem(it.Num);
        PlayerInfomation.instance.UseItem(it, it.Num);
        OnCloseBtnClick();
    }

    public void OnCloseBtnClick()
    {
        ClearItem();
        gameObject.SetActive(false);
        transform.parent.parent.SendMessage("HideBtn");  //通知禁用出售按钮
    }

    void ClearItem()
    {
        it = null;
        ki = null;
    }
}
