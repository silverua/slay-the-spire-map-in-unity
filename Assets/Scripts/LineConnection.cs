using UnityEngine;

[System.Serializable]
public class LineConnection
{
    public LineRenderer lr;
    public MapNode from;
    public MapNode to;

    public LineConnection(LineRenderer lr, MapNode from, MapNode to)
    {
        this.lr = lr;
        this.from = from;
        this.to = to;
    }

    public void SetColor(Color color)
    {
        // Debug.Log("In setcolor");
        lr.material.color = color;
        
        foreach (var key in lr.colorGradient.colorKeys)
        {
            var gradientColorKey = key;
            gradientColorKey.color = color;
        }
    }
}
