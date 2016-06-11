using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;
    private TweenAlpha tween;
    private UILabel messager;
    private bool isActive = false;

    void Awake()
    {
        instance = this;
        tween = GetComponent<TweenAlpha>();
        messager = transform.Find("Sprite/Label").GetComponent<UILabel>();
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

    }

    public void ShowMessage(string message, float time = 1f)
    {
        gameObject.SetActive(true);
        StartCoroutine(Hide(message, time));
    }

    IEnumerator Hide(string message, float time)
    {
        isActive = true;
        tween.PlayForward();
        messager.text = message;
        yield return new WaitForSeconds(time);
        tween.PlayReverse();
        isActive = false;
    }

    void OnTweenFinished()
    {
        if (isActive == false)
        {
            gameObject.SetActive(false);
        }
    }
}
