using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/WidthExample")]
public class WidthExample : ScriptableObject {
    [SerializeField, OneLine]
    private RootField root;

    [Serializable]
    public class RootField {
        [SerializeField, Width(70)]
        private string first;
        [SerializeField] // by default width = 0
        private string second;
        [SerializeField, Weight(2), Width(25)]
        private string third;
        [SerializeField, Width(10000000)]
        private NestedField forth;
    }

    [Serializable]
    public class NestedField {
        [SerializeField]
        private string first;
    }
}
}