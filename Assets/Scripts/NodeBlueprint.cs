using System;
using UnityEngine;

public enum NodeType
{
    MinorEnemy, 
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
        switch (nodeType)
        {
            case NodeType.MinorEnemy:
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
