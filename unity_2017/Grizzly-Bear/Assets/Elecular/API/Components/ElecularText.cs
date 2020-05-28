using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	/// <summary>
	/// Add this component to a Text UI to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Text))]
	public class ElecularText : ChangeableElement<ElecularText.TextVariationConfiguration>
	{
		[HideInInspector]
		[SerializeField]
		private List<TextVariationConfiguration> variations;

		/// <inheritdoc />
		protected override void Setup(TextVariationConfiguration variationConfiguration)
		{
			var text = GetComponent<Text>();
			text.font = variationConfiguration.font;
			text.text = variationConfiguration.text;
			text.fontStyle = variationConfiguration.fontStyle;
			text.fontSize = variationConfiguration.fontSize;
			text.alignment = variationConfiguration.alignment;
			text.color = variationConfiguration.color;
		}

		/// <inheritdoc />
		public override IEnumerable<ElecularText.TextVariationConfiguration> Configurations
		{
			get { return variations; }
		}
		
		/// <summary>
		/// Defines how a text looks like in a variation
		/// </summary>
		[Serializable]
		public class TextVariationConfiguration : VariationConfiguration
		{
			[SerializeField] 
			public string text;

			[SerializeField] 
			public Font font;
			
			[SerializeField] 
			public FontStyle fontStyle;

			[SerializeField] 
			public int fontSize;

			[SerializeField] 
			public TextAnchor alignment;

			[SerializeField] 
			public Color color;

			/// <inheritdoc />
			public override Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Text>();
			}
		}
	}	
}

