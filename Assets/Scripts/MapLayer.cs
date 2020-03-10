using OneLine;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class MapLayer
    {
        [Tooltip("Default node for this map layer. If Randomize Nodes is 0, you will get this node 100% of the time")]
        public NodeType nodeType;
        // [OneLineWithHeader] public IntMinMax numOfNodes;
        [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;
        [Tooltip("Distance between the nodes on this layer")]
        public float nodesApartDistance;
        [Tooltip("If this is set to 0, nodes on this layer will appear in a straight line. Closer to 1f = more position randomization")]
        [Range(0f, 1f)] public float randomizePosition;
        [Tooltip("Chance to get a random node that is different from the default node on this layer")]
        [Range(0f, 1f)] public float randomizeNodes;
    }
}