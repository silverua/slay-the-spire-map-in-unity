using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color32 attainableColor = Color.white;
    public Color32 lockedColor = Color.gray;
    public List<MapNode> incomingConnections;
    public List<MapNode> outgoingConnections;

    public NodeBlueprint Blueprint { get; private set; }
    public int LayerIndex { get; private set; }

    public void SetUp(NodeBlueprint blueprint, int layerIndex)
    {
        Blueprint = blueprint;
        LayerIndex = layerIndex;
        sr.sprite = blueprint.sprite;
    }

    public void SetAttainable(bool attainable)
    {
        sr.color = attainable ? attainableColor : lockedColor;
    }
}
