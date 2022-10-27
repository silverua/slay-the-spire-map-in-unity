using System;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/Benchmarks/Array")]
public class ArrayBenchmark : ScriptableObject {

    [OneLine, HideLabel]
    public PureClasses[] pure;

    [Serializable]
    public class PureClasses {
        public int integerField;
        public long longField;
        public float floatField;
        [Range(0,1)]
        public double doubleField;
        public bool booleanField;
        public string stringField;
    }
}
}