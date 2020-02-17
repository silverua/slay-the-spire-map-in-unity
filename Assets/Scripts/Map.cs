using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class Map
{
    public List<Node> nodes;
    public List<Point> path;

    public Map(List<Node> nodes, List<Point> path)
    {
        this.nodes = nodes;
        this.path = path;
    }

    public Node GetBossNode()
    {
        return nodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
    }

    public Node GetNode(Point point)
    {
        return nodes.FirstOrDefault(n => n.point.Equals(point));
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
