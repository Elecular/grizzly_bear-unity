using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularImage))]
	public class ElecularImageEditor : ChangeableElementEditor<ElecularImage.ImageVariationConfiguration> {
		
		protected override void DrawVariationConfiguration(SerializedProperty variationConfiguration, GameObject gameObject)
		{
			var serializedSourceImage = variationConfiguration.FindPropertyRelative("sourceImage");
			EditorGUILayout.PropertyField(serializedSourceImage);
			
			var serializedColor = variationConfiguration.FindPropertyRelative("color");
			EditorGUILayout.PropertyField(serializedColor);
			
			var serializedMaterial = variationConfiguration.FindPropertyRelative("material");
			EditorGUILayout.PropertyField(serializedMaterial);
		}

		/// <inheritdoc />
		protected override void Initialize(SerializedProperty config, GameObject gameObject)
		{
			var image = gameObject.GetComponent<Image>();
			config.FindPropertyRelative("sourceImage").objectReferenceValue = image.sprite;
			config.FindPropertyRelative("color").colorValue = image.color;
		}
	}

}
