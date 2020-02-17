using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class Node
{
    public Point point;
    public List<Point> incoming = new List<Point>();
    public List<Point> outgoing = new List<Point>();
    [JsonConverter(typeof(StringEnumConverter))]
    public NodeType nodeType;
    public Vector2 position;

    public Node(NodeType nodeType, Point point)
    {
        this.nodeType = nodeType;
        this.point = point;
    }
    
    public void AddIncoming(Point node)
    {
        if(incoming.Contains(node))
            return;

        incoming.Add(node);
    }

    public void AddOutgoing(Point node)
    {
        if(outgoing.Contains(node))
            return;

        outgoing.Add(node);
    }
    
    public void RemoveIncoming(Point node)
    {
        if(incoming.Contains(node))
            incoming.Remove(node);
    }

    public void RemoveOutgoing(Point node)
    {
        if(outgoing.Contains(node))
            outgoing.Remove(node);
    }
    
    public bool HasNoConnections()
    {
        return incoming.Count == 0 && outgoing.Count == 0;
    }
}