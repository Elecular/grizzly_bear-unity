using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Attach this component to a Button to give it a variation
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class ElecularButton : MonoBehaviour
	{
		[SerializeField]
		private Experiment experiment;
		
		/// <summary>
		/// This is a list of variation configurations that define how the button will look like in each variation
		/// </summary>
		[SerializeField] 
		[HideInInspector]
		private List<VariationConfiguration> variations = new List<VariationConfiguration>();

		private Button button;

		private void Start()
		{
			button = GetComponent<Button>();
			experiment.GetVariation(variation =>
			{
				var config = FindConfiguration(variation.Name);
				SetupButton(config);
			}, () =>
			{
				SetupButton(null);
			});
		}

		private void SetupButton(VariationConfiguration config)
		{
			if (config == null)
			{
				Debug.LogError("There was an error while setting variation for button: " + gameObject.name + ". Please check your project/experiment id and click the reset button on ElecularButton component");
				return;
			}
			button.transition = config.Transition;
			button.colors = config.ColorBlock;
			button.spriteState = config.SpriteState;
			button.animationTriggers = config.AnimationTriggers;
		}

		private VariationConfiguration FindConfiguration(string variationName)
		{
			return variations.Find(variation => variation.Name.Equals(variationName));
		}

		/// <summary>
		/// Gets the experiment that this button is running on
		/// </summary>
		public Experiment Experiment
		{
			get
			{
				return experiment;
			}
		}

		/// <summary>
		/// A Variation configuration defines how a button looks like in a given variation
		/// </summary>
		[Serializable]
		public class VariationConfiguration
		{
			[SerializeField]
			private string experimentName;
			
			[SerializeField]
			private string variationName;
			
			[SerializeField]
			private Selectable.Transition transition;

			[SerializeField]
			private ColorBlock colorBlock;

			[SerializeField]
			private SpriteState spriteState;
			
			[SerializeField]
			private AnimationTriggers animationTriggers;

			/// <summary>
			/// Name of the variation
			/// </summary>
			public string Name
			{
				get { return variationName; }
			}
			
			/// <summary>
			/// Transition type used for this variation
			/// </summary>
			public Selectable.Transition Transition
			{
				get { return transition; }
			}
			
			/// <summary>
			/// ColorBlock used for this variation
			/// </summary>
			public ColorBlock ColorBlock
			{
				get { return colorBlock; }
			}
			
			/// <summary>
			/// SpriteState used for this variation
			/// </summary>
			public SpriteState SpriteState
			{
				get { return spriteState; }
			}
			
			/// <summary>
			/// Animation Triggers used for this variation
			/// </summary>
			public AnimationTriggers AnimationTriggers
			{
				get { return animationTriggers; }
			}
		}
	}	
}

