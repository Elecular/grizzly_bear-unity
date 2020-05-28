using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

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

		/// <inheritdoc />
		protected override void Setup(ButtonVariationConfiguration variationConfiguration)
		{
			var button = (Button)variationConfiguration.GetTarget(gameObject);
			button.transition = variationConfiguration.transition;
			button.colors = variationConfiguration.colorBlock;
			button.spriteState = variationConfiguration.spriteState;
			button.animationTriggers = variationConfiguration.animationTriggers;

			var graphic = button.targetGraphic as Image;
			if (graphic != null)
			{
				graphic.sprite = variationConfiguration.sourceImage;
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
			public Selectable.Transition transition;
			
			[SerializeField]
			public Sprite sourceImage;

			[SerializeField]
			public ColorBlock colorBlock;

			[SerializeField]
			public SpriteState spriteState;
			
			[SerializeField]
			public AnimationTriggers animationTriggers;

			/// <inheritdoc />
			public override UnityEngine.Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Button>();
			}
		}
	}
}
