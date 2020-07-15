using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
    [CustomEditor(typeof(ElecularMeshRenderer))]
    public class ElecularMeshRendererEditor : ChangeableElementEditor<ElecularMeshRenderer.MeshRendererVariationConfiguration> 
    {
        protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject, bool assigned)
        {
            var materials = config.FindPropertyRelative("materials");
            
            EditorGUILayout.LabelField("Materials");
            EditorGUI.indentLevel++;
            
            materials.arraySize = EditorGUILayout.IntField("Size", materials.arraySize);
            for (var count = 0; count < materials.arraySize; count++)
            {
                var material = materials.GetArrayElementAtIndex(count);
                EditorGUILayout.PropertyField(material);
            }
            EditorGUI.indentLevel--;
        }
        
        /// <inheritdoc />
        protected override void Initialize(SerializedProperty config, GameObject gameObject)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            
            var serializedMaterials = config.FindPropertyRelative("materials");
            serializedMaterials.arraySize = renderer.sharedMaterials.Length;
            for(var count = 0; count < renderer.sharedMaterials.Length; count++)
            {
                serializedMaterials.GetArrayElementAtIndex(count).objectReferenceValue = renderer.sharedMaterials[count];
            }
        }
    }	
}