using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class GameHandler : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private EnemyHandler _enemyPrefab;

    public int EnemyCount;
    public int EnemySpawnedCount;

    public static GameHandler Instance;
    private GameData _gameData;

    private ObjectPool<EnemyHandler> _enemyPool;
    private bool _usePool;

    private void Awake()
    {
        Instance = this;
        _gameData = Data.GameData;
        EnemySpawnedCount = 0;
        EnemyCount = 0;
        _usePool = true;

        _enemyPool = new ObjectPool<EnemyHandler>(() =>
        {
            //Create
            return Instantiate(_enemyPrefab);
        },
        enemy =>
        {
            //Get
            enemy.gameObject.SetActive(true);
            enemy.InitEnemy();
        },
        enemy =>
        {
            //Release
            enemy.gameObject.SetActive(false);
        },
        enemy =>
        {
            //Destroy
            Destroy(enemy.gameObject);
        },
        true,
        10,
        20
        );
    }

    private void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LostGame()
    {
        Debug.Log("Lost the game !");
    }

    public void WonGame()
    {
        Debug.Log("Won the game !");
    }

    public void DecreaseEnemyCount()
    {
        EnemyCount--;
        if (EnemyCount <= 0)
        {
            WonGame();
        }
    }

    public void IncreaseEnemyCount()
    {
        EnemyCount++;
    }

    private IEnumerator EnemySpawner()
    {
        float randomCoordX, randomCoordY;
        while (EnemySpawnedCount < _gameData.MaxAmountEnemies)
        {
            for (int i = 0; i < _gameData.enemySpawnAmount; i++)
            {
                if (_usePool)
                {
                    _enemyPool.Get();
                }
                else Instantiate(_enemyPrefab);
            }
            yield return new WaitForSeconds(Random.Range(_gameData.minSpawnTime, _gameData.maxSpawnTime));
        }
    }

}
