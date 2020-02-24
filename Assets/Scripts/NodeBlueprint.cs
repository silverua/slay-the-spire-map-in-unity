using System;
using UnityEngine;

public enum NodeType
{
    MinorEnemy, 
    EliteEnemy,
    RestSite, 
    Treasure, 
    Store, 
    Boss, 
    Mystery
}

[CreateAssetMenu]
public class NodeBlueprint : ScriptableObject
{
    public Sprite sprite;
    public NodeType nodeType;

    public void EnterNode()
    {
        Debug.Log("Entering node: " + nodeType);
        // load appropriate scene with context based on nodeType:
        // or show appropriate GUI over the map: 
        // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
        switch (nodeType)
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
}
