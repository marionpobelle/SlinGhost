using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameData), menuName = "Slingshot/" + nameof(GameData))]
public class GameData : ScriptableObject
{
    [Header("Slingshot")]
    [Tooltip("The minimum time between shots.")]
    public float CooldownBetweenShotsInSeconds = 0.5f;

    [Header("Enemy")]
    [Tooltip("Enemy starting scale (the smallest the furthest, the biggest the closest.")]
    public float EnemyMinScale = 1f;
    public float EnemyMaxScale = 5f;
    [Tooltip("By how much the enemy moves each step default value.")]
    public float EnemyScaleMinStep = 1f;
    [Tooltip("By how much the enemy moves each step max value.")]
    public float EnemyScaleMaxStep = 1.5f;

    [Tooltip("Enemy health points default.")]
    public int EnemyHP = 1;
    [Tooltip("Enemy capsule collider radius.")]
    public float ColliderSize = 4.5f;

    [Header("Difficulty progression")]
    [Tooltip("Amount of time until the difficulty increases.")]
    public float TimeInterval = 10f;
    [Tooltip("Amount by which the speed increases.")]
    public float Increase = 0.2f;
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