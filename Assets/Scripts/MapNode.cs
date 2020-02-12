using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color32 attainableColor = Color.white;
    public Color32 lockedColor = Color.gray;
    public readonly List<MapNode> IncomingConnections = new List<MapNode>();
    public readonly List<MapNode> OutgoingConnections = new List<MapNode>();

    public NodeBlueprint Blueprint { get; private set; }
    public int LayerIndex { get; private set; }

    public void SetUp(NodeBlueprint blueprint, int layerIndex)
    {
        Blueprint = blueprint;
        LayerIndex = layerIndex;
        sr.sprite = blueprint.sprite;
    }

    public void AddIncoming(MapNode node)
    {
        if(IncomingConnections.Contains(node))
            return;

        IncomingConnections.Add(node);
    }

    public void AddOutgoing(MapNode node)
    {
        if(OutgoingConnections.Contains(node))
            return;

        OutgoingConnections.Add(node);
    }

    public bool HasNoConnections()
    {
        return IncomingConnections.Count == 0 && OutgoingConnections.Count == 0;
    }

    public void SetAttainable(bool attainable)
    {
        sr.color = attainable ? attainableColor : lockedColor;
    }
}
