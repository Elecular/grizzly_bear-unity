using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularButton))]
	public class ElecularButtonEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var experimentProperty = serializedObject.FindProperty("experiment");
			
			//Experiment field
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(experimentProperty);
			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				UpdateVariationConfigurations();
			}

			var serializedVariations = serializedObject.FindProperty("variations");
			if (serializedVariations.arraySize == 0)
			{
				EditorGUILayout.HelpBox("Could not load experiment data. Please check if your project id and experiment id are valid, and click the Reset button", MessageType.Error);
			}
			
			for (var count = 0; count < serializedVariations.arraySize; count++)
			{
				var serializedVariation = serializedVariations.GetArrayElementAtIndex(count);
				DrawVariationConfiguration(serializedVariation);
			}
			
			EditorGUILayout.Space();
			if (GUILayout.Button("Reset"))
			{
				UpdateVariationConfigurations();
			}
			
			serializedObject.ApplyModifiedProperties();
			
		}

		private void DrawVariationConfiguration(SerializedProperty serializedVariation)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(
				serializedVariation.FindPropertyRelative("variationName").stringValue,
				EditorStyles.boldLabel
			);
			EditorGUI.indentLevel++;
			
			//Transition
			var serializedTransition = serializedVariation.FindPropertyRelative("transition");
			var selectedTransition = (Selectable.Transition)EditorGUILayout.EnumPopup("Transition", (Selectable.Transition)serializedTransition.enumValueIndex);
			serializedTransition.enumValueIndex = (int) selectedTransition;
			
			if (selectedTransition == Selectable.Transition.ColorTint)
			{
				EditorGUILayout.PropertyField(serializedVariation.FindPropertyRelative("colorBlock"));
			}
			else if (selectedTransition == Selectable.Transition.SpriteSwap)
			{
				EditorGUILayout.PropertyField(serializedVariation.FindPropertyRelative("spriteState"));
			}
			else if (selectedTransition == Selectable.Transition.Animation)
			{
				EditorGUILayout.PropertyField(serializedVariation.FindPropertyRelative("animationTriggers"));
			}
			EditorGUI.indentLevel--;
		}

		private void UpdateVariationConfigurations()
		{
			var serializedVariations = serializedObject.FindProperty("variations");

			var experiment = ((ElecularButton) target).Experiment;
			if (experiment == null)
			{
				serializedVariations.ClearArray();
				serializedObject.ApplyModifiedProperties();
				return;
			}
			
			experiment.GetAllVariations(variations =>
			{
				serializedVariations.ClearArray();
				for (var count = 0; count < variations.Length; count++)
				{
					serializedVariations.InsertArrayElementAtIndex(count);
					var serializedVariation = serializedVariations.GetArrayElementAtIndex(count);
					serializedVariation
						.FindPropertyRelative("variationName")
						.stringValue = variations[count].Name;
				}
				serializedObject.ApplyModifiedProperties();
			}, () =>
			{
				serializedVariations.ClearArray();
				serializedObject.ApplyModifiedProperties();
			});
		}
	}
}

