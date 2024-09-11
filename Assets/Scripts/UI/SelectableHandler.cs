using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableHandler : MonoBehaviour, ISelectHandler
{
    [SerializeField] private SelectableObject _selectableObject;

    public void OnSelect(BaseEventData eventData)
    {
        //AUDIO
        //Play sounds depending on what is the _selectableObject : name or int
        AkSoundEngine.SetSwitch("ButtonType",_selectableObject.ToString(), gameObject);
        AkSoundEngine.PostEvent("OnSelect", gameObject);
    }

    public enum SelectableObject
    {
        Play, //int = 0
        Settings, //int = 1
        Quit, // ...
        Difficulty,
        Audio,
        Close,
        Return,
        DifficultySlider,
        WheelchairMode,
        MasterVolume,
        SFXVolume,
        MusicVolume,
        UIVolume,
        TargetVolume
    }

}
