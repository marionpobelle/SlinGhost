using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private int _enemyCount;
    public static GameHandler Instance;

    private GameData _gameData;

    private void Awake()
    {
        Instance = this;
        _gameData = Data.GameData;
    }

    private void Start()
    {
        
        _enemyCount = _gameData.MaxAmountEnemies;
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
        yield return new WaitForEndOfFrame();
    }

}
