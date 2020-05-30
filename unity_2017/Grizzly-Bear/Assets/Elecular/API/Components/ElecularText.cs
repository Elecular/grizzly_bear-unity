using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Add this component to a Text UI to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Text))]
	public class ElecularText : ChangeableElement
	{
		[HideInInspector]
		[SerializeField]
		private List<TextVariationConfiguration> variations;

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get { return variations.Cast<VariationConfiguration>(); }
		}
		
		/// <summary>
		/// Defines how a text looks like in a variation
		/// </summary>
		[Serializable]
		public class TextVariationConfiguration : VariationConfiguration
		{
			[SerializeField] 
			private string text;

			[SerializeField] 
			private Font font;
			
			[SerializeField] 
			private FontStyle fontStyle;

			[SerializeField] 
			private int fontSize;

			[SerializeField] 
			private TextAnchor alignment;

			[SerializeField] 
			private Color color;

			public void Init(string text, Font font, FontStyle fontStyle, int fontSize, TextAnchor alignment, Color color)
			{
				this.text = text;
				this.font = font;
				this.fontStyle = fontStyle;
				this.fontSize = fontSize;
				this.alignment = alignment;
				this.color = color;
			}

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Text>();
			}
			
			/// <inheritdoc />
			public override void DisableTarget(GameObject gameObject)
			{
				((Text) GetTarget(gameObject)).enabled = false;
			}

			/// <inheritdoc />
			public override void EnableTarget(GameObject gameObject)
			{
				((Text) GetTarget(gameObject)).enabled = true;
			}

			/// <inheritdoc />
			public override void SetupTarget(GameObject gameObject)
			{
				var textComponent = gameObject.GetComponent<Text>();
				textComponent.font = font;
				textComponent.text = text;
				textComponent.fontStyle = fontStyle;
				textComponent.fontSize = fontSize;
				textComponent.alignment = alignment;
				textComponent.color = color;
			}
		}
	}	
}

