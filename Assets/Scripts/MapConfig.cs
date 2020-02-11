using Malee;
using UnityEngine;

[CreateAssetMenu]
public class MapConfig : ScriptableObject
{
    [Reorderable]
    public ListOfMapLayers layers;
    
    [System.Serializable]
    public class ListOfMapLayers : ReorderableArray<MapLayer>
    {
    }
}
