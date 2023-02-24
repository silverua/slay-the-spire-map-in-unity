using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapViewUI : MapView
    {
        [Header("UI Map Settings")]
        [SerializeField] private ScrollRect scrollRectHorizontal;
        [SerializeField] private ScrollRect scrollRectVertical;
        [SerializeField] private float unitsToPixelsMultiplier  = 10f;

        protected override void ClearMap()
        {
            base.ClearMap();
        }

        public override void ShowMap(Map m)
        {
            base.ShowMap(m);
        }

        protected override void CreateMapBackground(Map m)
        {
            base.CreateMapBackground(m);
        }

        protected override void AddLineConnection(MapNode @from, MapNode to)
        {
            base.AddLineConnection(@from, to);
        }
    }
}