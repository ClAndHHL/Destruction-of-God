using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    private TweenPosition tweenP;
    private TweenScale tweenS;

    private UISprite headSprite;
    private UILabel nameLabel;
    private UILabel levelLabel;
    private UILabel powerLabel;
    private UILabel expLabel;
    private UISlider expBar;

    private UILabel diamondLabel;
    private UILabel coinLabel;

    private UILabel energyLabel;
    private UILabel energy_partLabel;
    private UILabel energy_allLabel;
    private UILabel toughenLabel;
    private UILabel toughen_partLabel;
    private UILabel toughen_allLabel;

    private UIButton closeBtn;
    private UIButton changeBtn;
    private UIButton confirmBtn;
    private UIButton cancelBtn;
    private UIInput nameInput;

    void Awake()
    {
        instance = this;
        PlayerInfomation.instance.OnPlayerInfoChanged += PlayerInfoChanged;

        tweenP = GetComponent<TweenPosition>();
        tweenS = transform.Find("change_bg").GetComponent<TweenScale>();

        headSprite = transform.Find("up/head_sprite").GetComponent<UISprite>();
        nameLabel = transform.Find("up/name").GetComponent<UILabel>();
        levelLabel = transform.Find("up/level").GetComponent<UILabel>();
        powerLabel = transform.Find("up/power").GetComponent<UILabel>();
        expLabel = transform.Find("up/exp_bg/exp_bar/exp").GetComponent<UILabel>();
        expBar = transform.Find("up/exp_bg/exp_bar").GetComponent<UISlider>();

        diamondLabel = transform.Find("middle/diamond").GetComponent<UILabel>();
        coinLabel = transform.Find("middle/coin").GetComponent<UILabel>();

        energyLabel = transform.Find("down/energy").GetComponent<UILabel>();
        energy_partLabel = transform.Find("down/energy_part").GetComponent<UILabel>();
        energy_allLabel = transform.Find("down/energy_all").GetComponent<UILabel>();
        toughenLabel = transform.Find("down/toughen").GetComponent<UILabel>();
        toughen_partLabel = transform.Find("down/toughen_part").GetComponent<UILabel>();
        toughen_allLabel = transform.Find("down/toughen_all").GetComponent<UILabel>();

        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
        changeBtn = transform.Find("up/change_btn").GetComponent<UIButton>();
        confirmBtn = transform.Find("change_bg/confirm_btn").GetComponent<UIButton>();
        cancelBtn = transform.Find("change_bg/cancel_btn").GetComponent<UIButton>();
        nameInput = transform.Find("change_bg/name_input").GetComponent<UIInput>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnCloseClick");
        closeBtn.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "OnChangeClick");
        changeBtn.onClick.Add(ed2);

        EventDelegate ed3 = new EventDelegate(this, "OnConfirmClick");
        confirmBtn.onClick.Add(ed3);

        EventDelegate ed4 = new EventDelegate(this, "OnCancelClick");
        cancelBtn.onClick.Add(ed4);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeShow();  //更新体力和历练的显示
    }

    void Destroy()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged -= PlayerInfoChanged;
    }

    void PlayerInfoChanged(InfoType type)
    {
        UpdateShow();
    }

    public void Show()
    {
        tweenP.PlayForward();
    }

    public void OnCloseClick()  //点击关闭按钮
    {
        tweenP.PlayReverse();
        tweenS.PlayReverse();  //如果打开更名面板，关闭人物状态时也一起关闭
    }

    public void OnChangeClick()  //点击更名按钮
    {
        tweenS.PlayForward();
    }

    public void OnConfirmClick()  //点击确认按钮
    {
        //联网验证名字TODO
        PlayerInfomation.instance.ChangeName(nameInput.value);
        tweenS.PlayReverse();
    }

    public void OnCancelClick()  //点击取消按钮
    {
        tweenS.PlayReverse();
    }

    public void UpdateShow()
    {
        PlayerInfomation info = PlayerInfomation.instance;

        nameLabel.text = info.Name;
        headSprite.spriteName = info.Head;
        levelLabel.text = info.Level + "";
        powerLabel.text = info.Power + "";
        int expNext = GameController.ExpByLevel(info.Level + 1);  //升到下一等级所需经验
        expLabel.text = info.Exp + "/" + expNext;
        expBar.value = (float)info.Exp / expNext;

        diamondLabel.text = info.Diamond + "";
        coinLabel.text = info.Coin + "";

        energyLabel.text = info.Energy + "/100";
        toughenLabel.text = info.Toughen + "/50";
    }

    public void UpdateTimeShow()
    {
        PlayerInfomation info = PlayerInfomation.instance;

        if (info.Energy >= 100)
        {
            energy_partLabel.text = "00:00:00";
            energy_allLabel.text = "00:00:00";
        }
        else
        {
            int seconds = 60 - (int)info.energy_timer;
            string str_seconds = seconds < 10 ? "0" + seconds : seconds + "";  //小于10的时候显示09，08，07...大于10的时候显示12，11，10...
            energy_partLabel.text = "00:00:" + str_seconds;

            int minutes = (99 - info.Energy);
            int hours = minutes / 60;
            minutes %= 60;
            string str_minutes = minutes < 10 ? "0" + minutes : minutes + "";
            string str_hours = hours < 10 ? "0" + hours : hours + "";
            energy_allLabel.text = str_hours + ":" + str_minutes + ":" + str_seconds;
        }

        if (info.Toughen >= 50)
        {
            toughen_partLabel.text = "00:00:00";
            toughen_allLabel.text = "00:00:00";
        }
        else
        {
            int seconds = 60 - (int)info.toughen_timer;
            string str_seconds = seconds < 10 ? "0" + seconds : seconds + "";  //小于10的时候显示09，08，07...大于10的时候显示12，11，10...
            toughen_partLabel.text = "00:00:" + str_seconds;

            int minutes = (49 - info.Toughen);
            int hours = minutes / 60;
            minutes %= 60;
            string str_minutes = minutes < 10 ? "0" + minutes : minutes + "";
            string str_hours = hours < 10 ? "0" + hours : hours + "";
            toughen_allLabel.text = str_hours + ":" + str_minutes + ":" + str_seconds;
        }
    }
}
