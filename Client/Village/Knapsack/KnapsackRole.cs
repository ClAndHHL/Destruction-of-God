using UnityEngine;
using System.Collections;

public class KnapsackRole : MonoBehaviour
{
    private KnapsackEquip helmEquip;
    private KnapsackEquip clothEquip;
    private KnapsackEquip weaponEquip;
    private KnapsackEquip shoesEquip;
    private KnapsackEquip necklaceEquip;
    private KnapsackEquip braceletEquip;
    private KnapsackEquip ringEquip;
    private KnapsackEquip wingEquip;

    private UILabel hpLabel;
    private UILabel damageLabel;
    private UILabel expLabel;
    private UISlider expBar;

    void Awake()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged += OnPlayerInfoChanged;

        helmEquip = transform.Find("role_grid/helm_sprite").GetComponent<KnapsackEquip>();
        clothEquip = transform.Find("role_grid/cloth_sprite").GetComponent<KnapsackEquip>();
        weaponEquip = transform.Find("role_grid/weapon_sprite").GetComponent<KnapsackEquip>();
        shoesEquip = transform.Find("role_grid/shoes_sprite").GetComponent<KnapsackEquip>();
        necklaceEquip = transform.Find("role_grid/necklace_sprite").GetComponent<KnapsackEquip>();
        braceletEquip = transform.Find("role_grid/bracelet_sprite").GetComponent<KnapsackEquip>();
        ringEquip = transform.Find("role_grid/ring_sprite").GetComponent<KnapsackEquip>();
        wingEquip = transform.Find("role_grid/wing_sprite").GetComponent<KnapsackEquip>();

        hpLabel = transform.Find("role_info/hp_bg/hp").GetComponent<UILabel>();
        damageLabel = transform.Find("role_info/damage_bg/damage").GetComponent<UILabel>();
        expLabel = transform.Find("role_info/exp_bg/exp_bar/exp").GetComponent<UILabel>();
        expBar = transform.Find("role_info/exp_bg/exp_bar").GetComponent<UISlider>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Destroy()
    {
        PlayerInfomation.instance.OnPlayerInfoChanged -= OnPlayerInfoChanged;
    }

    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.All || type == InfoType.Hp || type == InfoType.Damage || type == InfoType.Exp || type == InfoType.Equip)
        {
            UpdateShow();
        }
    }

    public void UpdateShow()
    {
        PlayerInfomation info = PlayerInfomation.instance;
        helmEquip.SetItem(info._mHelm);
        clothEquip.SetItem(info._mCloth);
        weaponEquip.SetItem(info._mWeapon);
        shoesEquip.SetItem(info._mShoes);
        necklaceEquip.SetItem(info._mNecklace);
        braceletEquip.SetItem(info._mBracelet);
        ringEquip.SetItem(info._mRing);
        wingEquip.SetItem(info._mWing);

        hpLabel.text = info.Hp + "";
        damageLabel.text = info.Damage + "";
        expLabel.text = info.Exp + "/" + GameController.ExpByLevel(info.Level + 1);
        expBar.value = (float)info.Exp / GameController.ExpByLevel(info.Level + 1);
    }
}
