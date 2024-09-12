using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyHandler))]
public class EnemyEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyHandler enemy = (EnemyHandler)target;
        Handles.color = Color.red;
        Handles.DrawWireCube(Vector3.zero, new Vector3(Data.GameData.Xrange*2, Data.GameData.Yrange*2, 0));

        Handles.color = Color.magenta;
        Handles.DrawWireArc(enemy.transform.position, Vector3.forward, Vector3.up, 360, enemy.EnemyCollider.radius*enemy.transform.localScale.x);
    }
}
