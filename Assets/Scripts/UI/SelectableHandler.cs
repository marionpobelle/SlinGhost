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
        Debug.Log("Button selected : " + _selectableObject);
    }

    public enum SelectableObject
    {
        Play, //int = 0
        Settings, //int = 1
        Quit, // ...
        Audio,
        Close,
        Return,
        MasterVolume,
        SFXVolume,
        MusicVolume,
        UIVolume,
        TargetVolume
    }

}
