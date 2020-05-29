using System;
using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularRectTransform))]
	public class ElecularRectTransformEditor : ChangeableElementEditor<ElecularRectTransform.RectTransformVariationConfiguration> 
	{
		protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject)
		{
			var transform = gameObject.GetComponent<RectTransform>();
			
			var offsetMin = config.FindPropertyRelative("offsetMin").vector2Value;
			var offsetMax = config.FindPropertyRelative("offsetMax").vector2Value;
			
			var anchorMin = config.FindPropertyRelative("anchorMin").vector2Value;
			var anchorMax = config.FindPropertyRelative("anchorMax").vector2Value;
			
			var pivot = config.FindPropertyRelative("pivot").vector2Value;

			var rotation = config.FindPropertyRelative("rotation").vector3Value;
			var scale = config.FindPropertyRelative("scale").vector3Value;

			EditorGUILayout.LabelField(string.Format("Offset Min: {0}  Offset Max: {1}", offsetMin, offsetMax));
			EditorGUILayout.LabelField(string.Format("Anchor Min: {0}  Anchor Max: {1}", anchorMin, anchorMax));
			EditorGUILayout.LabelField("Pivot: " + pivot);
			EditorGUILayout.LabelField("Rotation: " + rotation);
			EditorGUILayout.LabelField("Scale: " + scale);
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(15 * EditorGUI.indentLevel);
			if (GUILayout.Button("Copy Transform Values"))
			{
				config.FindPropertyRelative("offsetMin").vector2Value = transform.offsetMin;
				config.FindPropertyRelative("offsetMax").vector2Value = transform.offsetMax;
			
				config.FindPropertyRelative("anchorMin").vector2Value = transform.anchorMin;
				config.FindPropertyRelative("anchorMax").vector2Value = transform.anchorMax;
			
				config.FindPropertyRelative("pivot").vector2Value = transform.pivot;

				config.FindPropertyRelative("rotation").vector3Value = transform.localEulerAngles;
				config.FindPropertyRelative("scale").vector3Value = transform.localScale;
			}
			GUILayout.EndHorizontal();
		}

		protected override void Initialize(SerializedProperty variationConfiguration, GameObject gameObject)
		{
			var rectTransform = gameObject.GetComponent<RectTransform>();
			variationConfiguration.FindPropertyRelative("offsetMin").vector2Value = rectTransform.offsetMin;
			variationConfiguration.FindPropertyRelative("offsetMax").vector2Value = rectTransform.offsetMax;
			
			variationConfiguration.FindPropertyRelative("anchorMin").vector2Value = rectTransform.anchorMin;
			variationConfiguration.FindPropertyRelative("anchorMax").vector2Value = rectTransform.anchorMax;
			
			variationConfiguration.FindPropertyRelative("pivot").vector2Value = rectTransform.pivot;

			variationConfiguration.FindPropertyRelative("rotation").vector3Value = rectTransform.localEulerAngles;
			variationConfiguration.FindPropertyRelative("scale").vector3Value = rectTransform.localScale;
		}
	}	
}
