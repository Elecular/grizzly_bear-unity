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

			var matchX = Math.Abs(transform.anchorMin.x - transform.anchorMax.x) <= Mathf.Epsilon;
			var matchY = Math.Abs(transform.anchorMin.y - transform.anchorMax.y) <= Mathf.Epsilon;
			var pivotWidth = transform.pivot.x * transform.rect.width;
			var pivotHeight = transform.pivot.y * transform.rect.height;
			
			var offsetMin = config.FindPropertyRelative("offsetMin").vector2Value;
			var offsetMax = config.FindPropertyRelative("offsetMax").vector2Value;

			if (matchX && matchY)
			{
				EditorGUILayout.BeginHorizontal();
				offsetMin.x = EditorGUILayout.FloatField("PosX", offsetMin.x + pivotWidth) - pivotWidth;
				offsetMin.y = EditorGUILayout.FloatField("PosY", offsetMin.y + pivotHeight) - pivotHeight;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				offsetMax.x = EditorGUILayout.FloatField("Width", offsetMax.x - offsetMin.x) + offsetMin.x ;
				offsetMax.y = EditorGUILayout.FloatField("Height", offsetMax.y - offsetMin.y) + offsetMin.y;
				EditorGUILayout.EndHorizontal();
			}
			else if (!matchX && !matchY)
			{
				EditorGUILayout.BeginHorizontal();
				offsetMin.x = EditorGUILayout.FloatField("Left", offsetMin.x);
				offsetMax.y = -EditorGUILayout.FloatField("Top", -offsetMax.y);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				offsetMax.x = -EditorGUILayout.FloatField("Right", -offsetMax.x);
				offsetMin.y = EditorGUILayout.FloatField("Bottom", offsetMin.y);
				EditorGUILayout.EndHorizontal();
			}
			else if (matchX)
			{
				EditorGUILayout.BeginHorizontal();
				offsetMin.x = EditorGUILayout.FloatField("PosX", offsetMin.x + pivotWidth) - pivotWidth;
				offsetMax.y = -EditorGUILayout.FloatField("Top", -offsetMax.y);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				offsetMax.x = EditorGUILayout.FloatField("Width", offsetMax.x - offsetMin.x) + offsetMin.x ;
				offsetMin.y = EditorGUILayout.FloatField("Bottom", offsetMin.y);
				EditorGUILayout.EndHorizontal();
			}
			else if (matchY)
			{
				EditorGUILayout.BeginHorizontal();
				offsetMin.x = EditorGUILayout.FloatField("Left", offsetMin.x);
				offsetMin.y = EditorGUILayout.FloatField("PosY", offsetMin.y + pivotHeight) - pivotHeight;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				offsetMax.x = -EditorGUILayout.FloatField("Right", -offsetMax.x);
				offsetMax.y = EditorGUILayout.FloatField("Height", offsetMax.y - offsetMin.y) + offsetMin.y;
				EditorGUILayout.EndHorizontal();
			}
			
			config.FindPropertyRelative("offsetMin").vector2Value = offsetMin;
			config.FindPropertyRelative("offsetMax").vector2Value = offsetMax;
		}

		protected override void Initialize(SerializedProperty variationConfiguration, GameObject gameObject)
		{
			var rectTransform = gameObject.GetComponent<RectTransform>();
			variationConfiguration.FindPropertyRelative("offsetMin").vector2Value = rectTransform.offsetMin;
			variationConfiguration.FindPropertyRelative("offsetMax").vector2Value = rectTransform.offsetMax;
		}
	}	
}
