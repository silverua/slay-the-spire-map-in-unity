using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Map
{
    [System.Serializable]
    public class LineConnection
    {
        public LineRenderer lr;
        public UILineRenderer uilr; 
        public MapNode from;
        public MapNode to;

        public LineConnection(LineRenderer lr, UILineRenderer uilr, MapNode from, MapNode to)
        {
            this.lr = lr;
            this.uilr = uilr;
            this.from = from;
            this.to = to;
        }

        public void SetColor(Color color)
        {
            if (lr != null)
            {
                Gradient gradient = lr.colorGradient;
                GradientColorKey[] colorKeys = gradient.colorKeys;
                for (int j = 0; j < colorKeys.Length; j++)
                {
                    colorKeys[j].color = color;
                }

                gradient.colorKeys = colorKeys;
                lr.colorGradient = gradient;
            }

            if (uilr != null) uilr.color = color;
        }
    }
}