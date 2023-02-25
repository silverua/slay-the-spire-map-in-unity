using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Map
{
    public class MapViewUI : MapView
    {
        [Header("UI Map Settings")]
        [SerializeField] private ScrollRect scrollRectHorizontal;
        [SerializeField] private ScrollRect scrollRectVertical;
        [SerializeField] private float unitsToPixelsMultiplier  = 10f;
        [SerializeField] private float padding; // padding of the background from the sides of the scroll rect:
        [SerializeField] private Vector2 backgroundPadding;
        [SerializeField] private float backgroundPPUMultiplier = 1;
        [SerializeField] private UILineRenderer uiLinePrefab;

        protected override void ClearMap()
        {
            scrollRectHorizontal.gameObject.SetActive(false);
            scrollRectVertical.gameObject.SetActive(false);

            foreach (var scrollRect in new []{scrollRectHorizontal, scrollRectVertical})
            foreach (Transform t in scrollRect.content)
                Destroy(t.gameObject);
            
            MapNodes.Clear();
            lineConnections.Clear();
        }

        private ScrollRect GetScrollRectForMap()
        {
            return orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft
                ? scrollRectHorizontal
                : scrollRectVertical;
        }

        protected override void CreateMapParent()
        {
            var scrollRect = GetScrollRectForMap();
            scrollRect.gameObject.SetActive(true);
            
            firstParent = new GameObject("OuterMapParent");
            firstParent.transform.SetParent(scrollRect.content);
            firstParent.transform.localScale = Vector3.one;
            var fprt = firstParent.AddComponent<RectTransform>();
            Stretch(fprt);
            
            mapParent = new GameObject("MapParentWithAScroll");
            mapParent.transform.SetParent(firstParent.transform);
            mapParent.transform.localScale = Vector3.one;
            var mprt = mapParent.AddComponent<RectTransform>();
            Stretch(mprt);
            
            SetMapLength();
        }

        private void SetMapLength()
        {
            var rt = GetScrollRectForMap().content;
            var sizeDelta = rt.sizeDelta;
            var length = padding + Map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
            if (orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft)
                sizeDelta.x = length;
            else
                sizeDelta.y = length;
            rt.sizeDelta = sizeDelta;
        }

        private static void Stretch(RectTransform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.anchorMin = Vector2.zero;
            tr.anchorMax = Vector2.one;
            tr.sizeDelta = Vector2.zero;
            tr.anchoredPosition = Vector2.zero;
        }

        protected override MapNode CreateMapNode(Node node)
        {
            var mapNodeObject = Instantiate(nodePrefab, mapParent.transform);
            var mapNode = mapNodeObject.GetComponent<MapNode>();
            var blueprint = GetBlueprint(node.blueprintName);
            mapNode.SetUp(node, blueprint);
            mapNode.transform.localPosition = GetNodePosition(node);
            return mapNode;
        }

        private Vector2 GetNodePosition(Node node)
        {
            var length = padding + Map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
            
            switch (orientation)
            {
                case MapOrientation.BottomToTop:
                    return new Vector2(-backgroundPadding.x / 2f, (padding - length) / 2f) +
                           node.position * unitsToPixelsMultiplier;
                case MapOrientation.TopToBottom:
                    return new Vector2(backgroundPadding.x / 2f, (length - padding) / 2f) -
                           node.position * unitsToPixelsMultiplier;
                case MapOrientation.RightToLeft:
                    return new Vector2((length - padding) / 2f, backgroundPadding.y / 2f) -
                           Flip(node.position) * unitsToPixelsMultiplier;
                case MapOrientation.LeftToRight:
                    return new Vector2((padding - length) / 2f, -backgroundPadding.y / 2f) +
                           Flip(node.position) * unitsToPixelsMultiplier;
                default:
                    return Vector2.zero;
            }
        }

        private static Vector2 Flip(Vector2 other) => new Vector2(other.y, other.x);

        protected override void SetOrientation()
        {
            // do nothing here for UI:
        }

        protected override void CreateMapBackground(Map m)
        {
            var backgroundObject = new GameObject("Background");
            backgroundObject.transform.SetParent(mapParent.transform);
            backgroundObject.transform.localScale = Vector3.one;
            var rt = backgroundObject.AddComponent<RectTransform>();
            Stretch(rt);
            rt.SetAsFirstSibling();
            rt.sizeDelta = backgroundPadding;
            
            var image = backgroundObject.AddComponent<Image>();
            image.color = backgroundColor;
            image.type = Image.Type.Sliced;
            image.sprite = background;
            image.pixelsPerUnitMultiplier = backgroundPPUMultiplier;
        }

        protected override void AddLineConnection(MapNode from, MapNode to)
        {
            if (uiLinePrefab == null) return;
            
            var lineRenderer = Instantiate(uiLinePrefab, mapParent.transform);
            lineRenderer.transform.SetAsFirstSibling();
            var fromPoint = from.transform.position +
                            (to.transform.position - from.transform.position).normalized * offsetFromNodes;

            var toPoint = to.transform.position +
                          (from.transform.position - to.transform.position).normalized * offsetFromNodes;

            // drawing lines in local space:
            lineRenderer.transform.position = fromPoint;

            // line renderer with 2 points only does not handle transparency properly:
            var list = new List<Vector2>();
            for (var i = 0; i < linePointsCount; i++)
            {
                list.Add(Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float)i / (linePointsCount - 1)));
            }

            lineRenderer.Points = list.ToArray();

            var dottedLine = lineRenderer.GetComponent<DottedLineRenderer>();
            if (dottedLine != null) dottedLine.ScaleMaterial();

            lineConnections.Add(new LineConnection(null, lineRenderer, from, to));
        }
    }
}