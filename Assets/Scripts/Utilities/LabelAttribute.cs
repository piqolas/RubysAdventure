using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace piqey.Utilities.Editor
{
	public class LabelAttribute : PropertyAttribute
	{
		public string Label;

		public LabelAttribute(string label) =>
			Label = label;
	}

	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(LabelAttribute))]
	public class LabelDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label.text = ((LabelAttribute)attribute).Label;
			EditorGUI.PropertyField(position, property, label, true);
		}
	}
	#endif
}
