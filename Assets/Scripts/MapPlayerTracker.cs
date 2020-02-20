using System.Linq;
using UnityEngine;

public class MapPlayerTracker : MonoBehaviour
{
    public bool lockAfterSelecting = false;
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

        Debug.Log("Selected node: " + mapNode.Node.point);

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
            var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
            var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

            if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                SendPlayerToNode(mapNode);
            else
                PlayWarningThatNodeCannotBeAccessed();
        }
    }

    private void SendPlayerToNode(MapNode mapNode)
    {
        Locked = lockAfterSelecting;
        mapManager.CurrentMap.path.Add(mapNode.Node.point);
        view.SetAttainableNodes();
        mapNode.ShowSwirlAnimation();
    }

    private void PlayWarningThatNodeCannotBeAccessed()
    {
        Debug.Log("Selected node cannot be accessed");
    }
}
