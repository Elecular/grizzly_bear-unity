using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularMeshFilter))]
	public class ElecularMeshFilterEditor : ChangeableElementEditor<ElecularMeshFilter.MeshFilterVariationConfiguration> 
	{
		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject, bool assigned)
		{
			EditorGUILayout.PropertyField(config.FindPropertyRelative("mesh"));
		}

		/// <inheritdoc />
		protected override void Initialize(SerializedProperty config, GameObject gameObject)
		{
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			config.FindPropertyRelative("mesh").objectReferenceValue = meshFilter.sharedMesh;
		}
	}	
}

