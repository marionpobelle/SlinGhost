using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableHandler : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private SelectableObject _selectableObject;

    public void OnSelect(BaseEventData eventData)
    {
        //AUDIO
        //Play sounds depending on what is the _selectableObject : name or int
        Debug.Log("Button selected : " + _selectableObject);
        AkSoundEngine.SetSwitch("ButtonType",_selectableObject.ToString(), gameObject);
        AkSoundEngine.PostEvent("OnSelect", gameObject);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log("Cursor Entering " + name + " GameObject");
        AkSoundEngine.SetSwitch("ButtonType", _selectableObject.ToString(), gameObject);
        AkSoundEngine.PostEvent("OnSelect", gameObject);
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
        TargetVolume,
        DifficultySlider,
        ScreenshakeToggle
    }

}
