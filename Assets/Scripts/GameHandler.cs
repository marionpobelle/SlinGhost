using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AK.Wwise;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _enemyPrefab;

    public static GameHandler Instance;
    private GameData _gameData;
    public int EnemyCount;
    public AkAudioListener GameHandlerAudioListener;

    private void Awake()
    {
        Instance = this;
        _gameData = Data.GameData;
        EnemyCount = 0;
        _gameData.Score = 0;
        GameHandlerAudioListener = GetComponentInChildren<AkAudioListener>();
        if (GameHandlerAudioListener == null) Debug.LogError("Coudln't retrieve Game Handler Audiolistener !");
    }

    private void Start()
    {
        SpawnEnemy();
    }

    public void EndGame()
    {
        Debug.Log("Game ended !");
        AkSoundEngine.PostEvent("Win", gameObject);
        SceneManager.LoadScene("Menu");
    }

    public void SpawnEnemy()
    {
        Instantiate(_enemyPrefab);
        EnemyCount++;
        Debug.Log("We spawned " + EnemyCount + " ghosts in total at this point !");
    }

}
