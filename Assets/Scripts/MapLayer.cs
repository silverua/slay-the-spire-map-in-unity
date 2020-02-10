using UnityEngine;

[System.Serializable]
public class MapLayer
{
    public NodeBlueprint node;
    public float distanceFromPreviousLayer;
    public float nodesApartDistance;
    [Range(0f, 1f)] public float randomizePosition;
    [Range(0f, 1f)] public float randomizeNodes;
}
