using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularText))]
	public class ElecularTextEditor : ChangeableElementEditor<ElecularText.TextVariationConfiguration> 
	{
		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty config)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(
				config.FindPropertyRelative("variationName").stringValue,
				EditorStyles.boldLabel
			);
			EditorGUI.indentLevel++;
			
			EditorGUILayout.PropertyField(config.FindPropertyRelative("text"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("font"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("fontStyle"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("fontSize"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("alignment"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("color"));

			EditorGUI.indentLevel--;
		}
		
		/// <inheritdoc />
		protected override void Initialize(SerializedProperty config)
		{
			config.FindPropertyRelative("text").stringValue = "New text";
			config.FindPropertyRelative("fontSize").intValue = 14;
			config.FindPropertyRelative("color").colorValue = Color.white;
		}
	}
}

