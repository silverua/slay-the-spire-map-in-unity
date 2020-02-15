using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum NodeStates
{
    Locked, 
    Visited,
    Attainable
}

public class MapNode : MonoBehaviour
{
    public SpriteRenderer sr;
    public SpriteRenderer visitedCircle;
    public Color32 visitedColor = Color.white;
    public Color32 lockedColor = Color.gray;
    public readonly List<MapNode> IncomingConnections = new List<MapNode>();
    public readonly List<MapNode> OutgoingConnections = new List<MapNode>();

    public NodeBlueprint Blueprint { get; private set; }
    public int LayerIndex { get; private set; }

    private float initialScale;
    private const float HoverScaleFactor = 1.2f;
    
    public void SetUp(NodeBlueprint blueprint, int layerIndex)
    {
        Blueprint = blueprint;
        LayerIndex = layerIndex;
        sr.sprite = blueprint.sprite;
        initialScale = sr.transform.localScale.x;
        visitedCircle.color = visitedColor;
        visitedCircle.gameObject.SetActive(false);
        SetState(NodeStates.Locked);
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
    
    public void RemoveIncoming(MapNode node)
    {
        if(IncomingConnections.Contains(node))
            IncomingConnections.Remove(node);
    }

    public void RemoveOutgoing(MapNode node)
    {
        if(OutgoingConnections.Contains(node))
            OutgoingConnections.Remove(node);
    }

    public bool HasNoConnections()
    {
        return IncomingConnections.Count == 0 && OutgoingConnections.Count == 0;
    }

    public void SetState(NodeStates state)
    {
        switch (state)
        {
            case NodeStates.Locked:
                sr.DOKill();
                sr.color = lockedColor;
                break;
            case NodeStates.Visited:
                sr.DOKill();
                sr.color = visitedColor;
                break;
            case NodeStates.Attainable:
                // start pulsating from visited to locked color:
                sr.color = lockedColor;
                sr.DOKill();
                sr.DOColor(visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnMouseEnter()
    {
        sr.transform.DOKill();
        sr.transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
    }
    
    private void OnMouseExit()
    {
        sr.transform.DOKill();
        sr.transform.DOScale(initialScale, 0.3f);
    }
}
