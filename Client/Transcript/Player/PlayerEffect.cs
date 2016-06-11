using UnityEngine;
using System.Collections;

public class PlayerEffect : MonoBehaviour
{
    public Renderer[] rArray;
    public NcCurveAnimation[] cArray;
    private GameObject effectOffset;

    // Use this for initialization
    void Start()
    {
        rArray = GetComponentsInChildren<Renderer>();
        cArray = GetComponentsInChildren<NcCurveAnimation>();
        if (transform.Find("EffectOffset"))
        {
            effectOffset = transform.Find("EffectOffset").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowEffect()
    {
        if (effectOffset == null)
        {
            foreach (Renderer renderer in rArray)
            {
                renderer.enabled = true;
            }
            foreach (NcCurveAnimation curve in cArray)
            {
                curve.ResetAnimation();
            }
        }
        else
        {
            effectOffset.SetActive(false);
            effectOffset.SetActive(true);
        }
    }
}
