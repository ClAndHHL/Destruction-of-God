using UnityEngine;
using System.Collections;

public class BossEffect : MonoBehaviour
{
    public float hideTime = 1f;
    public float hideTimer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hideTimer += Time.deltaTime;
        if (hideTimer >= hideTime)
        {
            gameObject.SetActive(false);
            hideTimer = 0f;
        }
    }
}
