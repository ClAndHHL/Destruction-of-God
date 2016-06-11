using UnityEngine;
using System.Collections;
using GodCommon.Models;
using System;

public enum TaskType
{
    Main,  //主线任务
    Reward,  //赏金任务
    Daily  //日常任务
}

public enum TaskProgress
{
    UnAccept,
    Accept,
    Complete,
    Reward,
}

public class Task
{
    //Id
    //任务类型（Main,Reward，Daily）
    //名称
    //图标
    //任务描述
    //获得的金币奖励
    //获得的钻石奖励
    //跟npc交谈的话语
    //Npc的id
    //副本的id
    //任务的状态
    //未开始
    //接受任务
    //任务完成
    //获取奖励（结束）
    #region property
    private int _id;
    private TaskType _type;
    private string _name;
    private string _icon;
    private string _describe;
    private int _coin;
    private int _diamond;
    private string _talkNpc;
    private int _npcId;
    private int _bookId;
    private TaskProgress _progress = TaskProgress.UnAccept;  //默认未接受

    public TaskDB TaskDB
    {
        get;
        set;
    }
    #endregion

    #region get set
    public int Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    public TaskType Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public string Icon
    {
        get
        {
            return _icon;
        }
        set
        {
            _icon = value;
        }
    }

    public string Describe
    {
        get
        {
            return _describe;
        }
        set
        {
            _describe = value;
        }
    }

    public int Coin
    {
        get
        {
            return _coin;
        }
        set
        {
            _coin = value;
        }
    }

    public int Diamond
    {
        get
        {
            return _diamond;
        }
        set
        {
            _diamond = value;
        }
    }

    public string TalkNpc
    {
        get
        {
            return _talkNpc;
        }
        set
        {
            _talkNpc = value;
        }
    }

    public int NpcId
    {
        get
        {
            return _npcId;
        }
        set
        {
            _npcId = value;
        }
    }

    public int BookId
    {
        get
        {
            return _bookId;
        }
        set
        {
            _bookId = value;
        }
    }

    public TaskProgress Progress
    {
        get
        {
            return _progress;
        }
        set
        {
            if (_progress != value)
            {
                _progress = value;
                OnTaskChanged();
            }
        }
    }
    #endregion

    public delegate void OnTaskChangeEvent();
    public event OnTaskChangeEvent OnTaskChanged;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SyncTask(TaskDB taskDB)  //用来同步任务信息
    {
        TaskDB = taskDB;
        _progress = (TaskProgress)TaskDB.State;  //注意此处使用_progress而不是Progress 防止同步任务之前taskItem就更新导致出错
    }

    public void UpdateTask()
    {
        if (TaskDB == null)  //还没有任务
        {
            TaskDB = new TaskDB();
            TaskDB.Type = (int)Type;  //枚举强转
            TaskDB.State = (int)Progress;  //枚举强转
            TaskDB.TaskId = Id;
            TaskManager.instance.taskDBController.AddTaskDB(TaskDB);  //添加到服务器
        }
        else  //已经存在任务
        {
            TaskDB.State = (int)Progress;  //枚举强转
            TaskManager.instance.taskDBController.UpdateTaskDB(TaskDB);  //更新到服务器
        }
    }
}
