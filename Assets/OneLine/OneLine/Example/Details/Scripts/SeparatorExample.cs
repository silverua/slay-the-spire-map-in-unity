using System;
using UnityEngine;
using OneLine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/SeparatorExample")]
public class SeparatorExample : ScriptableObject {
    [SerializeField, OneLine]
    private TwoFields first;

    [Space]
    [SerializeField, Separator("[ Separator separates ]"), OneLine]
    private TwoFields second;

    [Serializable]
    public class TwoFields {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
    }
}
}