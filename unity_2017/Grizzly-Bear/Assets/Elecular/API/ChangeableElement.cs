using System.Collections.Generic;
using System.Linq;
using Elecular.api;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// A Changeable element is an element that can change itself based on a variation.
	/// Example Buttons, Images, Text etc
	/// </summary>
	public abstract class ChangeableElement : MonoBehaviour
	{
		[SerializeField]
		protected Experiment experiment;

		private void Awake()
		{
			OnAwake();
			Setup();
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
				variationConfig.DisableTarget(gameObject);
				variationConfig.SetupTarget(gameObject);
				variationConfig.EnableTarget(gameObject);
			}, () =>
			{
				Debug.LogError(string.Format("Could not set variation for button: {0}. Please check the ElecularButton Component.", name));
			});
		}

		/// <summary>
		/// The variation configurations for this changeable element
		/// </summary>
		public abstract IEnumerable<VariationConfiguration> Configurations { get; }
		
		/// <summary>
		/// Finds the configuration of given variation.
		/// Returns null if the configuration does not exist 
		/// </summary>
		/// <param name="variationName"></param>
		/// <returns></returns>
		private VariationConfiguration GetConfiguration(string variationName)
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
		
		/// <summary>
		/// Called on awake. Use this instead of Awake function
		/// </summary>
		protected virtual void OnAwake() {}
		
#if UNITY_EDITOR
		
		/// <summary>
		/// This is used by editor to preview the variation change on the element
		/// Do not use in production.
		/// </summary>
		public void Preview(string variationName)
		{
			var variationConfig = GetConfiguration(variationName);
			UnityEditor.Undo.RecordObject(variationConfig.GetTarget(gameObject), "Previewed Variation");
			variationConfig.DisableTarget(gameObject);
			variationConfig.SetupTarget(gameObject);
			variationConfig.EnableTarget(gameObject);
		}

#endif
	}
}

