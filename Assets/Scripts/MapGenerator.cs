using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapConfig config;
    public List<NodeBlueprint> randomNodes;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        
    }

    private void PlaceLayer(int layerIndex)
    {
        
    }

    private NodeBlueprint GetRandomNode()
    {
        return randomNodes.Count == 0 ? null : randomNodes[Random.Range(0, randomNodes.Count)];
    }
}
