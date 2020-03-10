using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public enum NodeStates
    {
        Locked,
        Visited,
        Attainable
    }
}

namespace Map
{
    public class MapNode : MonoBehaviour
    {
        public SpriteRenderer sr;
        public SpriteRenderer visitedCircle;
        public Image visitedCircleImage;

        public Node Node { get; private set; }
        public NodeBlueprint Blueprint { get; private set; }

        private float initialScale;
        private const float HoverScaleFactor = 1.2f;
        private float mouseDownTime;

        private const float MaxClickDuration = 0.5f;

        public void SetUp(Node node, NodeBlueprint blueprint)
        {
            Node = node;
            Blueprint = blueprint;
            sr.sprite = blueprint.sprite;
            if (node.nodeType == NodeType.Boss) transform.localScale *= 1.5f;
            initialScale = sr.transform.localScale.x;
            visitedCircle.color = MapView.Instance.visitedColor;
            visitedCircle.gameObject.SetActive(false);
            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates state)
        {
            visitedCircle.gameObject.SetActive(false);
            switch (state)
            {
                case NodeStates.Locked:
                    sr.DOKill();
                    sr.color = MapView.Instance.lockedColor;
                    break;
                case NodeStates.Visited:
                    sr.DOKill();
                    sr.color = MapView.Instance.visitedColor;
                    visitedCircle.gameObject.SetActive(true);
                    break;
                case NodeStates.Attainable:
                    // start pulsating from visited to locked color:
                    sr.color = MapView.Instance.lockedColor;
                    sr.DOKill();
                    sr.DOColor(MapView.Instance.visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
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

        public void ShowSwirlAnimation()
        {
            if (visitedCircleImage == null)
                return;

            const float fillDuration = 0.3f;
            visitedCircleImage.fillAmount = 0;

            DOTween.To(() => visitedCircleImage.fillAmount, x => visitedCircleImage.fillAmount = x, 1f, fillDuration);
        }
    }
}
