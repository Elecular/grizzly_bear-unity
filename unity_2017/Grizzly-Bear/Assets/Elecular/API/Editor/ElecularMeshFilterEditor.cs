using UnityEditor;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularMeshFilter))]
	public class ElecularMeshFilterEditor : ChangeableElementEditor<ElecularMeshFilter.MeshFilterVariationConfiguration> 
	{
		protected override void DrawVariationConfiguration(SerializedProperty config)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(
				config.FindPropertyRelative("variationName").stringValue,
				EditorStyles.boldLabel
			);
			EditorGUI.indentLevel++;
			
			EditorGUILayout.PropertyField(config.FindPropertyRelative("mesh"));
			
			EditorGUI.indentLevel--;
		}
	}	
}

