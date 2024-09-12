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
    private float _scaleStep;
    private float _maxTriggerScale;

    [SerializeField] private string _currentSize;
    public SphereCollider EnemyCollider;
    
    private EnemySpawnPoint _spawnPoint; // Only used for Z positioning of the enemy on spawn

    private void Awake()
    {
        _gameData = Data.GameData;
        transform.localScale = new UnityEngine.Vector3(_gameData.EnemyDefaultScale, _gameData.EnemyDefaultScale, _gameData.EnemyDefaultScale);
        _crosshairPosition = UnityEngine.Vector3.zero;
        _isCrosshairOnEnemy = false;
        _currentHP = _gameData.EnemyHP;
        _scaleStep = Random.Range(_gameData.EnemyMinScaleStep, _gameData.EnemyMaxScaleStep);
        _maxTriggerScale = Random.Range(_gameData.EnemyMinTriggerScale, _gameData.EnemyMaxTriggerScale);
        if (this.gameObject.TryGetComponent<SphereCollider>(out var collider))
        {
            collider.radius = _gameData.ColliderRadius;
            EnemyCollider = collider;
        }
        float randomCoordX =  Random.Range(-_gameData.Xrange, _gameData.Xrange);
        float randomCoordY =  Random.Range(-_gameData.Yrange, _gameData.Yrange);
        
        var enemySpawnPoint = FindObjectOfType<EnemySpawnPoint>();
        if (enemySpawnPoint)
        {
            transform.position = new UnityEngine.Vector3(randomCoordY, randomCoordX, enemySpawnPoint.transform.position.z);
        }
        else
        {
            transform.position = new UnityEngine.Vector3(randomCoordY, randomCoordX, 0);
        }
        
        
        //_timeOffset = Random.Range(0f, 100f);
    }

    private void Start()
    {
        _crosshairController = FindObjectOfType<CrosshairController>();
        if (_crosshairController == null)
        {
            Debug.LogError("Couldn't find CrosshairController !", this);
        }
        AkSoundEngine.PostEvent("NME_Spawn", gameObject);
    }

    private void FixedUpdate()
    {
        //Change enemy scale according to speed
        transform.localScale = transform.localScale + new UnityEngine.Vector3(_scaleStep, _scaleStep, _scaleStep);
        //If enemy reached maximum scale, end the game
        if (transform.localScale.x >= _maxTriggerScale)
        {
            GameHandler.Instance.EndGame();
        }
        _currentSize = (GetPercent(_maxTriggerScale, transform.localScale.x)).ToString();
        AkSoundEngine.SetRTPCValue("NME_Scale", transform.localScale.y);
        AkSoundEngine.SetRTPCValue("Elevation", transform.position.y - _crosshairController.transform.position.y);

        _crosshairController.UpdateDistanceValue(GetDistanceFromCrosshair());
        _crosshairController.UpdateCurrentScaleRatio(GetScaleRatio());
    }

    public void HitEnemy()
    {
        Debug.LogWarning("ENEMY HIT");
        _currentHP--;
        if(-_currentHP > 0)
        {
             AkSoundEngine.PostEvent("NME_Hit", gameObject);
        }
        else if (_currentHP <= 0)
        {
             _gameData.Score++;
             AkSoundEngine.PostEvent("NME_Death", gameObject);
             GameHandler.Instance.StartCoroutine(GameHandler.Instance.SpawnEnemy());
             Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Gets the distance in between the crosshair and the enemy in 2D space.
    /// </summary>
    public float GetDistanceFromCrosshair()
    {
        return Vector3.Distance(_crosshairController.transform.position, transform.position);
    }

    public float GetScaleRatio()
    {
        //Debug.Log(Mathf.InverseLerp(.1f, _maxTriggerScale, transform.localScale.x));
        return Mathf.InverseLerp(.1f, _maxTriggerScale, transform.localScale.x);
    }

    private float GetPercent(float maxValue, float currentValue)
    {
        return 100f * currentValue / maxValue;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<CrosshairController>(out var crosshair))
        {
            _isCrosshairOnEnemy = true;
            crosshair.AddEnemyToPotentialLockList(this);
            AkSoundEngine.PostEvent("NME_Zone_Enter", gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<CrosshairController>(out var crosshair))
        {
            _isCrosshairOnEnemy = false;
            crosshair.RemoveEnemyFromPotentialLockList(this);
            AkSoundEngine.PostEvent("NME_Zone_Exit", gameObject);
        }
    }

    private void OnDestroy()
    {
        _crosshairController.RemoveEnemyFromPotentialLockList(this);
    }
}
