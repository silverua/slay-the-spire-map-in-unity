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
    
    public MapOrientation orientation;
    public List<NodeBlueprint> blueprints;
    public GameObject nodePrefab;
    [Header("Line settings")]
    public GameObject linePrefab;
    public int linePointsCount = 10;
    public float offsetFromNodes = 0.5f;
    
    private GameObject mapParent;
    private List<List<Point>> paths;
    // ALL nodes by layer:
    private readonly List<MapNode> mapNodes = new List<MapNode>();

    public static MapView Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void ClearMap()
    {
        if (mapParent != null)
            Destroy(mapParent);
        
        mapNodes.Clear();
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
        mapParent = new GameObject("MapParent");
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
            mapNodes.Add(mapNode);
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
        foreach (var node in mapNodes.Where(n => n.Node.point.y == 0))
            node.SetState(NodeStates.Attainable);
    }

    private void SetOrientation()
    {
        switch (orientation)
        {
            case MapOrientation.BottomToTop:
                // do nothing
                break;
            case MapOrientation.TopToBottom:
                mapParent.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case MapOrientation.RightToLeft:
                mapParent.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case MapOrientation.LeftToRight:
                mapParent.transform.eulerAngles = new Vector3(0, 0, -90);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DrawLines()
    {
        foreach (var node in mapNodes)
        {
            foreach (var connection in node.Node.outgoing)
                AddLineConnection(node, GetNode(connection));
        }
    }

    private void ResetNodesRotation()
    {
        foreach (var node in mapNodes)
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
        return mapNodes.FirstOrDefault(n => n.Node.point.Equals(p));
    }

    public NodeBlueprint GetBlueprint(NodeType type)
    {
        return blueprints.FirstOrDefault(n => n.nodeType == type);
    }
}
