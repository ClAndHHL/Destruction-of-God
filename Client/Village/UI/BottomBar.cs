using UnityEngine;
using System.Collections;

public class BottomBar : MonoBehaviour
{
    private UIButton fightBtn;
    private UIButton bagBtn;
    private UIButton taskBtn;
    private UIButton skillBtn;
    private UIButton shopBtn;
    private UIButton systemBtn;

    void Awake()
    {
        fightBtn = transform.Find("fight_btn").GetComponent<UIButton>();
        bagBtn = transform.Find("bag_btn").GetComponent<UIButton>();
        taskBtn = transform.Find("task_btn").GetComponent<UIButton>();
        skillBtn = transform.Find("skill_btn").GetComponent<UIButton>();
        shopBtn = transform.Find("shop_btn").GetComponent<UIButton>();
        systemBtn = transform.Find("system_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnFightBtnClick");
        fightBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnBagBtnClick");
        bagBtn.onClick.Add(ed2);
        EventDelegate ed3 = new EventDelegate(this, "OnTaskBtnClick");
        taskBtn.onClick.Add(ed3);
        EventDelegate ed4 = new EventDelegate(this, "OnSkillBtnClick");
        skillBtn.onClick.Add(ed4);
        EventDelegate ed5 = new EventDelegate(this, "OnShopBtnClick");
        shopBtn.onClick.Add(ed5);
        EventDelegate ed6 = new EventDelegate(this, "OnSystemBtnClick");
        systemBtn.onClick.Add(ed6);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFightBtnClick()
    {
        Map.instance.Show();
    }

    public void OnBagBtnClick()
    {
        Knapsack.instance.Show();
    }

    public void OnTaskBtnClick()
    {
        TaskList.instance.Show();
    }

    public void OnSkillBtnClick()
    {
        SkillList.instance.Show();
    }

    public void OnShopBtnClick()
    {
        Shop.instance.Show();
    }

    public void OnSystemBtnClick()
    {
        System_.instance.Show();
    }
}
