using UnityEngine;
using UnityEditor;

public class SnapBlocksToGrid : EditorWindow
{
    float gridSize = 0.001f;

    [MenuItem("Tools/Snap Blocks To Grid (0.001 Safe)")]
    public static void ShowWindow()
    {
        GetWindow<SnapBlocksToGrid>("Snap Blocks");
    }

    void OnGUI()
    {
        GUILayout.Label("Snap blocks to grid", EditorStyles.boldLabel);
        gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);

        if (GUILayout.Button("Snap All Blocks"))
        {
            SnapBlocks();
        }
    }

    void SnapBlocks()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject obj in allObjects)
        {
            Vector3 oldPos = obj.transform.position;
            Vector3 newPos = new Vector3(
                Mathf.Round(oldPos.x / gridSize) * gridSize,
                Mathf.Round(oldPos.y / gridSize) * gridSize,
                Mathf.Round(oldPos.z / gridSize) * gridSize
            );

            Undo.RecordObject(obj.transform, "Snap Block");
            obj.transform.position = newPos;

            Debug.Log($"{obj.name} snapped from {oldPos} to {newPos}");
        }
    }
}
