using UnityEngine;
using System.Collections;

public class TranscriptDialog : MonoBehaviour
{
    public static TranscriptDialog instance;
    private TweenScale tween;
    private UILabel describe;
    private UILabel energy;
    private UIButton personBtn;
    private UIButton teamBtn;
    private UIButton closeBtn;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenScale>();
        describe = transform.Find("bg/describe").GetComponent<UILabel>();
        energy = transform.Find("bg/energy").GetComponent<UILabel>();
        personBtn = transform.Find("bg/person_btn").GetComponent<UIButton>();
        teamBtn = transform.Find("bg/team_btn").GetComponent<UIButton>();
        closeBtn = transform.Find("bg/close_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed = new EventDelegate(this, "OnCloseBtnClick");
        closeBtn.onClick.Add(ed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCloseBtnClick()
    {
        tween.PlayReverse();
    }

    public void ShowDialog(TranscriptItem transcript)  //显示提示信息
    {
        tween.PlayForward();
        describe.color = Color.yellow;
        describe.text = transcript.describe;
        energy.text = transcript.eneryNeed + "";
        ShowBtn();
    }

    public void ShowWarn()  //等级不足，警告
    {
        tween.PlayForward();
        describe.color = Color.red;
        describe.text = "等级不足，无法进入地下城";
        energy.text = "未知";
        HideBtn();
    }

    public void HideBtn()
    {
        personBtn.SetState(UIButtonColor.State.Disabled, true);  //进入按钮失效
        personBtn.GetComponent<Collider>().enabled = false;
        teamBtn.SetState(UIButtonColor.State.Disabled, true);  //进入按钮失效
        teamBtn.GetComponent<Collider>().enabled = false;
    }

    public void ShowBtn()
    {
        personBtn.SetState(UIButtonColor.State.Normal, true);
        personBtn.GetComponent<Collider>().enabled = true;
        teamBtn.SetState(UIButtonColor.State.Normal, true);
        teamBtn.GetComponent<Collider>().enabled = true;
    }
}
