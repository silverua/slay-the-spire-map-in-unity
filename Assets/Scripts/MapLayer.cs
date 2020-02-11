using OneLine;
using UnityEngine;

[System.Serializable]
public class MapLayer
{
    public NodeBlueprint node;
    [OneLineWithHeader] public IntMinMax numOfNodes;
    [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;
    public float nodesApartDistance;
    [Range(0f, 1f)] public float randomizePosition;
    [Range(0f, 1f)] public float randomizeNodes;
}
