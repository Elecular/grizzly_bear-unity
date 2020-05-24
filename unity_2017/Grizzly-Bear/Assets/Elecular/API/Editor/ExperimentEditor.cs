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
			
			//Drawing variation dropdown
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("If you want to test your game with a specific variation, you can forcefully set the variation. This will only impact the editor. It will NOT have any impact in the shipped game", MessageType.Info);
			var forceSetVariation = EditorGUILayout.Toggle("Set Variation", IsAVariationForced());
			SetForceVariation(forceSetVariation);
			
			if (forceSetVariation)
			{
				var variationIndex = GetSelectedVariationIndex(variations);
				SetSelectedVariation(variations[EditorGUILayout.Popup(variationIndex,variations.ToArray())]);
			}
			else
			{
				SetSelectedVariation(null);
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
		
		/// <summary>
		/// The developer can force set a variation in order to test the game with that variation.
		/// </summary>
		/// <param name="force"></param>
		public void SetForceVariation(bool force)
		{
			var guid = GUID;
			EditorPrefs.SetBool(string.Format("elecular-{0}-force-variation", guid), force);
		}
		
		/// <summary>
		/// Returns true if the developer wants to set a variation for testing
		/// </summary>
		/// <returns></returns>
		public bool IsAVariationForced()
		{
			var guid = GUID;
			return EditorPrefs.GetBool(string.Format("elecular-{0}-force-variation", guid));
		}
		
		/// <summary>
		/// Sets the variation that needs to be tested in editor prefs
		/// </summary>
		/// <param name="variation"></param>
		public void SetSelectedVariation(string variation)
		{
			var guid = GUID;
			EditorPrefs.SetString(string.Format("elecular-{0}-selected-variation", guid), variation);
		}
		
		/// <summary>
		/// Gets the selected variation index for testing from editor prefs
		/// </summary>
		/// <param name="variations">All the variations</param>
		/// <returns></returns>
		public int GetSelectedVariationIndex(IList<string> variations)
		{
			var guid = GUID;
			var variation = EditorPrefs.GetString(string.Format("elecular-{0}-selected-variation", guid));
			return Mathf.Max(0, variations.IndexOf(variation));
		}
		
		/// <summary>
		/// Sets all the variations of this experiment in editor prefs
		/// </summary>
		/// <param name="variations"></param>
		public void SetVariations(IList<string> variations)
		{
			var guid = GUID;
			EditorPrefs.SetInt(string.Format("elecular-{0}-variation-size", guid), variations.Count);
			for(var count = 0; count < variations.Count; count++)
			{
				EditorPrefs.SetString(string.Format("elecular-{0}-variation-{1}", guid, count), variations[count]);
			}
		}
		
		/// <summary>
		/// Gets all the variations in this experiment from editor prefs
		/// </summary>
		/// <returns></returns>
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
		
		/// <summary>
		/// Gets the GUID of the target
		/// </summary>
		public string GUID
		{
			get
			{
				var expeirment = (Experiment) target;
				return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(expeirment));
			}
		}
	}
}
