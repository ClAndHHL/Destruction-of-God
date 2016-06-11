using UnityEngine;
using System.Collections;

public class FollowTarget2 : MonoBehaviour
{
    public Vector3 offset;
    private Transform player;
    public float smooth = 4f;  //缓动效果

    // Use this for initialization
    void Start()
    {
        player = TranscriptManager.instance.player.transform.Find("Bip01");  //跟随Bip是为了晃动效果
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smooth * Time.deltaTime);
    }
}
