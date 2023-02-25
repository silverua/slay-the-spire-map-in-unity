using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public class MapNode : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public SpriteRenderer sr;
        public Image image;
        public SpriteRenderer visitedCircle;
        public Image circleImage;
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
            if (sr != null) sr.sprite = blueprint.sprite;
            if (image != null) image.sprite = blueprint.sprite;
            if (node.nodeType == NodeType.Boss) transform.localScale *= 1.5f;
            if (sr != null) initialScale = sr.transform.localScale.x;
            if (image != null) initialScale = image.transform.localScale.x;

            if (visitedCircle != null)
            {
                visitedCircle.color = MapView.Instance.visitedColor;
                visitedCircle.gameObject.SetActive(false);
            }

            if (circleImage != null)
            {
                circleImage.color = MapView.Instance.visitedColor;
                circleImage.gameObject.SetActive(false);    
            }
            
            SetState(NodeStates.Locked);
        }

        public void SetState(NodeStates state)
        {
            if (visitedCircle != null) visitedCircle.gameObject.SetActive(false);
            if (circleImage != null) circleImage.gameObject.SetActive(false);
            
            switch (state)
            {
                case NodeStates.Locked:
                    if (sr != null)
                    {
                        sr.DOKill();
                        sr.color = MapView.Instance.lockedColor;
                    }

                    if (image != null)
                    {
                        image.DOKill();
                        image.color = MapView.Instance.lockedColor;
                    }

                    break;
                case NodeStates.Visited:
                    if (sr != null)
                    {
                        sr.DOKill();
                        sr.color = MapView.Instance.visitedColor;
                    }
                    
                    if (image != null)
                    {
                        image.DOKill();
                        image.color = MapView.Instance.visitedColor;
                    }
                    
                    if (visitedCircle != null) visitedCircle.gameObject.SetActive(true);
                    if (circleImage != null) circleImage.gameObject.SetActive(true);
                    break;
                case NodeStates.Attainable:
                    // start pulsating from visited to locked color:
                    if (sr != null)
                    {
                        sr.color = MapView.Instance.lockedColor;
                        sr.DOKill();
                        sr.DOColor(MapView.Instance.visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    }
                    
                    if (image != null)
                    {
                        image.color = MapView.Instance.lockedColor;
                        image.DOKill();
                        image.DOColor(MapView.Instance.visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            if (sr != null)
            {
                sr.transform.DOKill();
                sr.transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
            }

            if (image != null)
            {
                image.transform.DOKill();
                image.transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (sr != null)
            {
                sr.transform.DOKill();
                sr.transform.DOScale(initialScale, 0.3f);
            }

            if (image != null)
            {
                image.transform.DOKill();
                image.transform.DOScale(initialScale, 0.3f);
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            mouseDownTime = Time.time;
        }

        public void OnPointerUp(PointerEventData data)
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
