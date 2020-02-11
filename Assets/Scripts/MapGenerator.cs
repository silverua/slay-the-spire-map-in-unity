using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum MapOrientation
    {
        TopToBottom,
        LeftToRight
    }
    
    public MapConfig config;
    public MapOrientation orientation;
    public List<NodeBlueprint> randomNodes;
    public GameObject nodePrefab;
    public GameObject linePrefab;

    private List<float> layerDistances;
    private GameObject mapParent;
    // nodes by layer:
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
        
        RandomizeNodePositions();
        
        SetUpConnections();
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
        for (var i = 0; i < layer.numOfNodes.GetValue(); i++)
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
        
    }

    private NodeBlueprint GetRandomNode()
    {
        return randomNodes.Count == 0 ? null : randomNodes[Random.Range(0, randomNodes.Count)];
    }
}
