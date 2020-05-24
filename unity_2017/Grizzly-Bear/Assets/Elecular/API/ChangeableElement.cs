using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// A Changeable element is a element that can change itself based on a variation.
	/// </summary>
	public abstract class ChangeableElement<T> : MonoBehaviour where T: VariationConfiguration
	{
		[SerializeField]
		protected Experiment experiment;

		private void Awake()
		{
			Setup();
		}

		private void Start()
		{
			ElecularApi.Instance.RegisterOnNewSessionEvent(Setup);
		}

		private void OnDestroy()
		{
			ElecularApi.Instance.UnRegisterFromNewSessionEvent(Setup);
		}

		private void Setup()
		{
			experiment.GetVariation(variation =>
			{
				var variationConfig = GetConfiguration(variation.Name);
				if (variationConfig == null || !variationConfig.ExperimentName.Equals(experiment.ExperimentName))
				{
					Debug.LogError(string.Format("Could not set variation for button: {0}. Please check the ElecularButton Component.", name));
					return;
				}
				Setup(variationConfig);
			}, () =>
			{
				Debug.LogError(string.Format("Could not set variation for button: {0}. Please check the ElecularButton Component.", name));
			});
		}
		
		/// <summary>
		/// This method sets the element based on the give configuration
		/// </summary>
		protected abstract void Setup(T variationConfiguration);
		
		/// <summary>
		/// The variation configurations for this changeable element
		/// </summary>
		public abstract IEnumerable<T> Configurations { get; }
		
		/// <summary>
		/// Finds the configuration of given variation.
		/// Returns null if the configuration does not exist 
		/// </summary>
		/// <param name="variationName"></param>
		/// <returns></returns>
		private T GetConfiguration(string variationName)
		{
			return Configurations.FirstOrDefault(variation => variation.VariationName.Equals(variationName));
		}

		/// <summary>
		/// The experiment this element is part of
		/// </summary>
		public Experiment Experiment
		{
			get { return experiment; }
		}
	}
}

