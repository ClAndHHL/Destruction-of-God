using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] cArray;
    public AudioSource source;
    private Dictionary<string, AudioClip> mDict = new Dictionary<string, AudioClip>();
    public bool isQuiet = false;  //是否静音

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        foreach (AudioClip clip in cArray)
        {
            mDict.Add(clip.name, clip);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string audio)
    {
        if (isQuiet)
        {
            return;
        }
        AudioClip clip = null;
        if (mDict.TryGetValue(audio, out clip))
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlaySound(string audio, AudioSource source)
    {
        if (isQuiet)
        {
            return;
        }
        AudioClip clip = null;
        if (mDict.TryGetValue(audio, out clip))
        {
            source.PlayOneShot(clip);
        }
    }
}
