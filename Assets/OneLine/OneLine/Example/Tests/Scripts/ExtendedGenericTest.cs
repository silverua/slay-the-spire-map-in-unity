using System;
using UnityEngine;
using OneLine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "OneLine/ExtendedGenericTest")]
public class ExtendedGenericTest : ScriptableObject {

    [SerializeField]
    private ChildClass root;

    [Serializable]
    public abstract class BaseClass<T> {
        [OneLine]
        public List<T> list;
    }

    [Serializable]
    public class ChildClass : BaseClass<Vector2> {
    }
}