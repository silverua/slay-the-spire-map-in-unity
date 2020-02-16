using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapView))]
public class MapViewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var myScript = (MapView) target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate"))
            myScript.Generate();
    }
}
