using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/FixedLengthExample")]
public class FixedLengthExample : ScriptableObject {
    [SerializeField, OneLine]
    private ImmutableLengthArray arrayWithImmutableLength;

    [Serializable]
    public class ImmutableLengthArray {
        [SerializeField, ArrayLength(7)]
        private string[] array;
    }
}
}