using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    AudioSource audioSource;
    public AudioClip buttonClickSound;
    AudioSource _audioSource;

    public string LevelSelect = "Level-Select";

    private Button _playButton;
    private Button _settingsButton;
    private Button _exitButton;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        _playButton = _uiDocument.rootVisualElement.Q("PlayButton") as Button;
        _exitButton = _uiDocument.rootVisualElement.Q("ExitButton") as Button;

        _playButton.RegisterCallback<ClickEvent>(OnPlay);
        _exitButton.RegisterCallback<ClickEvent>(OnExit);

        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            _audioSource = audioManager.GetComponent<AudioSource>();
        }
    }

    private void OnPlay(ClickEvent clickEvent)
    {
        Debug.Log("This should switch to the level select scene...");
        PlayButtonSound();
        SceneManager.LoadScene(LevelSelect);
    }

    private void OnExit(ClickEvent clickEvent)
    {
        Debug.Log("APPLICATION TERMINATED.");
        PlayButtonSound();
        Application.Quit();
    }

    void PlayButtonSound()
    {
        if (buttonClickSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
