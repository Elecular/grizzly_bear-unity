using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elecular.API
{
	[CustomEditor(typeof(Experiment))]
	public class ExperimentEditor : Editor
	{
		[NonSerialized]
		private List<string> variations = null;
		
		private void OnEnable()
		{
			UpdateAllVariations();
		}

		public override void OnInspectorGUI()
		{
			//Experiment name field
			var serializedExperimentName = serializedObject.FindProperty("experimentName");
			var serializedForceVariation = serializedObject.FindProperty("forceVariation");
			var serializedSelectedVariation = serializedObject.FindProperty("selectedVariation");
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedExperimentName);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				UpdateAllVariations();
			}

			if (variations == null) return;
			
			//Loading Variations
			if (variations.Count == 0)
			{
				EditorGUILayout.HelpBox("Could not download experiment. Please check if your project id and experiment name are valid.", MessageType.Error);
				return;
			}
			
			//Drawing variation dropdown
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("If you want to test your game with a specific variation, you can forcefully set the variation.", MessageType.Info);
			
			EditorGUI.BeginChangeCheck();
			var forceVariation = EditorGUILayout.Toggle("Set Variation", serializedForceVariation.boolValue);
			serializedForceVariation.boolValue = forceVariation;
			
			if (forceVariation)
			{
				var selectedVariation = serializedSelectedVariation.stringValue;
				var selectedVariationIndex = Mathf.Max(0, variations.IndexOf(selectedVariation));
				
				var newIndex = EditorGUILayout.Popup(selectedVariationIndex, variations.ToArray());
				serializedSelectedVariation.stringValue = variations[newIndex];

				EditorGUILayout.HelpBox("Remember to uncheck this toggle before shipping to your players or else they will all experience the selected variation.", MessageType.Warning);
			}
			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				((Experiment) target).GetVariation(variation =>
				{
					UpdateAllElements(variation.Name);
				});
			}
		}

		private void UpdateAllVariations()
		{
			var experiment = (Experiment)target;
			var experimentName = experiment.ExperimentName;
			if (experiment.ExperimentName == null || experiment.ExperimentName.Equals("")) return;
			experiment.GetAllVariations(variations =>
			{
				this.variations = variations.Select(variation => variation.Name).ToList();
				Repaint();
			}, () => 
			{
				if(!experimentName.Equals(experiment.ExperimentName))
				{
					return;
				}
				variations = new List<string>();
				Repaint();
			});
		}

		private void UpdateAllElements(string variationName)
		{
			var experimentName = ((Experiment) target).ExperimentName;
			foreach (var element in GameObject.FindObjectsOfType<ChangeableElement>())
			{
				if (element.Experiment.ExperimentName.Equals(experimentName))
				{
					element.Preview(variationName);
				}
			}
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}
}
