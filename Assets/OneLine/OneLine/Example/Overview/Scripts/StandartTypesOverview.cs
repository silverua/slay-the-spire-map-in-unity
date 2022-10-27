using System;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Examples {
[CreateAssetMenu]
public class StandartTypesOverview : ScriptableObject {

    [Separator("Pure types"),OneLineWithHeader, HideLabel]
    public PureClasses pure;

    [Separator, Space(25), Separator("Colors and reference")]
    [OneLineWithHeader, HideLabel]
    public FirstUnity first;

    [Separator, Space(25), Separator("Enumerators and Curves")]
    [OneLineWithHeader, HideLabel]
    public SecondUnity second;

    [Separator, Space(25), Separator("Enumerators and Curves")]
    [OneLineWithHeader, HideLabel]
    public ThirdUnity third;

    [Separator, Space(25), Separator("Other")]
    [OneLine]
    public Matrix4x4 matrixField;
    [OneLine]
    public Rect rectField;
    [OneLine]
    public Bounds boundsField;
    

    [Serializable]
    public class PureClasses {
        public int integerField;
        public long longField;
        public float floatField;
        public double doubleField;
        public bool booleanField;
        public string stringField;
    }

    [Serializable]
    public class FirstUnity {
        public Color colorField;
        public Color32 color32Field;
        public ScriptableObject objectField;
    }

    [Serializable]
    public class SecondUnity{
        public LayerMask layerMaskField;
        public SystemLanguage enumField;
        public AnimationCurve curveField;
    }

    [Serializable]
    public class ThirdUnity {
        public float floatField;
        public Vector2 vector2Field;
        public Vector3 vector3Field;
        public Vector4 vector4Field;
    }



}
}