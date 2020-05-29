using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Attach this component to an Image to add variations
	/// </summary>
	[RequireComponent(typeof(Image))]
	[DisallowMultipleComponent]
	public class ElecularImage : ChangeableElement 
	{
		/// <summary>
		/// This is a list of variation configurations that define how the image will look like in each variation
		/// </summary>
		[SerializeField] 
		[HideInInspector]
		private List<ImageVariationConfiguration> variations = new List<ImageVariationConfiguration>();

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get { return variations.Cast<VariationConfiguration>(); }
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

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Image>();
			}
			
			/// <inheritdoc />
			public override void DisableTarget(GameObject gameObject)
			{
				((Image) GetTarget(gameObject)).enabled = false;
			}

			/// <inheritdoc />
			public override void EnableTarget(GameObject gameObject)
			{
				((Image) GetTarget(gameObject)).enabled = true;
			}

			/// <inheritdoc />
			public override void SetupTarget(GameObject gameObject)
			{
				var image = gameObject.GetComponent<Image>();
				image.sprite = sourceImage;
				image.material = material;
				image.color = color;
			}
		}
	}
}

