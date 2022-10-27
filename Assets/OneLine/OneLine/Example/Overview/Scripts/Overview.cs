using System.Collections;
using System;
using UnityEngine;

namespace OneLine.Examples {
    [CreateAssetMenu]
    public class Overview : ScriptableObject {

#region One Line
        [Separator(" Hey There! ")]
        [SerializeField, OneLine]
        private OneLineOneField letMeTellYouAboutTheOneLineAttribute;
        [SerializeField, OneLine]
        private OneLineSecondLine itWillDrawAnyFieldYouWantIntoOneLineWithAllChildren;
        [SerializeField, OneLine]
        private OneLineWithSpace oneLineDrawsAllSpaceAttributesInYourCode;
        [SerializeField, OneLine, Tooltip("I'm a tooltip on the ROOT FIELD")]
        private OneLineWithTooltip tooltipAttribute;

        [Serializable]
        public class OneLineOneField {
            [SerializeField]
            private string first;
        }
        [Serializable]
        public class OneLineSecondLine {
            [SerializeField, Width(30)]
            private string first;
            [SerializeField, Width(50)]
            private string second;
            [SerializeField, Width(45)]
            private string third;
            [SerializeField, Width(30)]
            private string forth;
            [SerializeField, Width(40)]
            private string fifth;
            [SerializeField, Width(55)]
            private string sixth;
            [SerializeField, Width(35)]
            private string seventh;
            [SerializeField, Width(35)]
            private string eighth;
            [SerializeField, Separator]
            private string ninth;
        }
        [Serializable]
        public class OneLineWithSpace {
            [SerializeField]
            private string first;
            [Space(25)]
            [SerializeField]
            private string second;
        }
        [Serializable]
        public class OneLineWithTooltip {
            [SerializeField, Weight(0.8f)]
            private string first;
            [SerializeField, Tooltip("I'm here -- tooltip on the second NESTED FIELD!")]
            private string second;
            [SerializeField]
            private string third;
        }
#endregion

#region Weight and Width
        [Space, Separator("[ Weight ] and [ Width ]")]
        [SerializeField, OneLine]
        private WidthFirstLine oneLineCalculatesFieldWidthesBasedOnAttributes;
        [SerializeField, OneLine]
        private WidthWeightAttribute weightAttributeDeterminesRelativeWidth;
        [SerializeField, OneLine]
        private WidthWidthAttribute widthAttributeDeterminesAdditionalFixedWidth;

        [Serializable]
        public class WidthFirstLine {
            [SerializeField]
            private string first;
        }
        [Serializable]
        public class WidthWeightAttribute {
            [SerializeField, Weight(3)]
            private string first;
            [SerializeField, Weight(2)]
            private string second;
            [SerializeField, Weight(1)]
            private string third;
        }
        [Serializable]
        public class WidthWidthAttribute {
            [SerializeField, Width(75)]
            private string first;
            [SerializeField]
            private string second;
            [SerializeField, Width(50)]
            private string third;
        }
#endregion

#region Hide Label
        [Space, Separator("[ Customize you database ]")]
        [SerializeField]
        private string youCanCustomizeYouDatabaseByAttributes;
        [SerializeField, OneLine, HideLabel]
        private CustomizeHideLabel hideLabel;
        [SerializeField, OneLine]
        private CustomizeHighlightedFields highlightAttributeHelpsToPointOnMostImportantThings;
        [Space]
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private CustomizeHeader youCanAlsoDrawFieldsNameAboveLine;

        [Serializable]
        public class CustomizeHideLabel {
            [SerializeField]
            private string first;
            [SerializeField]
            private string second;
        }
        [Serializable]
        public class CustomizeHighlightedFields {
            [SerializeField]
            private string first;
            [SerializeField]
            private string second;
            [SerializeField, Highlight(0, 1, 0)]
            private string third;
            [SerializeField]
            private string fourth;
        }
        [Serializable]
        public class CustomizeHeader {
            [SerializeField]
            private string first;
            [SerializeField]
            private string second;
            [SerializeField]
            private string third;
        }
#endregion

#region Arrays
        [Separator("Nested Arrays, [Hide Buttons Attribute], [Array Length]")]
        [SerializeField, OneLine]
        private ArraysOneArray oneLineDrawsAllNestedArraysIntoLine;
        [SerializeField, OneLine]
        private ArraysTwoArrays arraysHaveToHaveCustomWeightOrWidth;
        [SerializeField, OneLine]
        private ArraysComplexElements lookAtSeparatorBetweenComplexElementsOfArray;
        [SerializeField, OneLine]
        private ArraysHideButtons thisArrayWithHideButtonsAttributeHidesItsButtons;
        [SerializeField, OneLine]
        private ArraysImmutableLength thisArrayHasImmutableLengthByArrayLengthAttribute;

        [Serializable]
        public class ArraysOneArray {
            [SerializeField]
            private string[] array;
        }
        [Serializable]
        public class ArraysTwoArrays {
            [SerializeField, Highlight(1, 0, 0)]
            private string[] first;
            [SerializeField, Highlight(0, 1, 0), Width(55)]
            private string[] second;
        }
        [Serializable]
        public class ArraysComplexElements {
            [SerializeField]
            private NestedField[] array;
            [Serializable]
            public class NestedField {
                [SerializeField]
                private string first;
                [SerializeField]
                private string second;
            }
        }
        [Serializable]
        public class ArraysHideButtons {
            [SerializeField, HideButtons]
            private string[] array;
        }
        [Serializable]
        public class ArraysImmutableLength {
            [SerializeField, ArrayLength(3)]
            private string[] array;
        }


#endregion

#region Other
        [Space, Separator("Other ways")]
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private OneLineArrayThreeFields[] youCanDoSomethingLikeThis;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private OneLineArrayWithHeader[] orThisButIDoNotKnowWhyDoYouNeetId;

        [Serializable]
        public class OneLineArrayThreeFields {
            [SerializeField]
            private string first;
            [SerializeField]
            private string second;
            [SerializeField]
            private string third;
        }

        [Serializable]
        public class OneLineArrayWithHeader {
            [SerializeField, Width(50)]
            private string first;
            [SerializeField, Width(50)]
            private string[] array;
        }
#endregion

    }
}
