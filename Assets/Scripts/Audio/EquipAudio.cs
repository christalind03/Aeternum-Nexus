using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAudio : MonoBehaviour
{
    public AudioClip swordEquip;
    public AudioClip gunEquip;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEquipAudio(string audio, bool allowPlay)
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.loop = false;
        AudioClip clip = null;
        switch (audio)
        {
            case "sword":
                clip = swordEquip;
                break;
            case "gun":
                clip = gunEquip;
                break;
        }

        if (clip == null)
        {
            return;
        }

        if (allowPlay)
        {
            if (audioSource.clip != clip) // prevent restarting same clip
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null; // prevent accidental restarts
            }
        }
    }
}