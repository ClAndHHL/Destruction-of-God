using UnityEngine;
using System.Collections;

public class NpcDialog : MonoBehaviour
{
    public static NpcDialog instance;
    private TweenScale tween;
    private UILabel dialog;
    private UIButton acceptBtn;
    private UIButton closeBtn;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenScale>();
        dialog = transform.Find("dialog").GetComponent<UILabel>();
        acceptBtn = transform.Find("accept_btn").GetComponent<UIButton>();
        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnAcceptBtnClick");
        acceptBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnCloseBtnClick");
        closeBtn.onClick.Add(ed2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowDialog(string talk)
    {
        dialog.text = talk;
        tween.PlayForward();
    }

    public void OnAcceptBtnClick()
    {
        TaskManager.instance.OnAcceptTask();
        tween.PlayReverse();
    }

    public void OnCloseBtnClick()
    {
        tween.PlayReverse();
    }
}
