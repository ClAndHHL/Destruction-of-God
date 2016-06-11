using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskList : MonoBehaviour
{
    public static TaskList instance;
    private TweenPosition tween;

    private UIGrid taskGrid;
    public GameObject taskPrefab;

    private UIButton closeBtn;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenPosition>();
        taskGrid = transform.Find("Scroll View/Grid").GetComponent<UIGrid>();
        closeBtn = transform.Find("close_btn").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        TaskManager.instance.OnSyncTaskFinished += OnSyncTaskFinished;
        EventDelegate ed = new EventDelegate(this, "Hide");
        closeBtn.onClick.Add(ed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        TaskManager.instance.OnSyncTaskFinished -= OnSyncTaskFinished;
    }

    void InitTaskList()  //初始化任务列表信息
    {
        List<Task> taskList = TaskManager.instance.taskList;

        foreach (Task task in taskList)
        {
            GameObject go = NGUITools.AddChild(taskGrid.gameObject, taskPrefab);
            TaskItem ti = go.GetComponent<TaskItem>();
            ti.SetTask(task);
        }
        taskGrid.GetComponent<UIGrid>().enabled = true;
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    public void OnSyncTaskFinished()  //同步完成，可以开始初始化
    {
        InitTaskList();
    }
}
