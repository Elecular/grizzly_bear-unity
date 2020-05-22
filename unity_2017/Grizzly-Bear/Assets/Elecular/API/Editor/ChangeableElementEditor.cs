
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// This is an editor used for drawing Changeable Elements.
	/// This draws the configuration of the changeable element for each variation.
	/// </summary>
	public abstract class ChangeableElementEditor<T> : Editor where T : VariationConfiguration
	{
		public override void OnInspectorGUI()
		{
			if (!(target is ChangeableElement<T>))
			{
				EditorGUILayout.HelpBox("Target for a ChangeableElementEditor must be an Changeable Element", MessageType.Error);
				return;
			}
			
			//Experiment field
			var experimentProperty = serializedObject.FindProperty("experiment");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(experimentProperty);
			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				UpdateVariationConfigurations();
			}
			
			//Drawing all the variation configurations
			var serializedVariations = serializedObject.FindProperty("variations");
			if (serializedVariations.arraySize == 0)
			{
				EditorGUILayout.HelpBox("Could not download experiment data. Please check if your project/experiment id are valid, and click the Reset button to load the variations again", MessageType.Error);
			}
			for (var count = 0; count < serializedVariations.arraySize; count++)
			{
				var serializedVariation = serializedVariations.GetArrayElementAtIndex(count);
				DrawVariationConfiguration(serializedVariation);
			}
			

			serializedObject.ApplyModifiedProperties();
			
			//Reset button to reload all configurations
			EditorGUILayout.Space();
			if (HasExperimentNameChanged())
			{
				EditorGUILayout.HelpBox(
					"It looks like you have changed the experiment name. Can you please click the Reset button to update the variations.", 
					MessageType.Error
				);
			}
			if (GUILayout.Button("Reset"))
			{
				UpdateVariationConfigurations();
			}
		}

		private void UpdateVariationConfigurations()
		{
			var experiment = ((ChangeableElement<T>) target).Experiment;
			var serializedVariations = serializedObject.FindProperty("variations");
	
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
					serializedVariation
						.FindPropertyRelative("experimentName")
						.stringValue = experiment.ExperimentName;
				}
				serializedObject.ApplyModifiedProperties();
			}, () =>
			{
				serializedVariations.ClearArray();
				serializedObject.ApplyModifiedProperties();
			});
		}
		
		private bool HasExperimentNameChanged()
		{
			var element = (ChangeableElement<T>) target;
			if (element.Experiment == null) return false;
			var experimentName = element.Experiment.ExperimentName;
			var variations = element.Configurations;
			return variations.Any(variation => !variation.ExperimentName.Equals(experimentName));
		}
		
		/// <summary>
		/// Draws the configuration for the changeable element
		/// </summary>
		protected abstract void DrawVariationConfiguration(SerializedProperty variationConfiguration);
	}
}

