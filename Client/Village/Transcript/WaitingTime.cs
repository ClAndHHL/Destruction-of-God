using UnityEngine;
using System.Collections;

public class WaitingTime : MonoBehaviour
{
    public static WaitingTime instance;
    private TweenScale tween;
    private UILabel waitingTime;
    private UIButton cancelBtn;
    public int time = 30;
    public float timer = 0f;
    private bool isStart = false;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenScale>();
        waitingTime = transform.Find("time").GetComponent<UILabel>();
        cancelBtn = transform.Find("cancel_btn").GetComponent<UIButton>();
    }

	// Use this for initialization
	void Start () {
        EventDelegate ed = new EventDelegate(this, "OnCancelBtnClick");
        cancelBtn.onClick.Add(ed);
	}
	
	// Update is called once per frame
	void Update () {
        if (isStart)
        {
            timer += Time.deltaTime;
            int remainTime = time - (int)timer;
            waitingTime.text = remainTime + "";  //显示剩余时间
            if (timer > time)  //计时结束
            {
                OnTimeEnd();
            }
        }
	}

    public void ShowWaitingTime()
    {
        timer = 0f;
        tween.PlayForward();
        isStart = true;
    }

    public void HideWaitingTime()
    {
        tween.PlayReverse();
        isStart = false;
    }

    public void OnTimeEnd()  //倒计时结束
    {
        HideWaitingTime();
        transform.parent.SendMessage("CancelTeam");
    }

    public void OnCancelBtnClick()  //点击取消组队按钮
    {
        HideWaitingTime();
        transform.parent.SendMessage("CancelTeam");
    }
}
