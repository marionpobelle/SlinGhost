using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    // Marker class for the spawn point of the enemy

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
}