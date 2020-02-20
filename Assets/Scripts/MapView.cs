using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapView : MonoBehaviour
{
    public enum MapOrientation
    {
        BottomToTop,
        TopToBottom,
        RightToLeft,
        LeftToRight
    }

    public MapManager mapManager;
    public MapOrientation orientation;
    public List<NodeBlueprint> blueprints;
    public GameObject nodePrefab;
    public float orientationOffset;
    [Header("Line settings")]
    public GameObject linePrefab;
    public int linePointsCount = 10;
    public float offsetFromNodes = 0.5f;
    
    private GameObject firstParent;
    private GameObject mapParent;
    private List<List<Point>> paths;
    // ALL nodes by layer:
    public readonly List<MapNode> MapNodes = new List<MapNode>();

    public static MapView Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void ClearMap()
    {
        if (firstParent != null)
            Destroy(firstParent);
        
        MapNodes.Clear();
    }

    public void ShowMap(Map m)
    {
        if (m == null)
        {
            Debug.LogWarning("Map was null in MapView.ShowMap()");
            return;
        }

        ClearMap();
        
        CreateMapParent();

        CreateNodes(m.nodes);

        DrawLines();
        
        SetOrientation();
        
        ResetNodesRotation();

        SetFirstLayerAttainable();
    }

    private void CreateMapParent()
    {
        firstParent = new GameObject("OuterMapParent");
        mapParent = new GameObject("MapParentWithAScroll");
        mapParent.transform.SetParent(firstParent.transform);
        var scrollNonUi = mapParent.AddComponent<ScrollNonUI>();
        scrollNonUi.freezeX = orientation == MapOrientation.BottomToTop || orientation == MapOrientation.TopToBottom;
        scrollNonUi.freezeY = orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft;
        var boxCollider = mapParent.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(100, 100, 1);
    }

    private void CreateNodes(IEnumerable<Node> nodes)
    {
        foreach (var node in nodes)
        {
            var mapNode = CreateMapNode(node);
            MapNodes.Add(mapNode);
        }
    }

    private MapNode CreateMapNode(Node node)
    {
        var mapNodeObject = Instantiate(nodePrefab, mapParent.transform);
        var mapNode = mapNodeObject.GetComponent<MapNode>();
        mapNode.SetUp(node);
        mapNode.transform.localPosition = node.position;
        return mapNode;
    }

    private void SetFirstLayerAttainable()
    {
        foreach (var node in MapNodes.Where(n => n.Node.point.y == 0))
            node.SetState(NodeStates.Attainable);
    }

    private void SetOrientation()
    {
        var scrollNonUi = mapParent.GetComponent<ScrollNonUI>();
        var span = mapManager.CurrentMap.DistanceBetweenFirstAndLastLayers();
        var cameraDimension = orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft
            ? GetCameraWidth()
            : GetCameraHeight();
        var constraint = Mathf.Max(0f, span - cameraDimension);
        var bossNode = MapNodes.FirstOrDefault(node => node.Node.nodeType == NodeType.Boss);
        Debug.Log("Map span in set orientation: " + span + " camera dimension: " + cameraDimension);

        switch (orientation)
        {
            case MapOrientation.BottomToTop:
                if (scrollNonUi != null)
                {
                    scrollNonUi.yConstraints.max = 0;
                    scrollNonUi.yConstraints.min = -(constraint - orientationOffset / 2);
                }
                firstParent.transform.localPosition += new Vector3(0, orientationOffset / 2, 0);
                break;
            case MapOrientation.TopToBottom:
                mapParent.transform.eulerAngles = new Vector3(0, 0, 180);
                if (scrollNonUi != null)
                {
                    scrollNonUi.yConstraints.min = 0;
                    scrollNonUi.yConstraints.max = constraint - orientationOffset / 2f;
                }
                // factor in map span:
                firstParent.transform.localPosition += new Vector3(0, -orientationOffset / 2, 0);
                break;
            case MapOrientation.RightToLeft:
                mapParent.transform.eulerAngles = new Vector3(0, 0, 90);
                // factor in map span:
                firstParent.transform.localPosition -= new Vector3(orientationOffset, bossNode.transform.position.y, 0);
                if (scrollNonUi != null)
                {
                    scrollNonUi.xConstraints.max = constraint - orientationOffset;
                    scrollNonUi.xConstraints.min = 0;
                }
                break;
            case MapOrientation.LeftToRight:
                mapParent.transform.eulerAngles = new Vector3(0, 0, -90);
                firstParent.transform.localPosition += new Vector3(orientationOffset, -bossNode.transform.position.y, 0);
                if (scrollNonUi != null)
                {
                    scrollNonUi.xConstraints.max = 0;
                    scrollNonUi.xConstraints.min = -(constraint - orientationOffset);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DrawLines()
    {
        foreach (var node in MapNodes)
        {
            foreach (var connection in node.Node.outgoing)
                AddLineConnection(node, GetNode(connection));
        }
    }

    private void ResetNodesRotation()
    {
        foreach (var node in MapNodes)
            node.transform.rotation = Quaternion.identity;
    }

    public void AddLineConnection(MapNode from, MapNode to)
    {
        var lineObject = Instantiate(linePrefab, mapParent.transform);
        var lineRenderer = lineObject.GetComponent<LineRenderer>();
        var fromPoint = from.transform.position +
                        (to.transform.position - from.transform.position).normalized * offsetFromNodes;

        var toPoint = to.transform.position +
                      (from.transform.position - to.transform.position).normalized * offsetFromNodes;

        // drawing lines in local space:
        lineObject.transform.position = fromPoint;
        lineRenderer.useWorldSpace = false;

        // line renderer with 2 points only does not handle transparency properly:
        lineRenderer.positionCount = linePointsCount;
        for (var i = 0; i < linePointsCount; i++)
        {
            lineRenderer.SetPosition(i,
                Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float) i / (linePointsCount - 1)));
        }
        
        var dottedLine = lineObject.GetComponent<DottedLineRenderer>();
        if(dottedLine != null) dottedLine.ScaleMaterial();
    }

    private MapNode GetNode(Point p)
    {
        return MapNodes.FirstOrDefault(n => n.Node.point.Equals(p));
    }

    public NodeBlueprint GetBlueprint(NodeType type)
    {
        return blueprints.FirstOrDefault(n => n.nodeType == type);
    }

    private static float GetCameraWidth()
    {
        var cam = Camera.main;
        if (cam == null) return 0;
        var height = 2f * cam.orthographicSize; 
        return height * cam.aspect;
    }
    
    private static float GetCameraHeight()
    {
        var cam = Camera.main;
        if (cam == null) return 0;
        return 2f * cam.orthographicSize;
    }
}
