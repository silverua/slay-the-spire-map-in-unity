using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public static class MapGenerator
    {
        private static MapConfig config;

        private static List<float> layerDistances;
        // ALL nodes by layer:
        private static readonly List<List<Node>> nodes = new List<List<Node>>();

        public static Map GetMap(MapConfig conf)
        {
            if (conf == null)
            {
                Debug.LogWarning("Config was null in MapGenerator.Generate()");
                return null;
            }

            config = conf;
            nodes.Clear();

            GenerateLayerDistances();

            for (int i = 0; i < conf.layers.Count; i++)
                PlaceLayer(i);

            List<List<Vector2Int>> paths = GeneratePaths();

            RandomizeNodePositions();

            SetUpConnections(paths);

            EnsureAllNodesUsed();

            RemoveCrossConnections();

            // select all the nodes with connections:
            bool keepIsolated = nodes.Count <= 1;
            List<Node> nodesList = nodes.SelectMany(n => n)
                .Where(n => keepIsolated || n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            // pick a random name of the boss level for this map:
            string bossNodeName = config.nodeBlueprints.Where(b => b.nodeType == NodeType.Boss).ToList().Random().name;
            return new Map(conf.name, bossNodeName, nodesList, new List<Vector2Int>());
        }

        private static void GenerateLayerDistances()
        {
            layerDistances = new List<float>();
            foreach (MapLayer layer in config.layers)
                layerDistances.Add(layer.distanceFromPreviousLayer.GetValue());
        }

        private static float GetDistanceToLayer(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;

            return layerDistances.Take(layerIndex + 1).Sum();
        }

        private static void PlaceLayer(int layerIndex)
        {
            MapLayer layer = config.layers[layerIndex];
            List<Node> nodesOnThisLayer = new List<Node>();

            // offset of this layer to make all the nodes centered:
            float offset = layer.nodesApartDistance * config.GridWidth / 2f;

            foreach (int column in PickNodeColumns(GetNodeCount(layer)))
            {
                var supportedRandomNodeTypes =
                    config.randomNodes.Where(t => config.nodeBlueprints.Any(b => b.nodeType == t)).ToList();
                NodeType nodeType = Random.Range(0f, 1f) < layer.randomizeNodes && supportedRandomNodeTypes.Count > 0
                    ? supportedRandomNodeTypes.Random()
                    : layer.nodeType;
                string blueprintName = config.nodeBlueprints.Where(b => b.nodeType == nodeType).ToList().Random().name;
                Node node = new Node(nodeType, blueprintName, new Vector2Int(column, layerIndex))
                {
                    position = new Vector2(-offset + column * layer.nodesApartDistance, GetDistanceToLayer(layerIndex))
                };
                nodesOnThisLayer.Add(node);
            }

            nodes.Add(nodesOnThisLayer);
        }

        private static int GetNodeCount(MapLayer layer)
        {
            int width = config.GridWidth;
            if (layer?.numOfNodes == null) return 1;

            int lower = Mathf.Min(layer.numOfNodes.min, layer.numOfNodes.max);
            int upper = Mathf.Max(layer.numOfNodes.min, layer.numOfNodes.max);
            lower = Mathf.Clamp(Mathf.Max(1, lower), 1, width);
            upper = Mathf.Clamp(Mathf.Max(lower, upper), 1, width);
            return Random.Range(lower, upper + 1);
        }

        private static List<int> PickNodeColumns(int count)
        {
            int width = config.GridWidth;
            count = Mathf.Clamp(count, 1, width);

            if (count == width)
                return Enumerable.Range(0, width).ToList();

            if (count == 1)
            {
                if (width % 2 == 1)
                    return new List<int> {width / 2};

                return new List<int> {Random.Range(0, 2) == 0 ? width / 2 : width / 2 - 1};
            }

            bool[] used = new bool[width];
            List<int> columns = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                int ideal = Mathf.RoundToInt(i * (width - 1f) / (count - 1));
                int column = NearestFreeColumn(used, ideal);
                used[column] = true;
                columns.Add(column);
            }

            for (int i = 0; i < columns.Count; i++)
            {
                int shifted = columns[i] + Random.Range(-1, 2);
                if (shifted < 0 || shifted >= width || used[shifted]) continue;

                used[columns[i]] = false;
                used[shifted] = true;
                columns[i] = shifted;
            }

            columns.Sort();
            return columns;
        }

        private static int NearestFreeColumn(bool[] used, int ideal)
        {
            if (!used[ideal]) return ideal;

            int width = used.Length;
            for (int distance = 1; distance < width; distance++)
            {
                int left = ideal - distance;
                int right = ideal + distance;
                bool leftFree = left >= 0 && !used[left];
                bool rightFree = right < width && !used[right];
                if (leftFree && rightFree)
                    return Random.Range(0, 2) == 0 ? left : right;
                if (leftFree) return left;
                if (rightFree) return right;
            }

            return ideal;
        }

        private static void RandomizeNodePositions()
        {
            for (int index = 0; index < nodes.Count; index++)
            {
                List<Node> list = nodes[index];
                MapLayer layer = config.layers[index];
                float distToNextLayer = index + 1 >= layerDistances.Count
                    ? 0f
                    : layerDistances[index + 1];
                float distToPreviousLayer = layerDistances[index];

                foreach (Node node in list)
                {
                    float xRnd = Random.Range(-0.5f, 0.5f);
                    float yRnd = Random.Range(-0.5f, 0.5f);

                    float x = xRnd * layer.nodesApartDistance;
                    float y = yRnd < 0 ? distToPreviousLayer * yRnd: distToNextLayer * yRnd;

                    node.position += new Vector2(x, y) * layer.randomizePosition;
                }
            }
        }

        private static void SetUpConnections(List<List<Vector2Int>> paths)
        {
            foreach (List<Vector2Int> path in paths)
            {
                for (int i = 0; i < path.Count - 1; ++i)
                {
                    Node node = GetNode(path[i]);
                    Node nextNode = GetNode(path[i + 1]);
                    if (node == null || nextNode == null) continue;
                    node.AddOutgoing(nextNode.point);
                    nextNode.AddIncoming(node.point);
                }
            }
        }

        private static void RemoveCrossConnections()
        {
            // Links can skip columns, so a cross is any pair of edges whose endpoints are in opposite order.
            for (int y = 0; y < nodes.Count - 1; y++)
            {
                int edgeBudget = Mathf.Max(1, nodes[y].Count * nodes[y + 1].Count);
                int guard = edgeBudget * edgeBudget;
                while (guard-- > 0 && TryUncrossOnePair(y))
                {
                }
            }
        }

        private static bool TryUncrossOnePair(int y)
        {
            List<Node> fromNodes = nodes[y];
            for (int i = 0; i < fromNodes.Count; i++)
            {
                Node fromA = fromNodes[i];
                for (int aEdge = 0; aEdge < fromA.outgoing.Count; aEdge++)
                {
                    Node toA = GetNode(fromA.outgoing[aEdge]);
                    if (toA == null || toA.point.y != y + 1) continue;

                    for (int k = i + 1; k < fromNodes.Count; k++)
                    {
                        Node fromB = fromNodes[k];
                        for (int bEdge = 0; bEdge < fromB.outgoing.Count; bEdge++)
                        {
                            Node toB = GetNode(fromB.outgoing[bEdge]);
                            if (toB == null || toB.point.y != y + 1) continue;
                            if ((fromA.point.x - fromB.point.x) * (toA.point.x - toB.point.x) >= 0) continue;

                            SwapOutgoing(fromA, toA, fromB, toB);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static void SwapOutgoing(Node fromA, Node toA, Node fromB, Node toB)
        {
            fromA.RemoveOutgoing(toA.point);
            toA.RemoveIncoming(fromA.point);
            fromB.RemoveOutgoing(toB.point);
            toB.RemoveIncoming(fromB.point);

            fromA.AddOutgoing(toB.point);
            toB.AddIncoming(fromA.point);
            fromB.AddOutgoing(toA.point);
            toA.AddIncoming(fromB.point);
        }

        private static Node GetNode(Vector2Int p)
        {
            if (p.y < 0 || p.y >= nodes.Count) return null;

            return nodes[p.y].FirstOrDefault(n => n.point.x == p.x);
        }

        private static Node ClosestNode(int layerIndex, int column)
        {
            if (layerIndex < 0 || layerIndex >= nodes.Count || nodes[layerIndex].Count == 0)
                return null;

            Node closest = nodes[layerIndex][0];
            int bestDistance = Mathf.Abs(closest.point.x - column);
            for (int i = 1; i < nodes[layerIndex].Count; i++)
            {
                Node candidate = nodes[layerIndex][i];
                int distance = Mathf.Abs(candidate.point.x - column);
                if (distance >= bestDistance) continue;

                bestDistance = distance;
                closest = candidate;
            }

            return closest;
        }

        private static void EnsureAllNodesUsed()
        {
            for (int y = 0; y < nodes.Count; y++)
            {
                foreach (Node node in nodes[y])
                {
                    if (y < nodes.Count - 1 && node.outgoing.Count == 0)
                    {
                        Node child = ClosestNode(y + 1, node.point.x);
                        if (child != null)
                        {
                            node.AddOutgoing(child.point);
                            child.AddIncoming(node.point);
                        }
                    }

                    if (y > 0 && node.incoming.Count == 0)
                    {
                        Node parent = ClosestNode(y - 1, node.point.x);
                        if (parent != null)
                        {
                            parent.AddOutgoing(node.point);
                            node.AddIncoming(parent.point);
                        }
                    }
                }
            }
        }

        private static List<List<Vector2Int>> GeneratePaths()
        {
            var paths = new List<List<Vector2Int>>();
            int lastLayer = config.layers.Count - 1;
            if (lastLayer <= 0 || nodes.Count <= 1) return paths;

            List<Vector2Int> startingPoints = nodes[0].Select(n => n.point).ToList();
            List<Vector2Int> bossPoints = nodes[lastLayer].Select(n => n.point).ToList();
            if (startingPoints.Count == 0 || bossPoints.Count == 0) return paths;

            startingPoints.Shuffle();
            bossPoints.Shuffle();

            if (lastLayer == 1)
            {
                int directPathCount = Mathf.Max(startingPoints.Count, bossPoints.Count) + Mathf.Max(0, config.extraPaths);
                for (int i = 0; i < directPathCount; i++)
                {
                    paths.Add(new List<Vector2Int>
                    {
                        startingPoints[i % startingPoints.Count],
                        bossPoints[i % bossPoints.Count]
                    });
                }

                return paths;
            }

            int preBossLayer = lastLayer - 1;
            List<Vector2Int> preBossPoints = nodes[preBossLayer].Select(n => n.point).ToList();
            if (preBossPoints.Count == 0) return paths;

            preBossPoints.Shuffle();
            int numOfPaths = Mathf.Max(startingPoints.Count, preBossPoints.Count) + Mathf.Max(0, config.extraPaths);
            for (int i = 0; i < numOfPaths; ++i)
            {
                Vector2Int startNode = startingPoints[i % startingPoints.Count];
                Vector2Int endNode = preBossPoints[i % preBossPoints.Count];
                List<Vector2Int> path = Path(startNode, endNode);
                path.Add(bossPoints[i % bossPoints.Count]);
                paths.Add(path);
            }

            return paths;
        }

        // Generates a random path bottom up.
        private static List<Vector2Int> Path(Vector2Int fromPoint, Vector2Int toPoint)
        {
            int toRow = toPoint.y;
            int toCol = toPoint.x;

            int lastNodeCol = fromPoint.x;

            List<Vector2Int> path = new List<Vector2Int> { fromPoint };
            for (int row = 1; row < toRow; ++row)
            {
                if (row >= nodes.Count || nodes[row].Count == 0) continue;

                int nextCol = ChooseNextColumn(row, lastNodeCol, toCol, toRow);
                path.Add(new Vector2Int(nextCol, row));
                lastNodeCol = nextCol;
            }

            path.Add(toPoint);

            return path;
        }

        private static int ChooseNextColumn(int row, int lastNodeCol, int toCol, int toRow)
        {
            int rowsLeft = toRow - row;
            var candidates = new List<int>();
            AddCandidate(candidates, lastNodeCol, row, toCol, rowsLeft);
            AddCandidate(candidates, lastNodeCol - 1, row, toCol, rowsLeft);
            AddCandidate(candidates, lastNodeCol + 1, row, toCol, rowsLeft);

            if (candidates.Count == 0)
            {
                foreach (Node node in nodes[row])
                {
                    if (Mathf.Abs(toCol - node.point.x) <= rowsLeft)
                        candidates.Add(node.point.x);
                }
            }

            if (candidates.Count == 0)
            {
                Node closest = ClosestNode(row, lastNodeCol);
                return closest != null ? closest.point.x : lastNodeCol;
            }

            return candidates[Random.Range(0, candidates.Count)];
        }

        private static void AddCandidate(List<int> candidates, int column, int row, int toCol, int rowsLeft)
        {
            if (GetNode(new Vector2Int(column, row)) == null) return;
            if (Mathf.Abs(toCol - column) <= rowsLeft)
                candidates.Add(column);
        }
    }
}