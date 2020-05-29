using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularText))]
	public class ElecularTextEditor : ChangeableElementEditor<ElecularText.TextVariationConfiguration> 
	{
		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject)
		{
			EditorGUILayout.PropertyField(config.FindPropertyRelative("text"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("font"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("fontStyle"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("fontSize"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("alignment"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("color"));
		}
		
		/// <inheritdoc />
		protected override void Initialize(SerializedProperty config, GameObject gameObject)
		{
			var text = gameObject.GetComponent<Text>();
			config.FindPropertyRelative("text").stringValue = text.text;
			config.FindPropertyRelative("font").objectReferenceValue = text.font;
			config.FindPropertyRelative("fontStyle").enumValueIndex = (int) text.fontStyle;
			config.FindPropertyRelative("fontSize").intValue = text.fontSize;
			config.FindPropertyRelative("alignment").enumValueIndex = (int) text.alignment;
			config.FindPropertyRelative("color").colorValue = text.color;
		}
	}
}

