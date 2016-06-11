using UnityEngine;
using System.Collections;

public class PowerMessage : MonoBehaviour
{
    public static PowerMessage instance;
    private TweenAlpha tween;

    private int startValue = 0;
    private int endValue = 0;
    private bool isStart = false;
    private bool isUp = false;
    private UILabel label;
    public int speed = 500;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenAlpha>();
        label = transform.Find("Sprite/Label").GetComponent<UILabel>();
    }

    // Use this for initialization
    void Start()
    {
        EventDelegate ed = new EventDelegate(this, "OnTweenFinished");
        tween.onFinished.Add(ed);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (isUp)  //增加属性
            {
                startValue += (int)(speed * Time.deltaTime);
                if (startValue > endValue)
                {
                    startValue = endValue;
                    isStart = false;
                    tween.PlayReverse();
                }
            }
            else  //减少属性
            {
                startValue -= (int)(speed * Time.deltaTime);
                if (startValue < endValue)
                {
                    startValue = endValue;
                    isStart = false;
                    tween.PlayReverse();
                }
            }
            label.text = "战斗力 " + startValue;
        }
    }

    public void ShowPowerMessage(int startValue, int endValue)
    {
        gameObject.SetActive(true);
        tween.PlayForward();

        this.startValue = startValue;
        this.endValue = endValue;
        if (startValue < endValue)
        {
            isUp = true;
        }
        else
        {
            isUp = false;
        }
        isStart = true;
    }

    void OnTweenFinished()
    {
        if (isStart == false)
        {
            gameObject.SetActive(false);
        }
    }
}
