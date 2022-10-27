using System.Collections;
using System;
using UnityEngine;
using OneLine;
using RectEx;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OneLine.Example {
	[CreateAssetMenu]
	public class SlicesTest : ScriptableObject {

		[OneLine]
		public Root root;
		public Root2 root2;

		[Serializable]
		public class First {
			public string first;
			public string second;
		}
		[Serializable]
		public class Second {
			public string[] array;
		}
		[Serializable]
		public class Root {
			public First first;
			[Separator]
			public Second second;
			[Separator]
			public string third;
		}

		[Serializable]
		public class Root2 {
			public First first;
			public Second second;
			public string third;
		}

#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(Root2))]
		public class Root2Editor : PropertyDrawer {

			public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label){
				rect = EditorGUI.PrefixLabel(rect, label);

				var separatorWidth = 8;
				var buttonWidth = 20;
				var rects = rect.Row(
					new float[]{1,1,0,1,1,0,0,0,1}, 
					new float[]{0,0,separatorWidth,0,0,buttonWidth,buttonWidth,separatorWidth,0}, 
					2);

				int i = 0;
				EditorGUI.TextField(rects[i++], "");
				EditorGUI.TextField(rects[i++], "");
				EditorGUI.DrawRect(rects[i++], Color.gray);
				EditorGUI.TextField(rects[i++], "");
				EditorGUI.TextField(rects[i++], "");
				GUI.Button(rects[i++], "+");
				GUI.Button(rects[i++], "-");
				EditorGUI.DrawRect(rects[i++], Color.gray);
				EditorGUI.TextField(rects[i++], "");
			}

		}
#endif
	}
}