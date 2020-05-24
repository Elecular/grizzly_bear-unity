using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Attach this component to an Image to add variations
	/// </summary>
	[RequireComponent(typeof(Image))]
	[DisallowMultipleComponent]
	public class ElecularImage : ChangeableElement<ElecularImage.ImageVariationConfiguration> 
	{
		/// <summary>
		/// This is a list of variation configurations that define how the image will look like in each variation
		/// </summary>
		[SerializeField] 
		[HideInInspector]
		private List<ImageVariationConfiguration> variations = new List<ImageVariationConfiguration>();

		protected override void Setup(ImageVariationConfiguration variationConfiguration)
		{
			var image = GetComponent<Image>();
			image.sprite = variationConfiguration.SourceImage;
			image.material = variationConfiguration.Material;
			image.color = variationConfiguration.Color;
		}

		public override IEnumerable<ImageVariationConfiguration> Configurations
		{
			get { return variations; }
		}
		
		/// <summary>
		/// This class defines how a image is going to look like in a certain variation
		/// </summary>
		[Serializable]
		public class ImageVariationConfiguration : VariationConfiguration
		{
			[SerializeField] private Sprite sourceImage;

			[SerializeField] private Color color;

			[SerializeField] private Material material;
			
			public Sprite SourceImage
			{
				get { return sourceImage; }
			}

			public Color Color
			{
				get { return color; }
			}

			public Material Material
			{
				get { return material; }
			}
		}
	}
}

