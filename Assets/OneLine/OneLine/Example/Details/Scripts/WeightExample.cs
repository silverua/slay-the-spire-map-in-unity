using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/WeightExample")]
public class WeightExample : ScriptableObject {
    [SerializeField, OneLine]
    private RootField root;

    [Serializable]
    public class RootField {
        [SerializeField, Weight(3)]
        private int first;
        [SerializeField, Weight(2)]
        private int second;
        [SerializeField] // by default weight == 1
        private int third;
        [SerializeField, Weight(10)]
        private NestedField forth;
    }

    [Serializable]
    public class NestedField {
        [SerializeField]
        private int first;
    }
}
}