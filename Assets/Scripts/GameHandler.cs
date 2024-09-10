using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _enemyPrefab;

    private int _enemyCount;
    private int _enemySpawnedCount;

    public static GameHandler Instance;
    private GameData _gameData;

    private void Awake()
    {
        Instance = this;
        _gameData = Data.GameData;
        _enemySpawnedCount = 0;
        _enemyCount = 0;
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
        _enemyCount--;
        if (_enemyCount <= 0)
        {
            WonGame();
        }
    }

    public void IncreaseEnemyCount()
    {
        _enemyCount++;
    }

    private IEnumerator EnemySpawner()
    {
        float randomCoordX, randomCoordY;
        while (_enemySpawnedCount < _gameData.MaxAmountEnemies)
        {
            for (int i = 0; i < _gameData.enemySpawnAmount; i++)
            {
                randomCoordX = Random.Range(-_gameData.Xrange, _gameData.Xrange);
                randomCoordY = Random.Range(-_gameData.Yrange, _gameData.Yrange);
                Instantiate(_enemyPrefab, new Vector3(randomCoordX, randomCoordY, 0), Quaternion.identity);
                _enemyCount++;
                _enemySpawnedCount++;
            }
            yield return new WaitForSeconds(Random.Range(_gameData.minSpawnTime, _gameData.maxSpawnTime));
        }
    }

}
