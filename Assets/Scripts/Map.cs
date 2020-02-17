using System.Collections.Generic;
using System.Linq;

public class Map
{
    public List<Node> nodes;
    public List<Point> path;

    public Map(List<Node> nodes, List<Point> path)
    {
        this.nodes = nodes;
        this.path = path;
    }

    public Node GetNode(Point point)
    {
        return nodes.FirstOrDefault(n => n.point.Equals(point));
    }
}
