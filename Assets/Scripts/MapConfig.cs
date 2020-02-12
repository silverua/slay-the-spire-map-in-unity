using Malee;
using OneLine;
using UnityEngine;

[CreateAssetMenu]
public class MapConfig : ScriptableObject
{
    public int gridWidth = 8;
    [OneLineWithHeader]
    public IntMinMax numOfStartingNodes;
    [Reorderable]
    public ListOfMapLayers layers;
    
    [System.Serializable]
    public class ListOfMapLayers : ReorderableArray<MapLayer>
    {
    }
}
