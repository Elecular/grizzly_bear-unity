using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Add this component to a button to add variations.
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Button))]
	public class ElecularButton : MonoBehaviour, IChangeableElement 
	{
		[SerializeField]
		private Experiment experiment;
		
		/// <summary>
		/// This is a list of variation configurations that define how the button will look like in each variation
		/// </summary>
		[SerializeField] 
		[HideInInspector]
		private List<ButtonVariationConfiguration> variations = new List<ButtonVariationConfiguration>();

		private Button button;
		
		private void Start()
		{
			button = GetComponent<Button>();
			experiment.GetVariation(variation =>
			{
				var variationConfig = GetConfiguration(variation.Name);
				SetupButton(variationConfig);
			}, () =>
			{
				SetupButton(null);
			});
		}
		
		/// <summary>
		/// Sets the button properties based on the given configuration.
		/// If the configuration is null, it has no effect.
		/// </summary>
		/// <param name="variationConfiguration"></param>
		private void SetupButton(ButtonVariationConfiguration variationConfiguration)
		{
			if (variationConfiguration == null || !variationConfiguration.ExperimentName.Equals(experiment.ExperimentName))
			{
				Debug.LogError(string.Format("Could not set variation for button: {0}. Please check the ElecularButton Component.", name));
				return;
			}
			button.transition = variationConfiguration.Transition;
			button.colors = variationConfiguration.ColorBlock;
			button.spriteState = variationConfiguration.SpriteState;
			button.animationTriggers = variationConfiguration.AnimationTriggers;
			var graphic = button.targetGraphic as Image;
			if (graphic != null)
			{
				graphic.sprite = variationConfiguration.SourceImage;
			}
			else
			{
				Debug.LogWarning("ElecularButton only supports buttons with Image as their Target Graphic. Ignore this message if you do not want to change the Target Graphic. If you want to change the Target Grahpic, consider using an Image component or writing a custom script.");
			}
		}
		
		/// <summary>
		/// Finds the configuration of given variation.
		/// Returns null if the configuration does not exist 
		/// </summary>
		/// <param name="variationName"></param>
		/// <returns></returns>
		private ButtonVariationConfiguration GetConfiguration(string variationName)
		{
			return variations.Find(variation => variation.VariationName.Equals(variationName));
		}
		
		/// <summary>
		/// This class defines how a button is going to look like in a certain variation
		/// </summary>
		[Serializable]
		public class ButtonVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			private Selectable.Transition transition;
			
			[SerializeField]
			private Sprite sourceImage;

			[SerializeField]
			private ColorBlock colorBlock;

			[SerializeField]
			private SpriteState spriteState;
			
			[SerializeField]
			private AnimationTriggers animationTriggers;

			public Selectable.Transition Transition
			{
				get { return transition; }
			}
			
			public ColorBlock ColorBlock
			{
				get { return colorBlock; }
			}

			public SpriteState SpriteState
			{
				get { return spriteState; }
			}

			public AnimationTriggers AnimationTriggers
			{
				get { return animationTriggers; }
			}

			public Sprite SourceImage
			{
				get { return sourceImage; }
			}
		}

		/// <inheritdoc />
		public Experiment Experiment
		{
			get
			{
				return experiment;
			}
		}

		/// <inheritdoc />
		public IEnumerable<VariationConfiguration> Configurations
		{
			get
			{
				return variations.Cast<VariationConfiguration>();
			}
		}
	}
}
