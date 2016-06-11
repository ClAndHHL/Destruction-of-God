using UnityEngine;
using System.Collections;

public class KnapsackEquip : MonoBehaviour
{
    private InventoryItem it;

    private UISprite _icon;
    public UISprite Icon
    {
        get
        {
            if (_icon == null)
            {
                _icon = GetComponent<UISprite>();
            }
            return _icon;
        }
        set
        {
            _icon = value;
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

    public void SetItem(InventoryItem it)
    {
        if (it == null)
        {
            return;
        }
        this.it = it;
        Icon.spriteName = it.Inventory.Icon;
    }

    public void ClearItem()
    {
        it = null;
        Icon.spriteName = "bg_道具";
    }

    void OnClick()  //穿戴上的装备显示
    {
        if (it != null)  //空格子不允许上传事件
        {
            object[] array = new object[3];
            array[0] = it;
            array[1] = false;
            array[2] = this;
            transform.parent.parent.parent.SendMessage("OnInventoryClick", array);
        }
    }
}
