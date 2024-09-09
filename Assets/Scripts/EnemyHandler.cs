using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameData _gameData;

    private CrosshairController _crosshairController;

    //Contains the crosshair position updated the last time the slingshot was fired.
    private UnityEngine.Vector3 _crosshairPosition;

    private void Awake()
    {
        transform.localScale = new UnityEngine.Vector3(_gameData.EnemyMinScale, _gameData.EnemyMinScale, _gameData.EnemyMinScale);
        _crosshairPosition = UnityEngine.Vector3.zero;
    }

    private void Start()
    {
        _crosshairController = FindObjectOfType<CrosshairController>();
        _crosshairController.OnSlingshotFired += SlingshotFired;
    }

    private void SlingshotFired(UnityEngine.Vector3 crosshairPosition)
    {
        _crosshairPosition = crosshairPosition;
        //IF ENEMY HIT
        //Stuff
        //IF ENEMY MISSED
        //Move enemy
        //Check if game is ended
    }

    /// <summary>
    /// Gets the distance in between the crosshair and the enemy on the Y axis.
    /// If the distance is negative, the crosshair is above the enemy.
    /// If the distance is positive, the crosshair is below the enemy.
    /// </summary>
    public float GetNormalizedYDistance()
    {
        UnityEngine.Vector3 differences = (this.transform.position - _crosshairPosition).normalized;
        return differences.y;
    }

    /// <summary>
    /// Gets the distance in between the crosshair and the enemy in 2D space.
    /// </summary>
    public float GetDistance()
    {
        return UnityEngine.Vector2.Distance(_crosshairPosition, this.transform.position);
    }

    private void OnDestroy()
    {
        _crosshairController.OnSlingshotFired -= SlingshotFired;
    }
}
