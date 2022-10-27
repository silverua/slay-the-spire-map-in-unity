using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples.Nested {

[CreateAssetMenu(menuName = "OneLine/Nested/Array")]
public class NestedArrayExample : ScriptableObject {
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
        [SerializeField, OneLine]
        private Nested nested;

        [SerializeField, Range(0, 100)]
        private long second;
    }

    [Serializable]
    public class Nested {
        [SerializeField]
        private string first;

        [SerializeField]
        private string[] second;

        [SerializeField]
        private bool third;

        [SerializeField, Range(0, 100)]
        private long forth;
    }
}
}