using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<MapNode> incomingConnections;
    public List<MapNode> outgoingConnections;

    public NodeBlueprint Blueprint { get; private set; }

    public void ApplyBlueprint(NodeBlueprint blueprint)
    {
        Blueprint = blueprint;
        sr.sprite = blueprint.sprite;
    }
}
