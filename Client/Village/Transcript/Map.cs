using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GodCommon.Models;

public class Map : MonoBehaviour
{
    public static Map instance;
    private FightController fightController;
    private TweenPosition tween;
    private UIButton backBtn;
    private Dictionary<int, TranscriptItem> transcriptDict = new Dictionary<int, TranscriptItem>();
    private TranscriptItem currentTranscript;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenPosition>();
        backBtn = transform.Find("back_btn").GetComponent<UIButton>();
        TranscriptItem[] transcripts = GetComponentsInChildren<TranscriptItem>();
        foreach (var temp in transcripts)  //存入字典
        {
            transcriptDict.Add(temp.id, temp);
        }
    }

    // Use this for initialization
    void Start()
    {
        fightController = GameController.Instance.GetComponent<FightController>();  //防止GameController未初始化时就已经调用
        fightController.OnHavingTeam += OnHavingTeam;
        fightController.OnWaitingTeam += OnWaitingTeam;
        fightController.OnCancelTeam += OnCancelTeam;
        EventDelegate ed = new EventDelegate(this, "OnBackBtnClick");
        backBtn.onClick.Add(ed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        fightController.OnHavingTeam -= OnHavingTeam;
        fightController.OnWaitingTeam -= OnWaitingTeam;
        fightController.OnCancelTeam -= OnCancelTeam;
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    void OnBackBtnClick()
    {
        Hide();
    }

    public void OnItemBtnClick(TranscriptItem transcript)  //点击地下城按钮
    {
        PlayerInfomation info = PlayerInfomation.instance;
        if (info.Level >= transcript.levelNeed)  //人物等级大于地下城等级，可以进入
        {
            currentTranscript = transcript;
            TranscriptDialog.instance.ShowDialog(transcript);
        }
        else  //等级不足，警告
        {
            TranscriptDialog.instance.ShowWarn();
        }
    }

    public void OnPersonBtnClick()  //点击个人地下城
    {
        bool isSuccess = PlayerInfomation.instance.CostEnergy(currentTranscript.eneryNeed);
        if (isSuccess)
        {
            GameController.Instance.type = FightType.Person;  //保存地下城类型
            GameController.Instance.transcriptId = currentTranscript.id;  //保存地下城id
            AsyncOperation ao = SceneManager.LoadSceneAsync(2);
            LoadGame.instance.Show(ao);
        }
        else
        {
            MessageManager.instance.ShowMessage("体力不足");
        }
    }

    public void OnTeamBtnClick()  //点击团队地下城
    {
        fightController.SendFight();  //向服务器发起组队请求
        WaitingTime.instance.ShowWaitingTime();  //显示倒计时UI
        TranscriptDialog.instance.OnCloseBtnClick();  //隐藏对话框UI
    }

    public void ShowTranscript(int id)  //自动寻路时候自动选择地下城
    {
        TranscriptItem transcript;
        transcriptDict.TryGetValue(id, out transcript);
        OnItemBtnClick(transcript);
    }

    public void CancelTeam()  //取消组队，用于WaitingTime调用
    {
        fightController.SendCancel();
    }

    public void OnHavingTeam(List<Role> roleList, int masterRoleId)  //组队成功，切换场景
    {
        Debug.Log("OnHavingTeam");
        if (PhotonEngine.Instance.role.Id == masterRoleId)  //判断是否为主机
        {
            GameController.Instance.isMaster = true;
        }
        else
        {
            GameController.Instance.isMaster = false;
        }
        GameController.Instance.roleList = roleList;
        GameController.Instance.type = FightType.Team;  //保存地下城类型
        GameController.Instance.transcriptId = currentTranscript.id;  //保存地下城id
        AsyncOperation ao = SceneManager.LoadSceneAsync(2);
        LoadGame.instance.Show(ao);
    }

    public void OnWaitingTeam()
    {
        MessageManager.instance.ShowMessage("等待中......");
    }

    public void OnCancelTeam()
    {
        MessageManager.instance.ShowMessage("取消成功");
    }
}
