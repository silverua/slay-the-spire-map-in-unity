using System;
using UnityEngine;
using OneLine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "OneLine/ExtendedClassTest")]
public class ExtendedClassTest : ScriptableObject {

    [SerializeField]
    private NestedClass root;

    [Serializable]
    public abstract class BaseClass {
        [OneLine]
        public List<Vector2> list;
    }

    [Serializable]
    public class NestedClass : BaseClass {
    }
}