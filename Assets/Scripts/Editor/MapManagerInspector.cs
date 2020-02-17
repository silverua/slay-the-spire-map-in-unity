using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var myScript = (MapManager) target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate"))
            myScript.GenerateNewMap();
    }
}
