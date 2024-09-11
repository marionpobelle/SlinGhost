using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [Header("MAIN")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [Header("SETTINGS")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _closeButton;

    [Header("AUDIO")]
    [SerializeField] private GameObject _audioPanel;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _uiSlider;
    [SerializeField] private Slider _targetSlider;
    [SerializeField] private Button _audioReturnButton;

    [Header("NAVIGATION")]

    private GameObject _currentPanel;
    [SerializeField] private EventSystem _layoutEventSystem;

    private void Awake()
    {
        //MAIN
        _playButton.onClick.AddListener(OnPlayClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
        _quitButton.onClick.AddListener(OnQuitClicked);

        //SETTINGS
        _audioButton.onClick.AddListener(OnAudioClicked);
        _closeButton.onClick.AddListener(OnCloseClicked);

        //AUDIO
        _masterSlider.onValueChanged.AddListener(OnMasterChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        _uiSlider.onValueChanged.AddListener(OnUIChanged);
        _targetSlider.onValueChanged.AddListener(OnTargetChanged);
        _audioReturnButton.onClick.AddListener(OnAudioReturnClicked);

        _currentPanel = _mainPanel;
        _layoutEventSystem.SetSelectedGameObject(_playButton.gameObject);
        _mainPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _audioPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        //MAIN
        _playButton.onClick.RemoveListener(OnPlayClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        _quitButton.onClick.RemoveListener(OnQuitClicked);

        //SETTINGS
        _audioButton.onClick.RemoveListener(OnAudioClicked);
        _closeButton.onClick.RemoveListener(OnCloseClicked);

        //AUDIO
        _masterSlider.onValueChanged.RemoveListener(OnMasterChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
        _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        _uiSlider.onValueChanged.RemoveListener(OnUIChanged);
        _targetSlider.onValueChanged.RemoveListener(OnTargetChanged);
        _audioReturnButton.onClick.RemoveListener(OnAudioReturnClicked);
    }

    //MAIN
    private void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnSettingsClicked()
    {
        _currentPanel.SetActive(false);
        _currentPanel = _settingsPanel;
        _settingsPanel.SetActive(true);
        _layoutEventSystem.SetSelectedGameObject(_closeButton.gameObject);

    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }

    //SETTINGS

    private void OnAudioClicked()
    {
        _currentPanel.SetActive(false);
        _currentPanel = _audioPanel;
        _audioPanel.SetActive(true);
        _layoutEventSystem.SetSelectedGameObject(_audioReturnButton.gameObject);
    }

    private void OnCloseClicked()
    {
        _currentPanel.SetActive(false);
        _currentPanel = _mainPanel;
        _mainPanel.SetActive(true);
        _layoutEventSystem.SetSelectedGameObject(_settingsButton.gameObject);
    }

    //AUDIO
    private void OnMasterChanged(float arg0)
    {
        //TO DO : Change the master volume using _masterSlider.value (in between 0 and 1).
        AkSoundEngine.SetRTPCValue("Master_Level", arg0);
    }

    private void OnSFXChanged(float arg0)
    {
        //TO DO : Change the sfx volume using _sfxSlider.value (in between 0 and 1).
        AkSoundEngine.SetRTPCValue("SFX_Level", arg0);
    }

    private void OnMusicChanged(float arg0)
    {
        //TO DO : Change the music volume using _musicSlider.value (in between 0 and 1).
        AkSoundEngine.SetRTPCValue("Music_Level", arg0);
    }

    private void OnUIChanged(float arg0)
    {
        //TO DO : Change the ui volume using _uiSlider.value (in between 0 and 1).
        AkSoundEngine.SetRTPCValue("UI_Level", arg0);
    }

    private void OnTargetChanged(float arg0)
    {
        //TO DO : Change the target volume using _targetSlider.value (in between 0 and 1).
        AkSoundEngine.SetRTPCValue("Target_Level", arg0);
    }

    private void OnAudioReturnClicked()
    {
        _currentPanel.SetActive(false);
        _currentPanel = _settingsPanel;
        _settingsPanel.SetActive(true);
        _layoutEventSystem.SetSelectedGameObject(_audioButton.gameObject);
    }
}
