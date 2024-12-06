using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    public AudioClip meleeSwing;
    public AudioClip meleeHit;
    public AudioClip gunShot;
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

    public void PlayWeaponAudio(string audio)
    {
        audioSource.time = 0f;
        switch (audio)
        {
            case "meleeSwing":
                audioSource.PlayOneShot(meleeSwing);
                break;

            case "meleeHit":
                audioSource.PlayOneShot(meleeHit);
                break;

            case "gunShot":
                audioSource.PlayOneShot(gunShot);
                break;
        }
    }
}