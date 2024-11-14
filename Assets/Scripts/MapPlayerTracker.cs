using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                Vector2Int currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                Node currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && CanTravelToNode(mapNode.Node, currentNode))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private static bool CanTravelToNode(Node to, Node from)
        {
            return from.outgoing.Any(point => point.Equals(to.point)) ||
                   from.point.y == to.point.y; 
            // (optionally use this instead of (from.point.y == to.point.y), but it can prevent movement to the side if the neighbor node is absent)
            // from.point.y == to.point.y && Mathf.Abs(from.point.x - to.point.x) == 1;
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            if (mapManager.CurrentMap.path.Count > 0 && mapManager.CurrentMap.path[^1].y == mapNode.Node.point.y)
            {
                mapManager.CurrentMap.path[^1] = mapNode.Node.point;
            }
            else
            {
                mapManager.CurrentMap.path.Add(mapNode.Node.point);
            }
            
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private static void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    break;
                case NodeType.EliteEnemy:
                    break;
                case NodeType.RestSite:
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    break;
                case NodeType.Boss:
                    break;
                case NodeType.Mystery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}