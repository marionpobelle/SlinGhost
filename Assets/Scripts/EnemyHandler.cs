using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private GameData _gameData;

    private CrosshairController _crosshairController;
    //Contains the crosshair position updated the last time the slingshot was fired.
    private UnityEngine.Vector3 _crosshairPosition;
    private bool _isCrosshairOnEnemy;

    private int _currentHP;
    private float _currentScaleStep;

    private float _speedTick = 0;

    private void Awake()
    {
        _gameData = Data.GameData;
        transform.localScale = new UnityEngine.Vector3(_gameData.EnemyMinScale, _gameData.EnemyMinScale, _gameData.EnemyMinScale);
        _crosshairPosition = UnityEngine.Vector3.zero;
        _isCrosshairOnEnemy = false;
        _currentHP = _gameData.EnemyHP;
        _currentScaleStep = _gameData.EnemyScaleMinStep;
        if(this.gameObject.TryGetComponent<CapsuleCollider>(out var collider))
        {
            collider.radius = _gameData.ColliderRadius;
        }
        float randomCoordX = 0;// Random.Range(-_gameData.Xrange, _gameData.Xrange);
        float randomCoordY = 0;// Random.Range(-_gameData.Yrange, _gameData.Yrange);
        transform.position = new UnityEngine.Vector3(randomCoordY, randomCoordX, 0);
    }

    private void Start()
    {
        _crosshairController = FindObjectOfType<CrosshairController>();
        if(_crosshairController == null)
        {
            Debug.LogError("Couldn't find CrosshairController !", this);
        }
        _crosshairController.OnSlingshotFired += SlingshotFired;
    }

    private void FixedUpdate()
    {
        _speedTick += Time.deltaTime;
        //If tick reached, increase enemy speed
        if(_speedTick >= _gameData.TimeInterval && _currentScaleStep < _gameData.EnemyScaleMaxStep)
        {
            _speedTick = 0f;
            _currentScaleStep += _gameData.Increase;

        }
        //Change enemy scale according to speed
        transform.localScale = transform.localScale + new UnityEngine.Vector3(_currentScaleStep, _currentScaleStep, _currentScaleStep);
        //If enemy reached maximum scale, end the game
        if (transform.localScale.x >= _gameData.EnemyMaxScale)
        {
            GameHandler.Instance.LostGame();
        }
        //Left-right movement
        transform.position = new Vector3(_gameData.MovementCurve.Evaluate(_gameData.EnemySpeed * Time.time) * _gameData.EnemyMovementOffset, 0, transform.position.z);
    }

    private void SlingshotFired(UnityEngine.Vector3 crosshairPosition)
    {
        _crosshairPosition = crosshairPosition;
        //IF ENEMY HIT
        if (_isCrosshairOnEnemy)
        {
            _currentHP--;
            if(_currentHP <= 0)
            {
                GameHandler.Instance.DecreaseEnemyCount();
                Destroy(this.gameObject);
            }
        }
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<CrosshairController>(out var crosshair))
        {
            _isCrosshairOnEnemy = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<CrosshairController>(out var crosshair))
        {
            _isCrosshairOnEnemy = false;
        }
    }

    private void OnDestroy()
    {
        _crosshairController.OnSlingshotFired -= SlingshotFired;
    }
}
