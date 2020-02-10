using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var myScript = (MapGenerator) target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate"))
            myScript.Generate();
    }
}
