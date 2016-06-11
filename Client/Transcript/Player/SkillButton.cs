using UnityEngine;
using System.Collections;

public class SkillButton : MonoBehaviour
{
    public PosType posType;
    private PlayerAnimation2 pa;
    private UISprite mask;
    private float coldTime = 4f;
    private float coldTimer = 0f;

    void Awake()
    {
        if (transform.Find("mask"))
        {
            mask = transform.Find("mask").GetComponent<UISprite>();
        }
    }

    // Use this for initialization
    void Start()
    {
        pa = TranscriptManager.instance.player.GetComponent<PlayerAnimation2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mask == null)
        {
            return;
        }
        if (coldTimer > 0f)
        {
            coldTimer -= Time.deltaTime;
            mask.fillAmount = coldTimer / coldTime;  //更新mask的进度
        }
        else
        {
            mask.fillAmount = 0;
            EnableBtn();
        }
    }

    void OnPress(bool isPress)
    {
        pa.OnAttackBtnClick(isPress, posType);
        if (isPress && mask != null)  //按下技能键1,2,3，冷却计时
        {
            coldTimer = coldTime;
            DisableBtn();
        }
    }

    void DisableBtn()
    {
        GetComponent<Collider>().enabled = false;
    }

    void EnableBtn()
    {
        GetComponent<Collider>().enabled = true;
    }
}
