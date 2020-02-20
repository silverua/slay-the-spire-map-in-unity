using System.Linq;
using UnityEngine;

public class MapPlayerTracker : MonoBehaviour
{
    public MapManager mapManager;
    
    public static MapPlayerTracker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectNode(MapNode mapNode)
    {
        Debug.Log("Selected node: " + mapNode.Node.point);

        if (mapManager.CurrentMap.path.Count == 0)
        {
            // player has not selected the node yet, he can select any of the nodes with y = 0
            if(mapNode.Node.point.y == 0)
                mapManager.CurrentMap.path.Add(mapNode.Node.point);
            else 
                PlayWarningThatNodeCannotBeAccessed();
        }
        else
        {
            var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
            var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

            if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                mapManager.CurrentMap.path.Add(mapNode.Node.point);
            else 
                PlayWarningThatNodeCannotBeAccessed();
        }
    }

    private void PlayWarningThatNodeCannotBeAccessed()
    {
        Debug.Log("Selected node cannot be accessed");
    }
}
