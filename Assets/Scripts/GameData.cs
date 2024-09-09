using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameData), menuName = "Slingshot/" + nameof(GameData))]
public class GameData : ScriptableObject
{
    [Header("Slingshot")]
    [Tooltip("The minimum time between shots.")]
    public float CooldownBetweenShotsInSeconds = 0.5f;
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