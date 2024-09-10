using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    private float _normalizedScaleY;

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
        AkSoundEngine.SetRTPCValue("NME_Scale", transform.localScale.y);
    }

    private void SlingshotFired(UnityEngine.Vector3 crosshairPosition)
    {
        _crosshairPosition = crosshairPosition;
        AkSoundEngine.PostEvent("SLG_Fire", gameObject);
        //IF ENEMY HIT
        if (_isCrosshairOnEnemy)
        {
            AkSoundEngine.PostEvent("NME_Hit", gameObject);
            _currentHP--;
            if(_currentHP <= 0)
            {
                AkSoundEngine.PostEvent("NME_Death", gameObject);
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
        float i = this.transform.position.y - _crosshairController.transform.position.y;
        return i;
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
            AkSoundEngine.PostEvent("NME_Zone_Enter", gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<CrosshairController>(out var crosshair))
        {
            _isCrosshairOnEnemy = false;
            AkSoundEngine.PostEvent("NME_Zone_Exit", gameObject);
        }
    }

    private void OnDestroy()
    {
        _crosshairController.OnSlingshotFired -= SlingshotFired;
    }
}
