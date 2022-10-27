using System;
using System.Collections.Generic;
using UnityEngine;

namespace OneLine.Examples {
[CreateAssetMenu(menuName = "OneLine/Benchmarks/Separate Fields")]
public class SeparateFieldsBenchmark : ScriptableObject {

    [Serializable]
    public class Field {
        public int integerField;
        public long longField;
        public float floatField;
        [Range(0,1)]
        public double doubleField;
        public bool booleanField;
        public string stringField;
    }

    /*
     * Generate with vim:
     * qa         // Start macros recording
     * Y          // Copy line
     * p          // Paste line
     * Ctrl+a     // Increment
     * q          // Save macros
     *
     * 100@a -- paste 100 lines
     */
    [OneLine, HideLabel] public Field field1;
    [OneLine, HideLabel] public Field field2;
    [OneLine, HideLabel] public Field field3;
    [OneLine, HideLabel] public Field field4;
    [OneLine, HideLabel] public Field field5;
    [OneLine, HideLabel] public Field field6;
    [OneLine, HideLabel] public Field field7;
    [OneLine, HideLabel] public Field field8;
    [OneLine, HideLabel] public Field field9;
    [OneLine, HideLabel] public Field field10;
    [OneLine, HideLabel] public Field field11;
    [OneLine, HideLabel] public Field field12;
    [OneLine, HideLabel] public Field field13;
    [OneLine, HideLabel] public Field field14;
    [OneLine, HideLabel] public Field field15;
    [OneLine, HideLabel] public Field field16;
    [OneLine, HideLabel] public Field field17;
    [OneLine, HideLabel] public Field field18;
    [OneLine, HideLabel] public Field field19;
    [OneLine, HideLabel] public Field field20;
    [OneLine, HideLabel] public Field field21;
    [OneLine, HideLabel] public Field field22;
    [OneLine, HideLabel] public Field field23;
    [OneLine, HideLabel] public Field field24;
    [OneLine, HideLabel] public Field field25;
    [OneLine, HideLabel] public Field field26;
    [OneLine, HideLabel] public Field field27;
    [OneLine, HideLabel] public Field field28;
    [OneLine, HideLabel] public Field field29;
    [OneLine, HideLabel] public Field field30;
    [OneLine, HideLabel] public Field field31;
    [OneLine, HideLabel] public Field field32;
    [OneLine, HideLabel] public Field field33;
    [OneLine, HideLabel] public Field field34;
    [OneLine, HideLabel] public Field field35;
    [OneLine, HideLabel] public Field field36;
    [OneLine, HideLabel] public Field field37;
    [OneLine, HideLabel] public Field field38;
    [OneLine, HideLabel] public Field field39;
    [OneLine, HideLabel] public Field field40;
    [OneLine, HideLabel] public Field field41;
    [OneLine, HideLabel] public Field field42;
    [OneLine, HideLabel] public Field field43;
    [OneLine, HideLabel] public Field field44;
    [OneLine, HideLabel] public Field field45;
    [OneLine, HideLabel] public Field field46;
    [OneLine, HideLabel] public Field field47;
    [OneLine, HideLabel] public Field field48;
    [OneLine, HideLabel] public Field field49;
    [OneLine, HideLabel] public Field field50;
    [OneLine, HideLabel] public Field field51;
    [OneLine, HideLabel] public Field field52;
    [OneLine, HideLabel] public Field field53;
    [OneLine, HideLabel] public Field field54;
    [OneLine, HideLabel] public Field field55;
    [OneLine, HideLabel] public Field field56;
    [OneLine, HideLabel] public Field field57;
    [OneLine, HideLabel] public Field field58;
    [OneLine, HideLabel] public Field field59;
    [OneLine, HideLabel] public Field field60;
    [OneLine, HideLabel] public Field field61;
    [OneLine, HideLabel] public Field field62;
    [OneLine, HideLabel] public Field field63;
    [OneLine, HideLabel] public Field field64;
    [OneLine, HideLabel] public Field field65;
    [OneLine, HideLabel] public Field field66;
    [OneLine, HideLabel] public Field field67;
    [OneLine, HideLabel] public Field field68;
    [OneLine, HideLabel] public Field field69;
    [OneLine, HideLabel] public Field field70;
    [OneLine, HideLabel] public Field field71;
    [OneLine, HideLabel] public Field field72;
    [OneLine, HideLabel] public Field field73;
    [OneLine, HideLabel] public Field field74;
    [OneLine, HideLabel] public Field field75;
    [OneLine, HideLabel] public Field field76;
    [OneLine, HideLabel] public Field field77;
    [OneLine, HideLabel] public Field field78;
    [OneLine, HideLabel] public Field field79;
    [OneLine, HideLabel] public Field field80;
    [OneLine, HideLabel] public Field field81;
    [OneLine, HideLabel] public Field field82;
    [OneLine, HideLabel] public Field field83;
    [OneLine, HideLabel] public Field field84;
    [OneLine, HideLabel] public Field field85;
    [OneLine, HideLabel] public Field field86;
    [OneLine, HideLabel] public Field field87;
    [OneLine, HideLabel] public Field field88;
    [OneLine, HideLabel] public Field field89;
    [OneLine, HideLabel] public Field field90;
    [OneLine, HideLabel] public Field field91;
    [OneLine, HideLabel] public Field field92;
    [OneLine, HideLabel] public Field field93;
    [OneLine, HideLabel] public Field field94;
    [OneLine, HideLabel] public Field field95;
    [OneLine, HideLabel] public Field field96;
    [OneLine, HideLabel] public Field field97;
    [OneLine, HideLabel] public Field field98;
    [OneLine, HideLabel] public Field field99;
    [OneLine, HideLabel] public Field field100;
}
}