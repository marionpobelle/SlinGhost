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
    [Tooltip("Enemy starting scale (the smallest the furthest, the biggest the closest.")]
    public float EnemyMinScale = 1f;
    public float EnemyMaxScale = 5f;
    [Tooltip("By how much the enemy moves each step default value.")]
    public float EnemyScaleMinStep = 1f;
    [Tooltip("By how much the enemy moves each step max value.")]
    public float EnemyScaleMaxStep = 1.5f;

    [Tooltip("Movement curve for the enemy position over time.")]
    public AnimationCurve MovementCurve;
    [Tooltip("Default enemy movement speed.")]
    public float EnemySpeed = 6f;
    [Tooltip("Axis offset value from default enemy position for left to right movement.")]
    public float EnemyMovementOffset = 1f;

    [Tooltip("Enemy health points default.")]
    public int EnemyHP = 1;
    [Tooltip("Enemy capsule collider radius.")]
    public float ColliderRadius = 1.2f;

    [Header("Difficulty progression")]
    [Tooltip("Amount of time until the difficulty increases.")]
    public float TimeInterval = 10f;
    [Tooltip("Amount by which the speed increases.")]
    public float Increase = 0.2f;

    [Header("Spawner")]
    [Tooltip("Max amount of enemies to spawn.")]
    public int MaxAmountEnemies = 5;
    [Tooltip("Amount of enemies to spawn per wave.")]
    public int enemySpawnAmount = 1;
    [Tooltip("Minimum time in between enemies spawning.")]
    public float minSpawnTime = 10f;
    [Tooltip("Maximum time in between enemies spawning.")]
    public float maxSpawnTime = 15f;
    [Tooltip("X axis range from 0 for enemy to spawn.")]
    public float Xrange = 1000f;
    [Tooltip("Y axis range from 0 for enemy to spawn.")]
    public float Yrange = 1000f;

    [Header("SCORE")]
    [Tooltip("Last registered score this session.")]
    public int Score = 0;
    [Tooltip("Last registered highscore this session.")]
    public int Highscore = 0;

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