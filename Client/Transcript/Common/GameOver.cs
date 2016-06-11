using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GodCommon.Models;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    private TaskDBController controller;
    private FightController fightController;
    private TweenScale tween;
    private UISprite legend;
    private UISprite angel;
    private UILabel message;

    void Awake()
    {
        instance = this;
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<TaskDBController>();
        tween = GetComponent<TweenScale>();
        message = transform.Find("message").GetComponent<UILabel>();
        legend = transform.Find("legend").GetComponent<UISprite>();
        angel = transform.Find("angel").GetComponent<UISprite>();
    }

    // Use this for initialization
    void Start()
    {
        fightController = GameController.Instance.GetComponent<FightController>();
        fightController.OnSyncGameState += OnSyncGameState;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        fightController.OnSyncGameState -= OnSyncGameState;
    }

    public void OnBossDie()
    {
        if (GameController.Instance.type == FightType.Person)
        {
            OnGameSuccess();
        }
        else
        {
            if (GameController.Instance.isMaster)  //只在主机端做成功检测
            {
                OnGameSuccess();

                //向其他客户端发送游戏成功信息
                fightController.SendGameState(new GameStateModel() { isSuccess = true });
            }
        }
    }

    public void OnPlayerDie(int roleId)
    {
        if (GameController.Instance.type == FightType.Person)
        {
            OnGameFailure();
        }
        else
        {
            if (GameController.Instance.isMaster)  //只在主机端做失败检测
            {
                GameController.Instance.roleDieSet.Add(roleId);
                if (GameController.Instance.roleDieSet.Count == GameController.Instance.roleList.Count)  //死亡数等于角色数表示游戏失败
                {
                    OnGameFailure();

                    //向其他客户端发送游戏失败信息
                    fightController.SendGameState(new GameStateModel() { isSuccess = false });
                }
            }
        }
    }

    public void OnBackBtnClick()  //返回城镇
    {
        Destroy(GameController.Instance.gameObject);  //防止返回第二个场景出现多个GameController
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        LoadGame.instance.Show(ao);
    }

    void OnGameSuccess()
    {
        tween.PlayForward();
        message.text = "游戏胜利";
        legend.gameObject.SetActive(true);  //传说
        angel.gameObject.SetActive(false);

        foreach (var task in TaskManager.instance.taskList)  //更新任务进度
        {
            if (task.Progress == TaskProgress.Accept)
            {
                if (task.BookId == GameController.Instance.transcriptId)
                {
                    TaskDB taskDB = task.TaskDB;
                    taskDB.State = (int)TaskState.Reward;
                    controller.UpdateTaskDB(taskDB);
                    return;
                }
            }
        }
    }

    void OnGameFailure()
    {
        tween.PlayForward();
        message.text = "游戏失败";
        legend.gameObject.SetActive(false);
        angel.gameObject.SetActive(true);  //天使
    }

    public void OnSyncGameState(GameStateModel model)
    {
        if (model.isSuccess)
        {
            OnGameSuccess();
        }
        else
        {
            OnGameFailure();
        }
    }
}
