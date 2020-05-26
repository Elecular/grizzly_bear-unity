using UnityEditor;

namespace Elecular.API
{
    [CustomEditor(typeof(ElecularMeshRenderer))]
    public class ElecularMeshRendererEditor : ChangeableElementEditor<ElecularMeshRenderer.MeshRendererVariationConfiguration> 
    {
        protected override void DrawVariationConfiguration(SerializedProperty config)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(
                config.FindPropertyRelative("variationName").stringValue,
                EditorStyles.boldLabel
            );
            EditorGUI.indentLevel++;
    
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
            
            EditorGUI.indentLevel--;
        }
        
        /// <inheritdoc />
        protected override void Initialize(SerializedProperty config)
        {
            var materials = config.FindPropertyRelative("materials");
            materials.arraySize = 1;
        }
    }	
}