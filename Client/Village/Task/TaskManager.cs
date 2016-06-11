using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GodCommon.Models;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;
    public TaskDBController taskDBController;

    public TextAsset text;

    public List<Task> taskList = new List<Task>();
    public Dictionary<int, Task> taskDict = new Dictionary<int, Task>();  //将task根据id存入字典
    private PlayerAutoMove move;
    private Task currentTask;

    public event OnSyncTaskFinishedEvent OnSyncTaskFinished;

    void Awake()
    {
        instance = this;
        taskDBController = GetComponent<TaskDBController>();
        taskDBController.OnGetTaskDBList += OnGetTaskDBList;
        taskDBController.OnAddTaskDB += OnAddTaskDB;
        taskDBController.OnUpdateTaskDB += OnUpdateTaskDB;
        InitTask();  //初始化任务
        taskDBController.GetTaskDBList();  //从服务器获取任务列表 
    }

    // Use this for initialization
    void Start()
    {   
        move = GameObject.FindGameObjectWithTag("me").GetComponent<PlayerAutoMove>();
    }

    void Destroy()
    {
        taskDBController.OnGetTaskDBList -= OnGetTaskDBList;
        taskDBController.OnAddTaskDB -= OnAddTaskDB;
        taskDBController.OnUpdateTaskDB -= OnUpdateTaskDB;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGetTaskDBList(List<TaskDB> list)
    {
        if (list == null)
        {
            return;
        }
        foreach (var taskDB in list)
        {
            Task task = null;
            taskDict.TryGetValue(taskDB.TaskId, out task);
            task.SyncTask(taskDB);            
        }
        OnSyncTaskFinished();
        Debug.Log("OnSyncTaskFinished");
    }

    public void OnAddTaskDB(TaskDB taskDB)
    {
        Task task = null;
        taskDict.TryGetValue(taskDB.TaskId, out task);
        task.SyncTask(taskDB);
    }

    public void OnUpdateTaskDB()
    {

    }

    public void InitTask()
    {
        string str = text.ToString();
        string[] arrays = str.Split('\n');
        foreach (string array in arrays)
        {
            string[] proArray = array.Split('|');

            //Id 任务类型（Main,Reward，Daily）
            Task task = new Task();
            task.Id = int.Parse(proArray[0]);
            switch (proArray[1])
            {
                case "Main":
                    task.Type = TaskType.Main;
                    break;
                case "Reward":
                    task.Type = TaskType.Reward;
                    break;
                case "Daily":
                    task.Type = TaskType.Daily;
                    break;
            }

            //名称 图标 任务描述 获得的金币奖励 获得的钻石奖励 跟npc交谈的话语 Npc的id 副本的id
            task.Name = proArray[2];
            task.Icon = proArray[3];
            task.Describe = proArray[4];
            task.Coin = int.Parse(proArray[5]);
            task.Diamond = int.Parse(proArray[6]);
            task.TalkNpc = proArray[7];
            task.NpcId = int.Parse(proArray[8]);
            task.BookId = int.Parse(proArray[9]);
            taskList.Add(task);
            taskDict.Add(task.Id, task);
        }
    }

    public void OnExcuteTask(Task task)  //执行某个任务
    {
        currentTask = task;
        if (task.Progress == TaskProgress.UnAccept)
        {
            //还没有开始任务，导航到npc接受任务
            move.SetDestination(NpcManager.instance.GetNpcById(task.NpcId).transform.position);
        }
        else if (task.Progress == TaskProgress.Accept)
        {
            move.SetDestination(NpcManager.instance.transcript.transform.position);
        }
    }

    public void OnFinishedTask(Task task)  //完成某个任务
    {
        if (task.Progress == TaskProgress.Reward)
        {
            MessageManager.instance.ShowMessage("稍后给予奖励，不要着急哟~~~", 2f);
        }
        else if(task.Progress == TaskProgress.Complete)
        {
            MessageManager.instance.ShowMessage("少侠您已经完成该任务了，不要贪心哟~~~", 2f);
        }
    }

    public void OnAcceptTask()
    {
        currentTask.Progress = TaskProgress.Accept;
        currentTask.UpdateTask();  //向服务器更新任务信息
        move.SetDestination(NpcManager.instance.transcript.transform.position);  //寻路到副本入口
    }

    public void Reach()  //到达目的地
    {
        if (currentTask.Progress == TaskProgress.UnAccept)  //自动弹出NPC对话框
        {
            NpcDialog.instance.ShowDialog(currentTask.TalkNpc);
        }
        else if (currentTask.Progress == TaskProgress.Accept)  //自动弹出地下城地图
        {
            Map.instance.Show();
            Map.instance.ShowTranscript(currentTask.BookId);
        }
    }
}
