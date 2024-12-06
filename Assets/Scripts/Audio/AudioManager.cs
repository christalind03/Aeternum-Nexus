using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip titleScreenMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip buttonPressSound;

    AudioSource audioSource;
    string currentScene;

    void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        
        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioSource = audioManager.GetComponent<AudioSource>();
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newScene = scene.name;

        if (newScene != currentScene)
        {
            currentScene = newScene;
            PlayMusicForScene(newScene);
        }
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        if (sceneName == "Title Screen" || sceneName == "Level-Select")
        {
            clipToPlay = titleScreenMusic;
        }
        else if (sceneName == "Level1")
        {
            clipToPlay = level1Music;
        }
        else if (sceneName == "Level2")
        {
            clipToPlay = level2Music;
        }

        if (clipToPlay != null && audioSource.clip != clipToPlay)
        {
            audioSource.clip = clipToPlay;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
