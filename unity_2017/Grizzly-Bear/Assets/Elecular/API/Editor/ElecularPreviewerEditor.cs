using System;
using System.Linq;
using Elecular.api;
using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularPreviewer))]
	public class ElecularPreviewerEditor : Editor
	{
		private string previewVariation;

		private void OnEnable()
		{
			UpdatePreviewVariation();
		}

		public override void OnInspectorGUI()
		{
			var serializedExperiment = serializedObject.FindProperty("experiment");
			var previewer = (ElecularPreviewer) target;
			var experiment = previewer.Experiment;
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedExperiment);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				UpdatePreviewVariation();
			}

			if (previewer.gameObject.scene.name == null)
			{
				EditorGUILayout.HelpBox("Cannot preview variation on prefabs. Please drag this object in a scene to preview", MessageType.Warning);
				return;
			}
			
			EditorGUILayout.HelpBox("If you would like to preview another variation. Click on the experiment and set a test variation.", MessageType.Info);
			
			if (previewVariation != null && !previewVariation.Equals("") && GUILayout.Button(string.Format("Preview {0}", previewVariation)))
			{
				var gameObject = ((ElecularPreviewer) target).gameObject;
				var elements = gameObject.GetComponents<ChangeableElement>()
					.Where(element => element.Experiment == experiment);
				foreach (var element in elements)
				{
					element.Preview(previewVariation);
				}
			}

		}

		private void UpdatePreviewVariation()
		{
			var previewer = target as ElecularPreviewer;
			if (previewer == null || previewer.Experiment == null)
			{
				previewVariation = "";
				return;
			}
			
			previewer.Experiment.GetVariation(variation =>
			{
				previewVariation = variation.Name;
				Repaint();
			});
		}
		
	}	
}
