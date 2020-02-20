using System;
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

    public Node Node { get; private set; }

    private float initialScale;
    private const float HoverScaleFactor = 1.2f;
    private float mouseDownTime;

    private const float MaxClickDuration = 0.5f;
    
    public void SetUp(Node node)
    {
        Node = node;
        sr.sprite = MapView.Instance.GetBlueprint(node.nodeType).sprite;
        initialScale = sr.transform.localScale.x;
        visitedCircle.color = visitedColor;
        visitedCircle.gameObject.SetActive(false);
        SetState(NodeStates.Locked);
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

    private void OnMouseDown()
    {
        mouseDownTime = Time.time;
    }

    private void OnMouseUp()
    {
        if (Time.time - mouseDownTime < MaxClickDuration)
        {
            // user clicked on this node:
            MapPlayerTracker.Instance.SelectNode(this);
        }
    }
}
