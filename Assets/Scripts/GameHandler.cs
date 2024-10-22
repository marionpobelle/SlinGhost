using System.Collections;
using UnityEngine;
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
        StartCoroutine(SpawnEnemy());
    }

    public void EndGame()
    {
        Debug.Log("Game ended !");
        AkSoundEngine.PostEvent("Win", gameObject);
        SceneManager.LoadScene("Menu");
    }

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_gameData.SpawnDelay);
        Instantiate(_enemyPrefab);
        EnemyCount++;
        Debug.Log("We spawned " + EnemyCount + " ghosts in total at this point !");
    }

}
