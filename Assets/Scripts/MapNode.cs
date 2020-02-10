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

    public void ApplyBlueprint(NodeBlueprint blueprint)
    {
        Blueprint = blueprint;
        sr.sprite = blueprint.sprite;
    }

    public void SetAttainable(bool attainable)
    {
        sr.color = attainable ? attainableColor : lockedColor;
    }
}
