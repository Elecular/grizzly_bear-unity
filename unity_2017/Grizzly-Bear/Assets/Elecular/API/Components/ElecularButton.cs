using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Add this component to a button to add variations.
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Button))]
	public class ElecularButton : ChangeableElement<ElecularButton.ButtonVariationConfiguration>
	{
		/// <summary>
		/// This is a list of variation configurations that define how the button will look like in each variation
		/// </summary>
		[SerializeField] 
		[HideInInspector]
		private List<ButtonVariationConfiguration> variations = new List<ButtonVariationConfiguration>();
		
		protected override void Setup(ButtonVariationConfiguration variationConfiguration)
		{
			var button = GetComponent<Button>();
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

		/// <inheritdoc />
		public override IEnumerable<ButtonVariationConfiguration> Configurations
		{
			get
			{
				return variations;
			}
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
	}
}
