using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/ExpandableExample")]
public class ExpandableExample : ScriptableObject {

    [SerializeField, Expandable]
    private UnityEngine.Object withoutOneLine;

    [SerializeField, OneLine]
    private TwoFields withOneLine;

    [Serializable]
    public class TwoFields {
        [SerializeField, ReadOnlyExpandable]
        private ScriptableObject first;
        [SerializeField, Expandable]
        private UnityEngine.Object second;
    }
}
}