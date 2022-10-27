using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/OneLineExample")]
public class OneLineExample : ScriptableObject {

    [SerializeField, OneLine]
    private RootField root;
    [Space]
    [SerializeField, OneLine, HideLabel]
    private RootField[] rootArray;

    [Serializable]
    public class RootField {
        [SerializeField]
        private string first;
        [SerializeField]
        private Color second;
        [SerializeField]
        private NestedField third;
    }

    [Serializable]
    public class NestedField {
        [SerializeField, Tooltip("This tooltip is very useful")]
        private bool first;
        [SerializeField]
        private Vector2 second;
    }
}
}