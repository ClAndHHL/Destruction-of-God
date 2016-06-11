using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    private TweenPosition tween;
    private UIScrollView diamondToCoinView;
    private UIScrollView coinToDiamondView;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenPosition>();
        diamondToCoinView = transform.Find("coin_view").GetComponent<UIScrollView>();
        coinToDiamondView = transform.Find("diamond_view").GetComponent<UIScrollView>();
    }

    // Use this for initialization
    void Start()
    {
        diamondToCoinView.gameObject.SetActive(false);
        coinToDiamondView.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    public void DiamondToCoin()  //钻石兑换金币
    {
        diamondToCoinView.gameObject.SetActive(true);
        coinToDiamondView.gameObject.SetActive(false);
    }

    public void CoinToDiamond()  //金币兑换钻石
    {
        diamondToCoinView.gameObject.SetActive(false);
        coinToDiamondView.gameObject.SetActive(true);
    }
}
