using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(Experiment))]
	public class ExperimentEditor : Editor
	{
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
			var serializedVariations = serializedObject.FindProperty("variations");
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedExperimentName);
			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				UpdateAllVariations();
			}
			
			//Loading Variations
			if (serializedVariations.arraySize == 0)
			{
				serializedForceVariation.boolValue = false;
				EditorGUILayout.HelpBox("Could not download experiment. Please check if your project id and experiment name are valid.", MessageType.Error);
				return;
			}
			
			//Drawing variation dropdown
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("If you want to test your game with a specific variation, you can forcefully set the variation. This will only impact the editor. It will NOT have any impact in the shipped game", MessageType.Info);
			var forceVariation = EditorGUILayout.Toggle("Set Variation", serializedForceVariation.boolValue);
			serializedForceVariation.boolValue = forceVariation;
			
			if (forceVariation)
			{
				var variations = GetVariations();
				var selectedVariation = serializedSelectedVariation.stringValue;
				var selectedVariationIndex = Mathf.Max(0, variations.IndexOf(selectedVariation));
				
				var newIndex = EditorGUILayout.Popup(selectedVariationIndex, variations.ToArray());
				serializedSelectedVariation.stringValue = variations[newIndex];
			}
			serializedObject.ApplyModifiedProperties();
		}

		private void UpdateAllVariations()
		{
			var experiment = (Experiment)target;
			experiment.GetAllVariations(variations =>
			{
				SetVariations(variations.Select(variation => variation.Name).ToArray());
			}, () =>
			{
				SetVariations(new string[]{});
			});
		}

		private void SetVariations(string[] variations)
		{
			var serializedVariations = serializedObject.FindProperty("variations");
			serializedVariations.ClearArray();
			foreach (var variation in variations)
			{
				serializedVariations.InsertArrayElementAtIndex(0);
				serializedVariations.GetArrayElementAtIndex(0).stringValue = variation;
			}
			serializedObject.ApplyModifiedProperties();
		}

		private List<string> GetVariations()
		{
			var variations = new List<string>();
			var serializedVariations = serializedObject.FindProperty("variations");
			for (var count = 0; count < serializedVariations.arraySize; count++)
			{
				variations.Add(serializedVariations.GetArrayElementAtIndex(count).stringValue);
			}
			return variations;
		}
	}
}
