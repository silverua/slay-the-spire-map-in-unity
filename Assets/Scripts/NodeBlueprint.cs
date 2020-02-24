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
}
