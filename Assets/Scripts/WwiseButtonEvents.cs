using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseButtonEvents : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event[] _events;

    public void PlayWwiseEvent(int i)
    {
        _events[i].Post(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
