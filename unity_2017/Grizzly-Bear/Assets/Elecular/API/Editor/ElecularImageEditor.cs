using UnityEditor;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularImage))]
	public class ElecularImageEditor : ChangeableElementEditor<ElecularImage.ImageVariationConfiguration> {
		
		protected override void DrawVariationConfiguration(SerializedProperty variationConfiguration)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(
				variationConfiguration.FindPropertyRelative("variationName").stringValue,
				EditorStyles.boldLabel
			);
			EditorGUI.indentLevel++;
			
			var serializedSourceImage = variationConfiguration.FindPropertyRelative("sourceImage");
			EditorGUILayout.PropertyField(serializedSourceImage);
			
			var serializedColor = variationConfiguration.FindPropertyRelative("color");
			EditorGUILayout.PropertyField(serializedColor);
			
			var serializedMaterial = variationConfiguration.FindPropertyRelative("material");
			EditorGUILayout.PropertyField(serializedMaterial);
			EditorGUI.indentLevel--;
		}
	}

}
