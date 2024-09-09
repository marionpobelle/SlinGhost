using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemyHandler enemyHandler;
    private float elevation;
    void Start()
    {
        enemyHandler = GetComponent<EnemyHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        elevation = enemyHandler.GetNormalizedYDistance();
        Debug.Log(elevation);
        AkSoundEngine.SetRTPCValue("Elevation", elevation);
    }
}
