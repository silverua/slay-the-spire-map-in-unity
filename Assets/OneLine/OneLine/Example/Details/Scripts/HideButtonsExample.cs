using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/HideButtonsExample")]
public class HideButtonsExample : ScriptableObject {
    [SerializeField, OneLine]
    private ArrayHidesButtons arrayWithElements;
    [SerializeField, OneLine]
    private ArrayHidesButtons zeroLengthArray;

    [Serializable]
    public class ArrayHidesButtons {
        [SerializeField, HideButtons]
        private string[] array;
    }
}
}