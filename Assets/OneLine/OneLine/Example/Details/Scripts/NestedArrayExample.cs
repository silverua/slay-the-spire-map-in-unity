using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/NestedArrayExample")]
public class NestedArrayExample : ScriptableObject {
    [SerializeField, OneLine]
    private NestedArray nestedArray;

    [Serializable]
    public class NestedArray {
        [SerializeField]
        private string[] array;
    }
}
}