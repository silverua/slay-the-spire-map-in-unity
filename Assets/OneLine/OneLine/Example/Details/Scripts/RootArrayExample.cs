using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/RootArrayExample")]
public class RootArrayExample : ScriptableObject {
    [SerializeField, OneLine]
    private RootField[] rootArray;
    [Space]
    [SerializeField, OneLine]
    private SingleArray singleNestedArray;
    [SerializeField, OneLine]
    private TwoArrays twoNestedArrays;

    [Serializable]
    public class RootField {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
        [SerializeField]
        private string third;
    }
    [Serializable]
    public class SingleArray {
        [SerializeField]
        private string[] array;
    }
    [Serializable]
    public class TwoArrays {
        [SerializeField, Highlight(1, 0, 0)]
        private int[] first;
        [SerializeField, Highlight(0, 1, 0), Width(125)]
        private string[] second;
    }
}
}