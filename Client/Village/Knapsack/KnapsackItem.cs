using UnityEngine;
using System.Collections;

public class KnapsackItem : MonoBehaviour
{
    public InventoryItem it;

    private UISprite _icon;
    public UISprite Icon
    {
        get
        {
            if (_icon == null)
            {
                return transform.Find("icon").GetComponent<UISprite>();
            }
            return _icon;
        }
        set
        {
            _icon = value;
        }
    }
    private UILabel _num;
    public UILabel Num
    {
        get
        {
            if (_num == null)
            {
                return transform.Find("num").GetComponent<UILabel>();
            }
            return _num;
        }
        set
        {
            _num = value;
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
        this.it = it;
        Icon.spriteName = it.Inventory.Icon;
        if (it.Num == 1)  //物品只有一个的时候默认不显示
        {
            Num.text = "";
        }
        else
        {
            Num.text = it.Num + "";
        }
    }

    public void ClearItem()
    {
        it = null;
        Icon.spriteName = "bg_道具";
        Num.text = "";
    }

    void OnClick()  //背包里的物品显示
    {
        if (it != null)
        {
            object[] array = new object[3];
            array[0] = it;
            array[1] = true;
            array[2] = this;
            transform.parent.parent.parent.SendMessage("OnInventoryClick", array);
        }
    }

    public void UseItem(int num)
    {
        int mNum = it.Num;
        if (mNum - num == 0)
        {
            ClearItem();
        }
        else if (mNum - num == 1)
        {
            Num.text = "";
        }
        else
        {
            Num.text = (mNum - num) + "";
        }
    }
}
