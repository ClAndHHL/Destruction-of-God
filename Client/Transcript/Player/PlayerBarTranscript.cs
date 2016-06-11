using UnityEngine;
using System.Collections;

public class PlayerBarTranscript : MonoBehaviour
{
    private UISprite headSprite;
    private UIButton headBtn;
    private UILabel nameLabel;
    private UILabel levelLabel;
    private UILabel hpLabel;
    private UILabel energyLabel;
    private UISlider hpBar;
    private UISlider energyBar;
    private UIButton hpPlusBtn;
    private UIButton energyPlusBtn;

    void Awake()
    {
        headSprite = transform.Find("head_sprite").GetComponent<UISprite>();
        headBtn = transform.Find("head_sprite").GetComponent<UIButton>();
        nameLabel = transform.Find("name_label").GetComponent<UILabel>();
        levelLabel = transform.Find("level_label").GetComponent<UILabel>();
        hpLabel = transform.Find("hp_bar/hp").GetComponent<UILabel>();
        energyLabel = transform.Find("energy_bar/energy").GetComponent<UILabel>();
        hpBar = transform.Find("hp_bar").GetComponent<UISlider>();
        energyBar = transform.Find("energy_bar").GetComponent<UISlider>();
        hpPlusBtn = transform.Find("hp_plus").GetComponent<UIButton>();
        energyPlusBtn = transform.Find("energy_plus").GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        TranscriptManager.instance.player.GetComponent<PlayerAttack>().OnPlayerHpChange += OnPlayerHpChange;
        hpPlusBtn.SetState(UIButtonColor.State.Disabled, true);  //禁用按钮
        hpPlusBtn.GetComponent<Collider>().enabled = false;
        energyPlusBtn.SetState(UIButtonColor.State.Disabled, true);
        energyPlusBtn.GetComponent<Collider>().enabled = false;
        UpdateShow();  //更新显示
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))  //测试用，满血复活
        {
            TranscriptManager.instance.player.GetComponent<PlayerAttack>().Hp();
            PlayerInfomation info = PlayerInfomation.instance;
            hpLabel.text = info.Hp + "/" + info.Hp;
            hpBar.value = info.Hp / info.Hp;
        }
    }

    void OnDestroy()
    {
        //TranscriptManager.instance.player.GetComponent<PlayerAttack>().OnPlayerHpChange -= OnPlayerHpChange;
    }

    public void UpdateShow()
    {
        PlayerInfomation info = PlayerInfomation.instance;

        nameLabel.text = info.Name;
        headSprite.spriteName = info.Head;
        levelLabel.text = "Lv." + info.Level;
        hpLabel.text = info.Hp + "/" + info.Hp;
        hpBar.value = info.Hp / info.Hp;
        energyLabel.text = info.Energy + "/100";
        energyBar.value = info.Energy / 100f;
    }

    public void OnPlayerHpChange(int hp_now)  //血量改变时候调用
    {
        PlayerInfomation info = PlayerInfomation.instance;

        hpLabel.text = hp_now + "/" + info.Hp;
        hpBar.value = (float)hp_now / info.Hp;
    }
}
