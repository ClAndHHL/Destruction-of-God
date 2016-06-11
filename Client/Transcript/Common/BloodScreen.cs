using UnityEngine;
using System.Collections;

public class BloodScreen : MonoBehaviour
{
    public static BloodScreen instance;
    private UISprite blood;
    private TweenAlpha tween;

    void Awake()
    {
        instance = this;
        blood = GetComponent<UISprite>();
        tween = GetComponent<TweenAlpha>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        blood.alpha = 1;
        tween.ResetToBeginning();
        tween.PlayForward();
    }
}
