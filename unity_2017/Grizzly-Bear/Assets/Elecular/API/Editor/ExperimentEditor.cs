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
			var experimentName = serializedObject.FindProperty("experimentName");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(experimentName);
			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				UpdateAllVariations();
			}
			
			//Loading Variations
			var variations = GetVariations();
			if (variations.Count == 0)
			{
				SetSelectedVariation(null);
				EditorGUILayout.HelpBox("Could not download experiment. Please check if your project id and experiment name are valid.", MessageType.Error);
				return;
			}
			
			//Drawing variations
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("If you want to test your game with a specific variation, you can forcefully set the variation. Forcefully setting the variation will only impact the editor. It will NOT have any impact in the shipped game", MessageType.Info);
			var forceSetVariation = EditorGUILayout.Toggle("Set Variation", GetForceVariation());
			SetForceVariation(forceSetVariation);
			
			if (forceSetVariation)
			{
				var variationIndex = variations.IndexOf(GetSelectedVariation());
				SetSelectedVariation(variations[EditorGUILayout.Popup(
					Mathf.Max(variationIndex, 0), 
					variations.ToArray()
				)]);
			}
			else
			{
				SetSelectedVariation(null);
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void UpdateAllVariations()
		{
			var experiment = (Experiment) target;
			experiment.GetAllVariations(variations =>
			{
				SetVariations(variations.Select(variation => variation.Name).ToArray());
			}, () =>
			{
				SetVariations(new string[]{});
			});
		}
		
		/// <summary>
		/// The developer can force set a variation in order to test the game with taht variation.
		/// </summary>
		/// <param name="force"></param>
		private void SetForceVariation(bool force)
		{
			var guid = GUID;
			EditorPrefs.SetBool(string.Format("elecular-{0}-force-variation", guid), force);
		}

		private bool GetForceVariation()
		{
			var guid = GUID;
			return EditorPrefs.GetBool(string.Format("elecular-{0}-force-variation", guid));
		}
		
		private void SetSelectedVariation(string variation)
		{
			var guid = GUID;
			EditorPrefs.SetString(string.Format("elecular-{0}-selected-variation", guid), variation);
		}

		private string GetSelectedVariation()
		{
			var guid = GUID;
			return EditorPrefs.GetString(string.Format("elecular-{0}-selected-variation", guid));
		}

		private void SetVariations(IList<string> variations)
		{
			var guid = GUID;
			EditorPrefs.SetInt(string.Format("elecular-{0}-variation-size", guid), variations.Count);
			for(var count = 0; count < variations.Count; count++)
			{
				EditorPrefs.SetString(string.Format("elecular-{0}-variation-{1}", guid, count), variations[count]);
			}
		}

		private IList<string> GetVariations()
		{
			var guid = GUID;
			var variations = new List<string>();
			var numberOfVariations = EditorPrefs.GetInt(string.Format("elecular-{0}-variation-size", guid));
			
			for(var count = 0; count < numberOfVariations; count++)
			{
				var variation = EditorPrefs.GetString(string.Format("elecular-{0}-variation-{1}", guid, count));
				variations.Add(variation);
			}
			return variations;
		}

		private string GUID
		{
			get
			{
				var expeirment = (Experiment) target;
				return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(expeirment));
			}
		}
	}
}
