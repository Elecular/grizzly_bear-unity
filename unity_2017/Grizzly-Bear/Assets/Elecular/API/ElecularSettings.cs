using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// This holds all the settings needed for integrating with Elecular
	/// </summary>
	public class ElecularSettings : ScriptableObject
	{
		public const string RESOURCE_PATH = @"Elecular/Settings";
		
		/// <summary>
		/// If the player is inactive for this amount of time (seconds), Elecular will start a new user session
		/// </summary>
		public const int SESSION_INACTIVE_TIME_THRESHOLD = 15 * 60;
		
		[SerializeField]
		private string projectId;

		[SerializeField]
		private List<Experiment> experiments;

		[NonSerialized]
		private static ElecularSettings instance;

		/// <summary>
		/// Gets the Singleton instance of Elecular Settings
		/// </summary>
		public static ElecularSettings Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<ElecularSettings>(RESOURCE_PATH);
				}
				return instance;
			}
		}

		/// <summary>
		/// Project Id of Elecular
		/// </summary>
		public string ProjectId
		{
			get
			{
				return projectId;
			}
		}
		
		/// <summary>
		/// Adds an experiment to ElecularSettings
		/// If you add an experiment to settings, Elecular will be able to cache the experiment when the app loads.
		/// This avoids Flickering effects
		/// </summary>
		/// <param name="experiment"></param>
		public void AddExperiment(Experiment experiment)
		{
			if (experiments.Contains(experiment)) return;
			experiments.Add(experiment);
		}

		/// <summary>
		/// Loads all experiments and puts the results in cache
		/// </summary>
		public void LoadAllExperiments()
		{
			foreach (var experiment in experiments)
			{
				if (experiment != null)
				{
					experiment.GetVariation(variation => {});
					if (Application.isEditor)
					{
						experiment.GetAllVariations(variations => {});
					}
				}
			}
		}

		private void OnValidate()
		{
			experiments = experiments.FindAll(experiment => experiment != null);
		}
	}	
}
