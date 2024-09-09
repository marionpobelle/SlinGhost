using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private int _enemyCount;
    public static GameHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        List<EnemyHandler> enemies = new List<EnemyHandler>();
        enemies = FindObjectsOfType<EnemyHandler>().ToList();
        if (enemies.Count == 0)
        {
            Debug.LogError("No enemies were found in the scene !", this);
        }
        _enemyCount = enemies.Count;
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

}
