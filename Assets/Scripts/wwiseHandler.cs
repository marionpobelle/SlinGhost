using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wwiseHandler : MonoBehaviour
{
    private GameData _gameData;
    [SerializeField] private EnemyHandler _enemyHandler;
    private float _enemyScale;
    private float _enemyElevation;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _enemyElevation = _enemyHandler.GetNormalizedYDistance();
        AkSoundEngine.SetRTPCValue("Elevation", _enemyElevation);
    }
}