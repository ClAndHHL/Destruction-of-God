using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class System_ : MonoBehaviour
{
    public static System_ instance;
    private TweenPosition tween;
    private UIButton audioBtn;
    private UILabel audioLabel;
    private AudioSource audioSource;
    private bool isOpen = true;  //默认开启音效

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenPosition>();
        audioBtn = transform.Find("audio_btn").GetComponent<UIButton>();
        audioLabel = transform.Find("audio_label").GetComponent<UILabel>();
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAudioClick()  //点击切换音效
    {
        if (isOpen)  //关闭音效
        {
            isOpen = false;
            audioBtn.normalSprite = "pic_音效关闭";
            audioLabel.text = "音效关闭";
            audioLabel.color = Color.red;
            audioSource.Stop();
        }
        else  //开启音效
        {
            isOpen = true;
            audioBtn.normalSprite = "pic_音效开启";
            audioLabel.text = "音效开启";
            audioLabel.color = Color.green;
            audioSource.Play();
        }
    }

    public void OnChangeClick()  //更换角色
    {
        Destroy(PhotonEngine.Instance.gameObject);  //销毁PhotonEngine
        AsyncOperation ao = SceneManager.LoadSceneAsync(0);
        LoadGame.instance.Show(ao);
    }

    public void OnExitClick()  //退出游戏
    {
        Application.Quit();
    }

    public void OnChatClick()  //点击联系我们
    {
        Application.OpenURL("http://www.baidu.com");
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }
}
