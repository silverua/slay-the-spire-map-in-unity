using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
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

        [Tooltip(
            "List of all the MapConfig scriptable objects from the Assets folder that might be used to construct maps. " +
            "Similar to Acts in Slay The Spire (define general layout, types of bosses.)")]
        public List<MapConfig> allMapConfigs;
        public GameObject nodePrefab;
        [Tooltip("Offset of the start/end nodes of the map from the edges of the screen")]
        public float orientationOffset;
        [Header("Background Settings")]
        [Tooltip("If the background sprite is null, background will not be shown")]
        public Sprite background;
        public Color32 backgroundColor = Color.white;
        public float xSize;
        public float yOffset;
        [Header("Line Settings")]
        public GameObject linePrefab;
        [Tooltip("Line point count should be > 2 to get smooth color gradients")]
        [Range(3, 10)]
        public int linePointsCount = 10;
        [Tooltip("Distance from the node till the line starting point")]
        public float offsetFromNodes = 0.5f;
        [Header("Colors")]
        [Tooltip("Node Visited or Attainable color")]
        public Color32 visitedColor = Color.white;
        [Tooltip("Locked node color")]
        public Color32 lockedColor = Color.gray;
        [Tooltip("Visited or available path color")]
        public Color32 lineVisitedColor = Color.white;
        [Tooltip("Unavailable path color")]
        public Color32 lineLockedColor = Color.gray;

        protected GameObject firstParent;
        protected GameObject mapParent;
        private List<List<Point>> paths;
        private Camera cam;
        // ALL nodes:
        public readonly List<MapNode> MapNodes = new List<MapNode>();
        protected readonly List<LineConnection> lineConnections = new List<LineConnection>();

        public static MapView Instance;

        public Map Map { get; protected set; }

        private void Awake()
        {
            Instance = this;
            cam = Camera.main;
        }

        protected virtual void ClearMap()
        {
            if (firstParent != null)
                Destroy(firstParent);

            MapNodes.Clear();
            lineConnections.Clear();
        }

        public virtual void ShowMap(Map m)
        {
            if (m == null)
            {
                Debug.LogWarning("Map was null in MapView.ShowMap()");
                return;
            }

            Map = m;

            ClearMap();

            CreateMapParent();

            CreateNodes(m.nodes);

            DrawLines();

            SetOrientation();

            ResetNodesRotation();

            SetAttainableNodes();

            SetLineColors();

            CreateMapBackground(m);
        }

        protected virtual void CreateMapBackground(Map m)
        {
            if (background == null) return;

            var backgroundObject = new GameObject("Background");
            backgroundObject.transform.SetParent(mapParent.transform);
            var bossNode = MapNodes.FirstOrDefault(node => node.Node.nodeType == NodeType.Boss);
            var span = m.DistanceBetweenFirstAndLastLayers();
            backgroundObject.transform.localPosition = new Vector3(bossNode.transform.localPosition.x, span / 2f, 0f);
            backgroundObject.transform.localRotation = Quaternion.identity;
            var sr = backgroundObject.AddComponent<SpriteRenderer>();
            sr.color = backgroundColor;
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.sprite = background;
            sr.size = new Vector2(xSize, span + yOffset * 2f);
        }

        protected virtual void CreateMapParent()
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

        protected void CreateNodes(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var mapNode = CreateMapNode(node);
                MapNodes.Add(mapNode);
            }
        }

        protected virtual MapNode CreateMapNode(Node node)
        {
            var mapNodeObject = Instantiate(nodePrefab, mapParent.transform);
            var mapNode = mapNodeObject.GetComponent<MapNode>();
            var blueprint = GetBlueprint(node.blueprintName);
            mapNode.SetUp(node, blueprint);
            mapNode.transform.localPosition = node.position;
            return mapNode;
        }

        public void SetAttainableNodes()
        {
            // first set all the nodes as unattainable/locked:
            foreach (var node in MapNodes)
                node.SetState(NodeStates.Locked);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // we have not started traveling on this map yet, set entire first layer as attainable:
                foreach (var node in MapNodes.Where(n => n.Node.point.y == 0))
                    node.SetState(NodeStates.Attainable);
            }
            else
            {
                // we have already started moving on this map, first highlight the path as visited:
                foreach (var point in mapManager.CurrentMap.path)
                {
                    var mapNode = GetNode(point);
                    if (mapNode != null)
                        mapNode.SetState(NodeStates.Visited);
                }

                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                // set all the nodes that we can travel to as attainable:
                foreach (var point in currentNode.outgoing)
                {
                    var mapNode = GetNode(point);
                    if (mapNode != null)
                        mapNode.SetState(NodeStates.Attainable);
                }
            }
        }

        public virtual void SetLineColors()
        {
            // set all lines to grayed out first:
            foreach (var connection in lineConnections)
                connection.SetColor(lineLockedColor);

            // set all lines that are a part of the path to visited color:
            // if we have not started moving on the map yet, leave everything as is:
            if (mapManager.CurrentMap.path.Count == 0)
                return;

            // in any case, we mark outgoing connections from the final node with visible/attainable color:
            var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
            var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

            foreach (var point in currentNode.outgoing)
            {
                var lineConnection = lineConnections.FirstOrDefault(conn => conn.from.Node == currentNode &&
                                                                            conn.to.Node.point.Equals(point));
                lineConnection?.SetColor(lineVisitedColor);
            }

            if (mapManager.CurrentMap.path.Count <= 1) return;

            for (var i = 0; i < mapManager.CurrentMap.path.Count - 1; i++)
            {
                var current = mapManager.CurrentMap.path[i];
                var next = mapManager.CurrentMap.path[i + 1];
                var lineConnection = lineConnections.FirstOrDefault(conn => conn.@from.Node.point.Equals(current) &&
                                                                            conn.to.Node.point.Equals(next));
                lineConnection?.SetColor(lineVisitedColor);
            }
        }

        protected virtual void SetOrientation()
        {
            var scrollNonUi = mapParent.GetComponent<ScrollNonUI>();
            var span = mapManager.CurrentMap.DistanceBetweenFirstAndLastLayers();
            var bossNode = MapNodes.FirstOrDefault(node => node.Node.nodeType == NodeType.Boss);
            Debug.Log("Map span in set orientation: " + span + " camera aspect: " + cam.aspect);

            // setting first parent to be right in front of the camera first:
            firstParent.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
            var offset = orientationOffset;
            switch (orientation)
            {
                case MapOrientation.BottomToTop:
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.yConstraints.max = 0;
                        scrollNonUi.yConstraints.min = -(span + 2f * offset);
                    }
                    firstParent.transform.localPosition += new Vector3(0, offset, 0);
                    break;
                case MapOrientation.TopToBottom:
                    mapParent.transform.eulerAngles = new Vector3(0, 0, 180);
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.yConstraints.min = 0;
                        scrollNonUi.yConstraints.max = span + 2f * offset;
                    }
                    // factor in map span:
                    firstParent.transform.localPosition += new Vector3(0, -offset, 0);
                    break;
                case MapOrientation.RightToLeft:
                    offset *= cam.aspect;
                    mapParent.transform.eulerAngles = new Vector3(0, 0, 90);
                    // factor in map span:
                    firstParent.transform.localPosition -= new Vector3(offset, bossNode.transform.position.y, 0);
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.xConstraints.max = span + 2f * offset;
                        scrollNonUi.xConstraints.min = 0;
                    }
                    break;
                case MapOrientation.LeftToRight:
                    offset *= cam.aspect;
                    mapParent.transform.eulerAngles = new Vector3(0, 0, -90);
                    firstParent.transform.localPosition += new Vector3(offset, -bossNode.transform.position.y, 0);
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.xConstraints.max = 0;
                        scrollNonUi.xConstraints.min = -(span + 2f * offset);
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

        protected virtual void AddLineConnection(MapNode from, MapNode to)
        {
            if (linePrefab == null) return;

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
                    Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float)i / (linePointsCount - 1)));
            }

            var dottedLine = lineObject.GetComponent<DottedLineRenderer>();
            if (dottedLine != null) dottedLine.ScaleMaterial();

            lineConnections.Add(new LineConnection(lineRenderer, null, from, to));
        }

        protected MapNode GetNode(Point p)
        {
            return MapNodes.FirstOrDefault(n => n.Node.point.Equals(p));
        }

        protected MapConfig GetConfig(string configName)
        {
            return allMapConfigs.FirstOrDefault(c => c.name == configName);
        }

        protected NodeBlueprint GetBlueprint(NodeType type)
        {
            var config = GetConfig(mapManager.CurrentMap.configName);
            return config.nodeBlueprints.FirstOrDefault(n => n.nodeType == type);
        }

        protected NodeBlueprint GetBlueprint(string blueprintName)
        {
            var config = GetConfig(mapManager.CurrentMap.configName);
            return config.nodeBlueprints.FirstOrDefault(n => n.name == blueprintName);
        }
    }
}
