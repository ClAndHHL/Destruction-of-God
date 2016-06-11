using UnityEngine;
using System.Collections;

public class TaskItem : MonoBehaviour
{
    private UISprite typeIcon;
    private UISprite icon;
    private UISprite reward1Icon;
    private UISprite reward2Icon;
    private UILabel nameLabel;
    private UILabel describeLabel;
    private UILabel reward1Label;
    private UILabel reward2Label;
    private UILabel taskLabel;
    private UIButton taskBtn;
    private UIButton rewardBtn;

    private Task task;

    void Awake()
    {
        typeIcon = transform.Find("type_icon").GetComponent<UISprite>();
        icon = transform.Find("icon").GetComponent<UISprite>();
        reward1Icon = transform.Find("reward1_icon").GetComponent<UISprite>();
        reward2Icon = transform.Find("reward2_icon").GetComponent<UISprite>();
        nameLabel = transform.Find("name").GetComponent<UILabel>();
        describeLabel = transform.Find("describe").GetComponent<UILabel>();
        reward1Label = transform.Find("reward1").GetComponent<UILabel>();
        reward2Label = transform.Find("reward2").GetComponent<UILabel>();
        taskLabel = transform.Find("task_btn/Label").GetComponent<UILabel>();
        taskBtn = transform.Find("task_btn").GetComponent<UIButton>();
        rewardBtn = transform.Find("reward_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed1 = new EventDelegate(this, "OnFight");
        taskBtn.onClick.Add(ed1);
        EventDelegate ed2 = new EventDelegate(this, "OnReward");
        rewardBtn.onClick.Add(ed2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTask(Task task)  //由TaskList调用
    {
        this.task = task;
        task.OnTaskChanged += OnTaskChanged;
        UpdateShow();
    }

    void UpdateShow()  //更新任务显示
    {
        switch (task.Type)
        {
            case TaskType.Main:
                typeIcon.spriteName = "pic_主线";
                icon.spriteName = "pic_星星";
                break;
            case TaskType.Reward:
                typeIcon.spriteName = "pic_奖赏";
                icon.spriteName = "金币";
                break;
            case TaskType.Daily:
                typeIcon.spriteName = "pic_日常";
                icon.spriteName = "钻石";
                break;
        }

        nameLabel.text = task.Name;
        describeLabel.text = task.Describe;
        if (task.Coin > 0 && task.Diamond > 0)
        {
            reward1Icon.spriteName = "金币";
            reward1Label.text = "+" + task.Coin;
            reward2Icon.spriteName = "钻石";
            reward2Label.text = "+" + task.Coin;
        }
        else if (task.Coin > 0)
        {
            reward1Icon.spriteName = "金币";
            reward1Label.text = "+" + task.Coin;
            reward2Icon.gameObject.SetActive(false);  //隐藏钻石图标
            reward2Label.gameObject.SetActive(false);  //隐藏钻石数量
        }
        else if (task.Diamond > 0)
        {
            reward1Icon.spriteName = "钻石";
            reward1Label.text = "+" + task.Diamond;
            reward2Icon.gameObject.SetActive(false);  //隐藏金币图标
            reward2Label.gameObject.SetActive(false);  //隐藏金币数量
        }
        switch (task.Progress)
        {
            case TaskProgress.UnAccept:
                taskBtn.gameObject.SetActive(true);
                rewardBtn.gameObject.SetActive(false);
                taskLabel.text = "下一步";
                break;
            case TaskProgress.Accept:
                taskBtn.gameObject.SetActive(true);
                rewardBtn.gameObject.SetActive(false);
                taskLabel.text = "战斗";
                break;
            case TaskProgress.Reward:
                taskBtn.gameObject.SetActive(false);  //隐藏
                rewardBtn.gameObject.SetActive(true);  //显示
                break;
            case TaskProgress.Complete:
                taskBtn.gameObject.SetActive(false);
                rewardBtn.gameObject.SetActive(true);
                break;
        }
    }

    void OnFight()
    {
        TaskManager.instance.OnExcuteTask(task);
        TaskList.instance.Hide();  //关闭任务窗口
    }

    void OnReward()
    {
        TaskManager.instance.OnFinishedTask(task);
        TaskList.instance.Hide();  //关闭任务窗口
    }

    void OnTaskChanged()
    {
        UpdateShow();
    }
}
