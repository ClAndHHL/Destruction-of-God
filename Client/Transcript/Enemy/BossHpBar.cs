using UnityEngine;
using System.Collections;

public class BossHpBar : MonoBehaviour
{
    public static BossHpBar instance;
    private UISlider hpBar;
    private UILabel hpLabel;
    private int hp_max;

    void Awake()
    {
        instance = this;
        hpBar = GetComponent<UISlider>();
        hpLabel = transform.Find("hp_label").GetComponent<UILabel>();
    }

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);  //默认不显示
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHp(int hp_max)  //第一次保存血量最大值
    {
        MessageManager.instance.ShowMessage("勇士，来决一胜负吧！", 2f);
        gameObject.SetActive(true);
        this.hp_max = hp_max;
        UpdateHp(hp_max);
    }

    public void UpdateHp(int hp_now)  //之后只需要传递当前血量
    {
        if (hp_now < 0)
        {
            hp_now = 0;
        }
        hpBar.value = (float)hp_now / hp_max;
        hpLabel.text = hp_now + "/" + hp_max;
    }

    public void HideHp()  //隐藏血条
    {
        MessageManager.instance.ShowMessage("你，你，你赢了！！！", 2f);
        gameObject.SetActive(false);
    }
}
