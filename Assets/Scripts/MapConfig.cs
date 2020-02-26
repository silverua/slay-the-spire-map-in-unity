using System.Collections.Generic;
using Malee;
using OneLine;
using UnityEngine;

[CreateAssetMenu]
public class MapConfig : ScriptableObject
{
    public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

    [OneLineWithHeader] 
    public IntMinMax numOfPreBossNodes;
    [OneLineWithHeader]
    public IntMinMax numOfStartingNodes;
    public List<NodeBlueprint> bossNodeOptions;
    [Reorderable]
    public ListOfMapLayers layers;
    
    [System.Serializable]
    public class ListOfMapLayers : ReorderableArray<MapLayer>
    {
    }
}
