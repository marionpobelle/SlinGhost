using UnityEngine;
using Event = AK.Wwise.Event;

public class WwiseButtonEvents : MonoBehaviour
{
    [SerializeField] Event[] _events;

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
