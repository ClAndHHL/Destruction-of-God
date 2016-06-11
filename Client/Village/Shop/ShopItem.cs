using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour
{
    public int coinChange = 0;
    public int diamondChange = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnConfirmBtnClick()
    {
        bool isSuccess = PlayerInfomation.instance.Exchange(coinChange, diamondChange);
        if (isSuccess)
        {
            MessageManager.instance.ShowMessage("兑换成功");
        }
        else
        {
            if (coinChange < 0)  //想用金币兑换钻石
            {
                MessageManager.instance.ShowMessage("金币不足");
            }
            else  //想用钻石兑换金币
            {
                MessageManager.instance.ShowMessage("钻石不足");
            }
        }
    }
}
