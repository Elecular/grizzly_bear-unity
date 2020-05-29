using System;
using System.Collections.Generic;
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
	public class ElecularButton : ChangeableElement
	{
		/// <summary>
		/// This is a list of variation configurations that define how the button will look like in each variation
		/// </summary>
		[SerializeField] 
		[HideInInspector]
		private List<ButtonVariationConfiguration> variations = new List<ButtonVariationConfiguration>();

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get
			{
				return variations.Cast<VariationConfiguration>();
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

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Button>();
			}

			/// <inheritdoc />
			public override void DisableTarget(GameObject gameObject)
			{
				((Button) GetTarget(gameObject)).enabled = false;
			}

			/// <inheritdoc />
			public override void EnableTarget(GameObject gameObject)
			{
				((Button) GetTarget(gameObject)).enabled = true;
			}

			/// <inheritdoc />
			public override void SetupTarget(GameObject gameObject)
			{
				var button = (Button)GetTarget(gameObject);
				button.transition = transition;
				button.colors = colorBlock;
				button.spriteState = spriteState;
				button.animationTriggers = animationTriggers;

				var graphic = button.targetGraphic as Image;
				if (graphic != null)
				{
					graphic.sprite = sourceImage;
				}
				else
				{
					Debug.LogWarning("ElecularButton only supports buttons with Image as their Target Graphic. Ignore this message if you do not want to change the Target Graphic. If you want to change the Target Grahpic, consider using an Image component or writing a custom script.");
				}
			}
		}
	}
}
