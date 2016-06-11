using UnityEngine;
using System.Collections;

public class TopBar : MonoBehaviour
{
    private UILabel coinLabel;
    private UILabel diamondLabel;
    private UIButton coinPlusBtn;
    private UIButton diamondPlusBtn;

    void Awake()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged += OnPlayerInfoChanged;

        coinLabel = transform.Find("coin_bg/coin").GetComponent<UILabel>();
        diamondLabel = transform.Find("diamond_bg/diamond").GetComponent<UILabel>();
        coinPlusBtn = transform.Find("coin_bg/coin_plus").GetComponent<UIButton>();
        diamondPlusBtn = transform.Find("diamond_bg/diamond_plus").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged -= OnPlayerInfoChanged;
    }

    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.All || type == InfoType.Diamond || type == InfoType.Coin)
        {
            UpdateShow();
        }
    }

    public void UpdateShow()
    {
        PlayerInfomation info = PlayerInfomation.instance;

        diamondLabel.text = info.Diamond + "";
        coinLabel.text = info.Coin + "";
    }
}
