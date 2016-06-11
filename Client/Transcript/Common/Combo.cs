using UnityEngine;
using System.Collections;

public class Combo : MonoBehaviour
{
    public static Combo instance;
    public float comboTime = 2f;  //连击时间，在这个时间内攻击可以增加连击数
    private float comboTimer = 0f;
    private int comboNum = 0;
    private UILabel numLabel;

    void Awake()
    {
        instance = this;
        numLabel = transform.Find("Label").GetComponent<UILabel>();
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        comboTimer -= Time.deltaTime;
        if (comboTimer < 0f)
        {
            gameObject.SetActive(false);  //超过时间隐藏连击数
            comboNum = 0;  //连击数归零
        }
    }

    public void ShowCombo()  //增加连击数
    {
        gameObject.SetActive(true);
        comboTimer = comboTime;  //开始计时
        comboNum++;
        numLabel.text = comboNum + "";
        transform.localScale = Vector3.zero;
        iTween.ScaleTo(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.1f);  //使用itween产生放大效果
        iTween.ShakePosition(gameObject, new Vector3(0.2f, 0.2f, 0.2f), 0.1f);  //使用itween产生震动效果
    }


}
