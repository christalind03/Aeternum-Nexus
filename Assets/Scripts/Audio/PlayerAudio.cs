using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip hurtSoundEffect;
    public AudioClip jumpSoundEffect;
    public AudioClip landSoundEffect;
    public AudioClip dashSoundEffect;
    public AudioClip moveSoundEffect;
    public AudioClip slideSoundEffect;
    AudioSource audioSource;
    AudioSource constantAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // get both AudioSources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            Debug.LogError("Need two audios");
            return;
        }

        audioSource = audioSources[0];
        constantAudioSource = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPlayerAudio(string audio)
    {
        switch (audio)
        {
            case "hurt":
                audioSource.PlayOneShot(hurtSoundEffect);
                break;

            case "jump":
                audioSource.PlayOneShot(jumpSoundEffect);
                break;

            case "land":
                audioSource.PlayOneShot(landSoundEffect);
                break;
                
            case "dash":
                audioSource.PlayOneShot(dashSoundEffect);
                break;
        }
    }

    public void LoopAudio(string audio, bool allowPlay)
    {
        if (constantAudioSource == null)
        {
            return;
        }

        AudioClip clip = null;
        switch (audio)
        {
            case "move":
                clip = moveSoundEffect;
                constantAudioSource.loop = true;
                break;
            case "slide":
                clip = slideSoundEffect;
                constantAudioSource.loop = false; // don't want slide effect looping
                break;
        }

        if (clip == null)
        {
            return;
        }

        if (allowPlay)
        {
            if (constantAudioSource.clip != clip) // prevent restarting same clip
            {
                constantAudioSource.clip = clip;
                constantAudioSource.Play();
            }
        }
        else
        {
            if (constantAudioSource.isPlaying)
            {
                constantAudioSource.Stop();
                constantAudioSource.clip = null; // prevent accidental restarts
            }
        }
    }
}