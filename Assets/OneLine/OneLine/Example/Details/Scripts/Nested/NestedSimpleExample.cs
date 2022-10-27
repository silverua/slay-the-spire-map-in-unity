using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples.Nested {
[CreateAssetMenu(menuName = "OneLine/Nested/Simple")]
public class NestedSimpleExample : ScriptableObject {
    [SerializeField]
    private Root root;

    [Serializable]
    public class Root {
        [SerializeField]
        private string first;

        [SerializeField]
        private Parent parent;

        [SerializeField]
        private int third;
    }

    [Serializable]
    public class Parent {
        [SerializeField, OneLine, Tooltip("A am drawn with OneLine.")]
        private Nested nested;

        [SerializeField, Range(0, 100)]
        private long second;
    }

    [Serializable]
    public class Nested {
        [SerializeField]
        private string first;

        [SerializeField, Tooltip("I am the second element.")]
        private string second;

        [SerializeField]
        private bool third;

        [SerializeField, Range(0, 100)]
        private long forth;
    }
}
}