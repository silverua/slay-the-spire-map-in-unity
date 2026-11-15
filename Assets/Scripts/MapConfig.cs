using System.Collections.Generic;
using OneLine;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu]
    public class MapConfig : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;
        [Tooltip("Nodes that will be used on layers with Randomize Nodes > 0")]
        public List<NodeType> randomNodes = new List<NodeType>
            {NodeType.Mystery, NodeType.Store, NodeType.Treasure, NodeType.MinorEnemy, NodeType.RestSite};
        public int GridWidth
        {
            get
            {
                int width = 1;
                if (layers == null) return width;

                foreach (MapLayer layer in layers)
                {
                    if (layer?.numOfNodes == null) continue;
                    width = Mathf.Max(width, Mathf.Max(layer.numOfNodes.min, layer.numOfNodes.max));
                }

                return width;
            }
        }

        [Tooltip("Increase this number to generate more paths")]
        public int extraPaths;
        public List<MapLayer> layers;

        // Kept so existing MapConfig assets still load their old counts. Cleared after they are copied onto layers.
        [SerializeField, HideInInspector] private IntMinMax numOfStartingNodes;
        [SerializeField, HideInInspector] private IntMinMax numOfPreBossNodes;

        private void OnValidate()
        {
            MigrateLegacyNodeCounts();
            if (layers == null) return;

            for (int i = 0; i < layers.Count; i++)
            {
                MapLayer layer = layers[i];
                if (layer?.numOfNodes == null) continue;

                int min = Mathf.Max(1, Mathf.Min(layer.numOfNodes.min, layer.numOfNodes.max));
                int max = Mathf.Max(min, Mathf.Max(layer.numOfNodes.min, layer.numOfNodes.max));
                if (layer.numOfNodes.min != min || layer.numOfNodes.max != max)
                {
                    layer.numOfNodes.min = min;
                    layer.numOfNodes.max = max;
                }

                if (layer.nodeType == NodeType.Boss && max > 1)
                {
                    Debug.LogWarning(
                        $"'{name}' layer {i} is a Boss layer with up to {max} nodes. Layout, the finished-run check, and the boss name all use the first boss node.",
                        this);
                }
            }
        }

        private void MigrateLegacyNodeCounts()
        {
            if (layers == null || layers.Count == 0) return;
            if (!HasLegacyCount(numOfStartingNodes) && !HasLegacyCount(numOfPreBossNodes)) return;

            int width = 1;
            if (HasLegacyCount(numOfStartingNodes))
                width = Mathf.Max(width, numOfStartingNodes.max);
            if (HasLegacyCount(numOfPreBossNodes))
                width = Mathf.Max(width, numOfPreBossNodes.max);

            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i] == null) continue;

                if (i == layers.Count - 1)
                    CopyNodeCount(layers[i], 1, 1);
                else
                    CopyNodeCount(layers[i], 1, width);
            }

            int layerBeforeBoss = layers.Count - 2;
            if (HasLegacyCount(numOfPreBossNodes) && layerBeforeBoss >= 0 && layers[layerBeforeBoss] != null)
            {
                CopyNodeCount(layers[layerBeforeBoss], numOfPreBossNodes);
                ClearLegacyCount(numOfPreBossNodes);
            }

            if (HasLegacyCount(numOfStartingNodes) && layers[0] != null)
            {
                CopyNodeCount(layers[0], numOfStartingNodes);
                ClearLegacyCount(numOfStartingNodes);
            }
        }

        private static bool HasLegacyCount(IntMinMax count)
        {
            return count != null && (count.min > 0 || count.max > 0);
        }

        private static void CopyNodeCount(MapLayer layer, IntMinMax count)
        {
            CopyNodeCount(layer, count.min, count.max);
        }

        private static void CopyNodeCount(MapLayer layer, int min, int max)
        {
            if (layer.numOfNodes == null)
                layer.numOfNodes = new IntMinMax();

            layer.numOfNodes.min = min;
            layer.numOfNodes.max = max;
        }

        private static void ClearLegacyCount(IntMinMax count)
        {
            count.min = 0;
            count.max = 0;
        }
    }
}