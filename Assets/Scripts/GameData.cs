using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameData), menuName = "Slingshot/" + nameof(GameData))]
public class GameData : ScriptableObject
{
    [Header("Slingshot")]
    [Tooltip("The minimum time between shots.")]
    public float CooldownBetweenShotsInSeconds = 0.5f;
}