using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour
{
    public static LoadGame instance;
    private UISlider loadBar;
    private AsyncOperation ao;
    private bool isAsync = false;

    void Awake()
    {
        instance = this;
        loadBar = transform.Find("load_bg/load_bar").GetComponent<UISlider>();
    }

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAsync)
        {
            loadBar.value = ao.progress;
        }
    }

    public void Show(AsyncOperation ao)  //显示加载界面
    {
        this.ao = ao;
        gameObject.SetActive(true);
        isAsync = true;
    }
}
