using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularTransform))]
	public class ElecularTransformEditor : ChangeableElementEditor<ElecularTransform.TransformVariationConfiguration>  
	{
		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject)
		{
			EditorGUILayout.PropertyField(config.FindPropertyRelative("position"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("rotation"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("scale"));
		}

		protected override void Initialize(SerializedProperty config, GameObject gameObject)
		{
			var transform = gameObject.GetComponent<Transform>();
			config.FindPropertyRelative("position").vector3Value = transform.localPosition;
			config.FindPropertyRelative("rotation").vector3Value = transform.localEulerAngles;
			config.FindPropertyRelative("scale").vector3Value = transform.localScale;
		}
	}	
}
