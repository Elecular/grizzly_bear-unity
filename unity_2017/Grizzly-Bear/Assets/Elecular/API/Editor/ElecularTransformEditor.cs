using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularTransform))]
	public class ElecularTransformEditor : ChangeableElementEditor<ElecularTransform.TransformVariationConfiguration>  
	{
		protected override void DrawElementHeader(GameObject gameObject)
		{
			EditorGUILayout.HelpBox("Always keep this component unfolded while modifying transform to save the transform state to the variations", MessageType.Warning);
		}

		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject, bool assigned)
		{
			EditorGUILayout.LabelField("Position: " + config.FindPropertyRelative("position").vector3Value);
			EditorGUILayout.LabelField("Rotation: " + config.FindPropertyRelative("rotation").vector3Value);
			EditorGUILayout.LabelField("Scale: " + config.FindPropertyRelative("scale").vector3Value);

			if (assigned)
			{
				config.FindPropertyRelative("position").vector3Value = gameObject.transform.position;
				config.FindPropertyRelative("rotation").vector3Value = gameObject.transform.localEulerAngles;
				config.FindPropertyRelative("scale").vector3Value = gameObject.transform.localScale;
			}
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
