using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreen : MonoBehaviour
{
    private UIDocument _uiDocument;

    private Button _playButton;
    private Button _settingsButton;
    private Button _exitButton;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        _playButton = _uiDocument.rootVisualElement.Q("PlayButton") as Button;
        _settingsButton = _uiDocument.rootVisualElement.Q("SettingsButton") as Button;
        _exitButton = _uiDocument.rootVisualElement.Q("ExitButton") as Button;

        _playButton.RegisterCallback<ClickEvent>(OnPlay);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettings);
        _exitButton.RegisterCallback<ClickEvent>(OnExit);
    }

    private void OnPlay(ClickEvent clickEvent)
    {
        Debug.Log("This should switch to the level select scene...");
    }

    private void OnSettings(ClickEvent clickEvent)
    {
        Debug.Log("This should switch to the settings scene...");
    }

    private void OnExit(ClickEvent clickEvent)
    {
        Debug.Log("APPLICATION TERMINATED.");
        Application.Quit();
    }
}
