using System.Collections.Generic;
using System.Linq;
using OneLine;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum MapOrientation
    {
        BottomToTop,
        TopToBottom,
        RightToLeft,
        LeftToRight
    }
    
    public MapConfig config;
    public MapOrientation orientation;
    public List<NodeBlueprint> randomNodes;
    public GameObject nodePrefab;
    public GameObject linePrefab;

    private List<float> layerDistances;
    private GameObject mapParent;
    private List<List<Point>> paths;
    // ALL nodes by layer:
    private readonly List<List<MapNode>> nodes = new List<List<MapNode>>();
    
    public static MapGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Generate();
    }

    private void ClearMap()
    {
        if (mapParent != null)
            Destroy(mapParent);
        
        nodes.Clear();
    }

    public void Generate()
    {
        ClearMap();
        
        mapParent = new GameObject("MapParent");
        
        GenerateLayerDistances();
        
        for (var i = 0; i < config.layers.Count; i++)
            PlaceLayer(i);
        
        GeneratePaths();
        
        RandomizeNodePositions();
        
        SetUpConnections();
        
        HideNodesWithoutConnections();
    }

    private void HideNodesWithoutConnections()
    {
        foreach (var layer in nodes)
        foreach (var node in layer)
            if (node.HasNoConnections())
                node.gameObject.SetActive(false);
    }

    private void GenerateLayerDistances()
    {
        layerDistances = new List<float>();
        foreach (var layer in config.layers)
            layerDistances.Add(layer.distanceFromPreviousLayer.GetValue());
    }

    private float GetDistanceToLayer(int layerIndex)
    {
        if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;
        
        return layerDistances.Take(layerIndex + 1).Sum();
    }

    private void PlaceLayer(int layerIndex)
    {
        var layer = config.layers[layerIndex];
        var layerParentObject = new GameObject("Layer " + layerIndex + " Parent");
        layerParentObject.transform.SetParent(mapParent.transform);
        var nodesOnThisLayer = new List<MapNode>();
        for (var i = 0; i < config.gridWidth; i++)
        {
            var nodeObject = Instantiate(nodePrefab, layerParentObject.transform);
            nodeObject.transform.localPosition = new Vector3(i * layer.nodesApartDistance, 0f, 0f);
            var node = nodeObject.GetComponent<MapNode>();
            nodesOnThisLayer.Add(node);
            var blueprint = Random.Range(0f, 1f) < layer.randomizeNodes ? GetRandomNode() : layer.node;
            node.SetUp(blueprint, layerIndex);
        }

        nodes.Add(nodesOnThisLayer);
        // offset of this layer to make all the nodes centered:
        var offset = (nodesOnThisLayer[nodesOnThisLayer.Count - 1].transform.localPosition.x -
                      nodesOnThisLayer[0].transform.localPosition.x) / 2f;
        layerParentObject.transform.localPosition = new Vector3(- offset, GetDistanceToLayer(layerIndex), 0f);
    }

    private void RandomizeNodePositions()
    {
        for (var index = 0; index < nodes.Count; index++)
        {
            var list = nodes[index];
            var layer = config.layers[index];
            var distToNextLayer = index + 1 >= layerDistances.Count
                ? 0f
                : layerDistances[index + 1];
            var distToPreviousLayer = layerDistances[index];
            
            foreach (var node in list)
            {
                var xRnd = Random.Range(-1f, 1f);
                var yRnd = Random.Range(-1f, 1f);

                var x = xRnd * layer.nodesApartDistance / 2f;
                var y = yRnd < 0 ? distToPreviousLayer * yRnd / 2f : distToNextLayer * yRnd / 2f;

                node.transform.localPosition += new Vector3(x, y, 0) * layer.randomizePosition;
            }
        }
    }

    private void SetUpConnections()
    {
        foreach (var path in paths)
        {
            for (var i = 0; i < path.Count; i++)
            {
                var node = GetNode(path[i]);
                
                if (i > 0)
                {
                    // previous because the path is flipped
                    var nextNode = GetNode(path[i - 1]);
                    nextNode.AddIncoming(node);
                    node.AddOutgoing(nextNode);
                }

                if (i < path.Count - 1)
                {
                    var previousNode = GetNode(path[i + 1]);
                    previousNode.AddOutgoing(node);
                    node.AddIncoming(previousNode);
                }
            }
        }
    }

    private MapNode GetNode(Point p)
    {
        return nodes[p.y][p.x];
    }

    private Point GetFinalNode()
    {
        var y = config.layers.Count - 1;
        if (config.gridWidth % 2 == 1)
            return new Point(config.gridWidth / 2 + 1, y);

        return Random.Range(0, 2) == 0
            ? new Point(config.gridWidth / 2 + 1, y)
            : new Point(config.gridWidth / 2, y);
    }

    private void GeneratePaths()
    {
        var finalNode = GetFinalNode();
        paths = new List<List<Point>>();
        var numOfStartingNodes = config.numOfStartingNodes.GetValue();
        while (!PathsLeadToNDifferentPoints(paths, numOfStartingNodes))
        {
            var path = Path(finalNode, 0, config.gridWidth);
            paths.Add(path);
        }
    }

    private bool PathsLeadToNDifferentPoints(IEnumerable<List<Point>> paths, int n)
    {
        return (from path in paths select path[path.Count - 1].x).Distinct().Count() == n;
    }

    private class Point
    {
        public int x; 
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private List<Point> Path(Point from, int toY, int width)
    {
        if (from.y == toY)
        {
            Debug.LogError("Points are on same layers, return");
            return null;
        }
        
        // making one y step in this direction with each move
        var direction = from.y > toY ? -1 : 1;
        
        var path = new List<Point> {from};
        while (path[path.Count - 1].y != toY)
        {
            var lastPoint = path[path.Count - 1];
            // forward
            var candidateXs = new List<int> {lastPoint.x};
            // left
            if (lastPoint.x - 1 >= 0) candidateXs.Add(lastPoint.x - 1);
            // right
            if (lastPoint.x + 1 < width) candidateXs.Add(lastPoint.x + 1);
            
            var nextPoint = new Point(candidateXs[Random.Range(0, candidateXs.Count)], lastPoint.y + direction);
            path.Add(nextPoint);
        }

        return path;
    }

    private NodeBlueprint GetRandomNode()
    {
        return randomNodes.Count == 0 ? null : randomNodes[Random.Range(0, randomNodes.Count)];
    }
}
