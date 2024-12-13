using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip hurtSoundEffect;

    AudioSource audioSource;
    AudioSource constantAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // AudioSource[] audioSources = GetComponents<AudioSource>();
        // if (audioSources.Length < 2)
        // {
        //     Debug.LogError("Need two audios");
        //     return;
        // }

        // audioSource = audioSources[0];
        // constantAudioSource = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnemyAudio(string audio)
    {
        switch (audio)
        {
            case "hurt":
                StartCoroutine(PlayHitAudioWithDelay(0.25f));
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
                // clip = moveSoundEffect;
                constantAudioSource.loop = true;
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

    IEnumerator PlayHitAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(hurtSoundEffect);
    }
}