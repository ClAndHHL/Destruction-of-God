using UnityEngine;
using System.Collections;

public class SkillItem : MonoBehaviour
{
    public PosType posType;
    private Skill skill;
    private UISprite skillIcon;
    private UIButton btnIcon;

    void Awake()
    {
        skillIcon = GetComponent<UISprite>();
        btnIcon = GetComponent<UIButton>();
    }

    // Use this for initialization
    void Start()
    {
        SkillManager.instance.OnSyncSkillFinished += OnSyncSkillFinished;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        SkillManager.instance.OnSyncSkillFinished -= OnSyncSkillFinished;
    }

    public void OnSyncSkillFinished()
    {
        UpdateShow();
    }

    void UpdateShow()
    {
        skill = SkillManager.instance.GetSkillByPosition(posType);
        skillIcon.spriteName = skill.Icon;
        btnIcon.normalSprite = skill.Icon;  //按钮的normal状态
    }

    void OnClick()
    {
        transform.parent.parent.SendMessage("OnSkillBtnClick", skill);
    }
}
