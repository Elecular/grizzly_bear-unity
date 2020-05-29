
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
		private string activeVariation;
		
		public override void OnInspectorGUI()
		{
			if (!(target is ChangeableElement))
			{
				EditorGUILayout.HelpBox("Target for a ChangeableElementEditor must be an Changeable Element", MessageType.Error);
				return;
			}
			
			//Experiment field
			var experimentProperty = serializedObject.FindProperty("experiment");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(experimentProperty);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				UpdateVariationConfigurations();
			}

			var hasExperimentNameChanged = HasExperimentNameChanged();
			
			//Reset button to reload all configurations
			EditorGUILayout.Space();
			if (hasExperimentNameChanged)
			{
				EditorGUILayout.HelpBox(
					"It looks like you have changed the experiment name. Can you please click the Reset button to update the variations.", 
					MessageType.Error
				);
			}

			if (!hasExperimentNameChanged)
			{
				//Drawing all the variation configurations
				var element = target as ChangeableElement;
				var serializedVariations = serializedObject.FindProperty("variations");
				if (serializedVariations.arraySize == 0 && element.Experiment != null)
				{
					EditorGUILayout.HelpBox("Could not download experiment data. Please check if your project/experiment id are valid, and click the Reset button to load the variations again", MessageType.Error);
				}
				
				UpdateActiveVariation();
			
				for (var count = 0; count < serializedVariations.arraySize; count++)
				{
					var serializedVariation = serializedVariations.GetArrayElementAtIndex(count);
					var variationName = serializedVariation.FindPropertyRelative("variationName").stringValue;
					var assigned = variationName.Equals(activeVariation);
					
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(
						variationName + (assigned ? " (Assigned)" : ""),
						assigned ? EditorStyles.boldLabel : EditorStyles.label
					);
					EditorGUI.indentLevel++;
				
					DrawVariationConfiguration(serializedVariation, element.gameObject);

					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUI.indentLevel--;
				}	
			}

			serializedObject.ApplyModifiedProperties();

			if (GUILayout.Button("Reset"))
			{
				UpdateVariationConfigurations();
			}
		}

		private void UpdateVariationConfigurations()
		{
			var element = ((ChangeableElement) target);
			var experiment = element.Experiment;
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
					Initialize(serializedVariation, element.gameObject);
				}
				serializedObject.ApplyModifiedProperties();
			}, () =>
			{
				serializedVariations.ClearArray();
				serializedObject.ApplyModifiedProperties();
			});
		}

		private void UpdateActiveVariation()
		{
			var element = ((ChangeableElement) target);
			var experiment = element.Experiment;

			if (experiment == null)
			{
				activeVariation = "";
				return;
			}
			
			experiment.GetVariation(variation =>
			{
				activeVariation = variation.Name;
				Repaint();
			});
		}
		
		private bool HasExperimentNameChanged()
		{
			var element = (ChangeableElement) target;
			if (element.Experiment == null) return false;
			var experimentName = element.Experiment.ExperimentName;
			var variations = element.Configurations;
			return variations.Any(variation => !variation.ExperimentName.Equals(experimentName));
		}
		
		/// <summary>
		/// Draws the configuration for the changeable element
		/// </summary>
		protected abstract void DrawVariationConfiguration(SerializedProperty variationConfiguration, GameObject gameObject);
		
		/// <summary>
		/// Used for defining default values of the variation configurations
		/// </summary>
		/// <param name="variationConfiguration"></param>
		protected virtual void Initialize(SerializedProperty variationConfiguration, GameObject gameObject)
		{
		}
	}
}

