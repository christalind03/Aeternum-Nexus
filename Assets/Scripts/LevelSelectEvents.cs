using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectEvents : MonoBehaviour
{
    private UIDocument main;

    public string Level1 = "Level1";
    public string Level1Boss = "Level1boss";
    public string Level2 = "Level2";
    public string Level2Boss = "Level2boss";

    public AudioClip buttonClickSound;
    AudioSource _audioSource;

    private Button topR;
    private Button topL;
    private Button botR;
    private Button botL;

    private void Awake()
    {
        main = GetComponent<UIDocument>();

        topR = main.rootVisualElement.Q("TopRight") as Button;
        topL = main.rootVisualElement.Q("TopLeft") as Button;
        botR = main.rootVisualElement.Q("BottomRight") as Button;
        botL = main.rootVisualElement.Q("BottomLeft") as Button;

        topR.RegisterCallback<ClickEvent>(OnTopRight);
        topL.RegisterCallback<ClickEvent>(OnTopLeft);
        botR.RegisterCallback<ClickEvent>(OnBottomRight);
        botL.RegisterCallback<ClickEvent>(OnBottomLeft);

        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            _audioSource = audioManager.GetComponent<AudioSource>();
        }
    }

    private void OnTopRight(ClickEvent click)
    { 
        {
            Debug.Log("Enter scene: Level1");
            PlayButtonSound();
            SceneManager.LoadScene(Level1);
        }
    }
    private void OnTopLeft(ClickEvent click)
    {
        {
            Debug.Log("Currently locked(level2)");
            PlayButtonSound();
            SceneManager.LoadScene(Level2);

        }
    }
    private void OnBottomRight(ClickEvent click)
    {
        {
            Debug.Log("Enter scene:LOCKEDLevel1-Boss");
            PlayButtonSound();
            SceneManager.LoadScene(Level1Boss);

        }
    }
    private void OnBottomLeft(ClickEvent click)
    {
        {
            Debug.Log("Currently locked(level2)");
            PlayButtonSound();
            SceneManager.LoadScene(Level2Boss);

        }
    }

    void PlayButtonSound()
    {
        if (buttonClickSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
