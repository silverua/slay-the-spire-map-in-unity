using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/HideLabelExample")]
public class HideLabelExample : ScriptableObject {
    [SerializeField, OneLine, HideLabel]
    private ThreeFields thisSelfDocumentedFieldNameWillNotBeShownInTheInspector;

    [Serializable]
    public class ThreeFields {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
        [SerializeField]
        private string third;
    }
}
}