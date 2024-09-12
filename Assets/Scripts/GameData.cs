using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameData), menuName = "Slingshot/" + nameof(GameData))]
public class GameData : ScriptableObject
{
    [Header("Slingshot")]
    [Tooltip("The minimum time between shots.")]
    public float CooldownBetweenShotsInSeconds = 0.5f;
    [Tooltip("The amount of time in seconds the crosshair needs to be on the crosshair to be considered as locked")]
    public float lockOnDelay = 0.1f;
    [Tooltip("The amount of time in seconds needed to unlock an enemy when the crossair is not on it anymore")]
    public float delockDelay = 0.7f;

    [Header("Enemy")]
    [Tooltip("Enemy starting scale (the smallest the furthest, the biggest the closest).")]
    public float EnemyDefaultScale = 1f;
    [Tooltip("Enemy trigger scale range for random value (the smallest the furthest, the biggest the closest).")]
    public float EnemyMinTriggerScale = 3f;
    public float EnemyMaxTriggerScale = 5f;
    [Tooltip("By how much the enemy moves each step range for random value.")]
    public float EnemyMinScaleStep = 1f;
    public float EnemyMaxScaleStep = 2f;

    [Tooltip("Enemy health points default.")]
    public int EnemyHP = 1;
    [Tooltip("Enemy capsule collider radius.")]
    public float ColliderRadius = 1.2f;

    [Header("Spawner")]
    [Tooltip("X axis range from 0 for enemy to spawn.")]
    public float Xrange = 1000f;
    [Tooltip("Y axis range from 0 for enemy to spawn.")]
    public float Yrange = 1000f;
    [Tooltip("Spawn delay in between enemies spawning.")]
    public float SpawnDelay = 2f;

    [Header("SCORE")]
    [Tooltip("Last registered score this session.")]
    public int Score = 0;

}


// Create a static getter that load GameData from Resources
// Usage : Data.GameData (from anywhere in the project)
public static class Data
{
    private static GameData _gameData;

    static Data()
    {
        Application.quitting += OnApplicationQuit;
    }

    public static GameData GameData
    {
        get
        {
            if (!_gameData) _gameData = Resources.Load<GameData>(nameof(GameData));

            return _gameData;
        }
    }

    private static void OnApplicationQuit()
    {
        _gameData = null;
    }
}