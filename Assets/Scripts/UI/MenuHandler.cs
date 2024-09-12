using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class MenuHandler : MonoBehaviour
{
    [Header("MAIN")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("SETTINGS")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Toggle _voiceToggle;

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
        _scoreText.text = (Data.GameData.Score).ToString();

        //SETTINGS
        _audioButton.onClick.AddListener(OnAudioClicked);
        _closeButton.onClick.AddListener(OnCloseClicked);
        _voiceToggle.onValueChanged.AddListener((value) => OnVoiceChanged(_voiceToggle));

        //AUDIO
        _masterSlider.onValueChanged.AddListener(OnMasterChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        _uiSlider.onValueChanged.AddListener(OnUIChanged);
        _targetSlider.onValueChanged.AddListener(OnTargetChanged);
        _audioReturnButton.onClick.AddListener(OnAudioReturnClicked);

        //SETUP
        _currentPanel = _mainPanel;
        _layoutEventSystem.SetSelectedGameObject(_playButton.gameObject);
        _mainPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _audioPanel.SetActive(false);

        if (_voiceToggle.isOn); //TO DO : Turn on voice over mode
        else if (!_voiceToggle.isOn); //TO DO : Turn off voice over mode
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
        _voiceToggle.onValueChanged.RemoveListener((value) => OnVoiceChanged(_voiceToggle));

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

    private void OnVoiceChanged(bool value)
    {
        if (_voiceToggle.isOn)
        {
            AkSoundEngine.SetRTPCValue("AD_Toggle", 1);
        }
        else
        {
            AkSoundEngine.SetRTPCValue("AD_Toggle", 0);
        }
    }

    //AUDIO
    private void OnMasterChanged(float arg0)
    {
        AkSoundEngine.SetRTPCValue("Master_Level", _masterSlider.value);
    }

    private void OnSFXChanged(float arg0)
    {
        AkSoundEngine.SetRTPCValue("SFX_Level", _sfxSlider.value);
    }

    private void OnMusicChanged(float arg0)
    {
        AkSoundEngine.SetRTPCValue("Music_Level", _musicSlider.value);
    }

    private void OnUIChanged(float arg0)
    {
        AkSoundEngine.SetRTPCValue("UI_Level", _uiSlider.value);
    }

    private void OnTargetChanged(float arg0)
    {
        AkSoundEngine.SetRTPCValue("Target_Level", _targetSlider.value);
    }

    private void OnAudioReturnClicked()
    {
        _currentPanel.SetActive(false);
        _currentPanel = _settingsPanel;
        _settingsPanel.SetActive(true);
        _layoutEventSystem.SetSelectedGameObject(_audioButton.gameObject);
    }
}
