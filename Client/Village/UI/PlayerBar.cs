using UnityEngine;
using System.Collections;

public class PlayerBar : MonoBehaviour
{
    private UISprite headSprite;
    private UIButton headBtn;
    private UILabel nameLabel;
    private UILabel levelLabel;
    private UILabel energyLabel;
    private UILabel toughenLabel;
    private UISlider energyBar;
    private UISlider toughenBar;
    private UIButton energyPlusBtn;
    private UIButton toughenPlusBtn;

    void Awake()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged += OnPlayerInfoChanged;

        headSprite = transform.Find("head_sprite").GetComponent<UISprite>();
        headBtn = transform.Find("head_sprite").GetComponent<UIButton>();
        nameLabel = transform.Find("name_label").GetComponent<UILabel>();
        levelLabel = transform.Find("level_label").GetComponent<UILabel>();
        energyLabel = transform.Find("energy_bar/energy").GetComponent<UILabel>();
        toughenLabel = transform.Find("toughen_bar/toughen").GetComponent<UILabel>();
        energyBar = transform.Find("energy_bar").GetComponent<UISlider>();
        toughenBar = transform.Find("toughen_bar").GetComponent<UISlider>();
        energyPlusBtn = transform.Find("energy_plus").GetComponent<UIButton>();
        toughenPlusBtn = transform.Find("toughen_plus").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed = new EventDelegate(this, "OnHeadClick");
        headBtn.onClick.Add(ed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged -= OnPlayerInfoChanged;
    }

    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.All || type == InfoType.Name || type == InfoType.Head || type == InfoType.Level || type == InfoType.Energy || type == InfoType.Toughen)
        {
            UpdateShow();
        }
    }

    public void UpdateShow()
    {
        PlayerInfomation info = PlayerInfomation.instance;

        nameLabel.text = info.Name;
        headSprite.spriteName = info.Head;
        levelLabel.text = "Lv." + info.Level;
        energyLabel.text = info.Energy + "/100";
        energyBar.value = info.Energy / 100f;
        toughenLabel.text = info.Toughen + "/50";
        toughenBar.value = info.Toughen / 50f;
    }

    public void OnHeadClick()  //点击头像查看信息按钮
    {
        headBtn.normalSprite = PlayerInfomation.instance.Head;
        PlayerStatus.instance.Show();
    }
}
