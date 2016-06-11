using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GodCommon.Models;
using UnityEngine.SceneManagement;

public enum IsLogin  //是否已经登录
{
    Yes,
    No
}

public enum IsRoleExist  //是否已经存在角色
{
    Yes,
    No
}

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;
    private LoginController loginController;
    private RegisterController registerController;
    private RoleController roleController;

    public TweenScale startTween;
    public TweenScale loginTween;
    public TweenScale registerTween;
    public TweenScale serverTween;
    public TweenPosition enterTween;
    public TweenPosition playerCreatTween;

    public UILabel usernameLabel_start;
    public UILabel serverLabel_start;
    public UILabel nameLabel;
    public UILabel levelLabel;
    public UIInput usernameInput_login;
    public UIInput passwordInput_login;
    public UIInput usernameInput_register;
    public UIInput passwordInput1_register;
    public UIInput passwordInput2_register;
    public UIInput nameInput;
    public UIButton confirmBtn;
    public UIButton backBtn;

    public static string username_login;
    public static string password_login;
    public static ServerInfomation info;
    public static List<Role> roleList;

    public UIGrid serverGrid;
    public GameObject serverBtn_red;
    public GameObject serverBtn_green;
    public GameObject server_select;
    public GameObject[] players;
    public GameObject[] players_select;
    public Transform player_parent;
    private GameObject player_select;

    public IsLogin isLogin = IsLogin.No;  //默认未登录
    public IsRoleExist isRoleExist = IsRoleExist.No;  //默认不存在角色
    public bool isMan = true;

    void Awake()
    {
        instance = this;
        loginController = GetComponent<LoginController>();
        registerController = GetComponent<RegisterController>();
        roleController = GetComponent<RoleController>();
        info = server_select.GetComponent<ServerInfomation>();

        roleController.OnAddRole += OnAddRole;
        roleController.OnGetRole += OnGetRole;
        roleController.OnSelectRole += OnSelectRole;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        roleController.OnAddRole -= OnAddRole;
        roleController.OnGetRole -= OnGetRole;
        roleController.OnSelectRole -= OnSelectRole;
    }

    public void OnGetRole(List<Role> roleList)
    {
        StartMenu.roleList = roleList;  //将roleList保存起来       
        if (roleList != null && roleList.Count > 0)  //已经存在角色，将来跳转到角色显示界面
        {
            //Debug.Log("Y: OnGetRole");
            PhotonEngine.Instance.role = roleList[0];  //将role保存起来
            isRoleExist = IsRoleExist.Yes;
        }
        else  //不存在角色，将来跳转到角色选择界面
        {
            //Debug.Log("N: OnGetRole");
            isRoleExist = IsRoleExist.No;
        }
    }

    public void OnAddRole(Role role)  //向服务器添加角色成功
    {
        if (roleList == null)
        {
            roleList = new List<Role>();
        }
        PhotonEngine.Instance.role = role;
        //Debug.Log("Y: OnAddRole  " + role.Name);
        roleList.Add(role);
        OnPlayerBackClick();  //返回角色界面
    }

    public void OnSelectRole()  //向服务器保存当前角色成功
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);  //异步加载第二个场景
        LoadGame.instance.Show(operation);  //此时由于panel作用，场景只剩下背景和主角
    }

    public void OnUsernameClick()  //跳转到登录界面
    {
        startTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(startTween.gameObject));
        loginTween.gameObject.SetActive(true);  //显示
        loginTween.PlayForward();
    }

    public void OnLoginClick()  //登录
    {
        //TODO，验证账号和密码
        username_login = usernameInput_login.value;
        password_login = passwordInput_login.value;

        loginController.Login(username_login, password_login);  //交给loginController处理
        //成功，返回开始界面
        //失败，提示信息
    }

    public void OnRegisterClick()  //注册
    {
        //TODO，记录账号和密码
        string username_register = usernameInput_register.value;
        string password1_register = passwordInput1_register.value;
        string password2_register = passwordInput2_register.value;
        if (username_register == null || username_register.Length < 3)  //账号不能为空或者少于3个字符
        {
            MessageManager.instance.ShowMessage("账号不能少于3个字符", 2f);
            return;
        }
        if (password1_register == null || password2_register.Length < 3)  //密码不能为空或者少于3个字符
        {
            MessageManager.instance.ShowMessage("密码不能少于3个字符", 2f);
            return;
        }
        if (password1_register != password2_register)  //两次输入的密码需要一致
        {
            MessageManager.instance.ShowMessage("两次输入的密码不一致", 2f);
            return;
        }
        registerController.Register(username_register, password1_register);  //交给registerController处理

        //成功，返回开始界面
        //失败，提示信息
    }

    public void OnLoginShowClick()  //跳转到登录界面
    {
        registerTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(registerTween.gameObject));
        loginTween.gameObject.SetActive(true);  //显示
        loginTween.PlayForward();
    }

    public void OnRegisterShowClick()  //跳转到注册界面
    {
        loginTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(loginTween.gameObject));
        registerTween.gameObject.SetActive(true);  //显示
        registerTween.PlayForward();
    }

    public void OnLoginCloseClick()  //关闭登录界面，显示开始界面
    {
        loginTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(loginTween.gameObject));
        startTween.gameObject.SetActive(true);  //显示
        startTween.PlayForward();
    }

    public void OnRegisterCloseClick()  //关闭注册界面，显示开始界面
    {
        registerTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(registerTween.gameObject));
        startTween.gameObject.SetActive(true);  //显示
        startTween.PlayForward();
    }

    public void OnServerClick()  //跳转到服务器选择界面
    {
        startTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(startTween.gameObject));
        serverTween.gameObject.SetActive(true);  //显示
        serverTween.PlayForward();
    }

    public void OnServerCloseClick()  //关闭服务器选择界面，显示开始界面
    {
        serverTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(serverTween.gameObject));
        startTween.gameObject.SetActive(true);  //显示
        startTween.PlayForward();
        serverLabel_start.text = info.serverName;  //更新服务器显示
    }

    public void OnEnterClick()  //跳转到游戏角色选择界面
    {
        if (isLogin == IsLogin.Yes)  //已经登陆
        {
            if (isRoleExist == IsRoleExist.Yes)  //已经存在角色
            {
                OnPlayerChangeClick(true);
            }
            else  //不存在角色
            {
                OnPlayerCreatClick();
            }
        }
        else  //没有登陆
        {
            MessageManager.instance.ShowMessage("玩家尚未登陆", 2f);
        }
    }

    public void OnServerSelector(GameObject go)  //点击选择服务器
    {
        info = go.GetComponent<ServerInfomation>();
        server_select.GetComponent<UISprite>().spriteName = go.GetComponent<UISprite>().spriteName;
        server_select.transform.Find("Label").GetComponent<UILabel>().text = info.serverName;
    }

    public void OnPlayerChangeClick(bool isNew = false)  //点击更换角色
    {
        if (isNew)  //刚进入游戏
        {
            backBtn.SetState(UIButtonColor.State.Disabled, false);  //确认按钮不可用
            backBtn.GetComponent<Collider>().enabled = false;
            startTween.PlayReverse();  //隐藏
            StartCoroutine(Hide(startTween.gameObject));

        }
        else  //重新选择角色
        {
            backBtn.SetState(UIButtonColor.State.Normal, true);  //确认按钮可用
            backBtn.GetComponent<Collider>().enabled = true;
            enterTween.PlayReverse();  //隐藏
            StartCoroutine(Hide(enterTween.gameObject));
        }
        playerCreatTween.gameObject.SetActive(true);  //显示
        playerCreatTween.PlayForward();
        nameInput.enabled = false;
        confirmBtn.SetState(UIButtonColor.State.Disabled, false);  //确认按钮不可用
        confirmBtn.GetComponent<Collider>().enabled = false;
    }

    public void OnPlayerCreatClick()  //跳转到游戏角色创建界面
    {
        startTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(startTween.gameObject));
        playerCreatTween.gameObject.SetActive(true);  //显示
        playerCreatTween.PlayForward();
        nameInput.enabled = true;
        backBtn.SetState(UIButtonColor.State.Disabled, false);  //确认按钮不可用
        backBtn.GetComponent<Collider>().enabled = false;
        confirmBtn.SetState(UIButtonColor.State.Normal, true);  //确认按钮可用
        confirmBtn.GetComponent<Collider>().enabled = true;
    }

    public void OnPlayerClick(GameObject go, bool isMan)  //点击选择角色
    {
        iTween.ScaleTo(go, new Vector3(1.5f, 1.5f, 1.5f), 0.2f);  //将选择的角色放大
        if (player_select != null && player_select != go)
        {
            iTween.ScaleTo(player_select, new Vector3(1f, 1f, 1f), 0.2f);  //其他角色正常显示
        }
        player_select = go;
        this.isMan = isMan;
        //判断当前选择的角色是否已经创建过
        Role roleTemp = null;
        if (roleList != null)
        {
            foreach (var role in roleList)
            {
                if ((role.IsMan && isMan) || (role.IsMan == false && isMan == false))
                {
                    //如果角色存在男性并且选择男性角色
                    //如果角色存在女性并且选择女性角色
                    //都不可以继续创建
                    roleTemp = role;
                }
            }
        }

        if (roleTemp == null)  //当前账号不存在角色
        {
            nameInput.value = "";
            nameInput.enabled = true;  //可以输入名字
            backBtn.SetState(UIButtonColor.State.Disabled, false);  //确认按钮不可用
            backBtn.GetComponent<Collider>().enabled = false;
            confirmBtn.SetState(UIButtonColor.State.Normal, true);  //按钮可用
            confirmBtn.GetComponent<Collider>().enabled = true;
        }
        else  //存在
        {
            PhotonEngine.Instance.role = roleTemp;
            nameInput.value = PhotonEngine.Instance.role.Name;
            nameInput.enabled = false;  //不可以输入名字
            backBtn.SetState(UIButtonColor.State.Normal, true);  //确认按钮可用
            backBtn.GetComponent<Collider>().enabled = true;
            confirmBtn.SetState(UIButtonColor.State.Disabled, true);  //按钮不可用
            confirmBtn.GetComponent<Collider>().enabled = false;
        }
    }

    public void OnPlayerConfirmClick()  //点击确认按钮
    {
        if (player_select == null)
        {
            MessageManager.instance.ShowMessage("请选择角色", 2f);
            return;
        }

        if (nameInput.value.Length < 3)  //名字不少于3个字符
        {
            MessageManager.instance.ShowMessage("名字不少于3个字符", 2f);
            return;
        }

        AddNewPlayer();  //添加初始化的角色
    }

    public void OnPlayerBackClick()  //点击返回按钮
    {
        playerCreatTween.PlayReverse();  //隐藏
        StartCoroutine(Hide(playerCreatTween.gameObject));
        enterTween.gameObject.SetActive(true);  //显示
        enterTween.PlayForward();
        ShowRole(PhotonEngine.Instance.role);
    }

    public void ShowRole(Role role)  //显示角色面板
    {
        nameLabel.text = PhotonEngine.Instance.role.Name;  //更新名称显示
        levelLabel.text = "Lv." + PhotonEngine.Instance.role.Level;  //更新等级显示
        if (PhotonEngine.Instance.role.IsMan)  //显示男性角色
        {
            players[0].SetActive(true);
            players[1].SetActive(false);
        }
        else  //显示女性角色
        {
            players[0].SetActive(false);
            players[1].SetActive(true);
        }
    }

    public void OnGameBtnClick()  //选择角色后点击进入游戏
    {
        roleController.SelectRole(PhotonEngine.Instance.role);  //保存当前角色
    }

    public void AddNewPlayer()  //初始化新角色
    {
        Role roleAdd = new Role();
        roleAdd.Name = nameInput.value;
        roleAdd.Level = 1;
        roleAdd.IsMan = isMan;
        roleAdd.Exp = 0;
        roleAdd.Coin = 1000;
        roleAdd.Diamond = 1000;
        roleAdd.Energy = 100;
        roleAdd.Toughen = 50;
        roleController.AddRole(roleAdd);
    }

    IEnumerator Hide(GameObject go)  //利用协程隐藏，防止隐藏过程中直接消失
    {
        yield return new WaitForSeconds(0.2f);
        go.SetActive(false);
    }
}
