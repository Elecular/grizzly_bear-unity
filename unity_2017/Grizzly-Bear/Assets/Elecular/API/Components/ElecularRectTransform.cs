using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class ElecularRectTransform : ChangeableElement<ElecularRectTransform.RectTransformVariationConfiguration>
	{
		[SerializeField]
		[HideInInspector]
		private List<RectTransformVariationConfiguration> variations;

		/// <inheritdoc />
		protected override void Setup(RectTransformVariationConfiguration variationConfiguration)
		{
			var rectTransform = GetComponent<RectTransform>();
			rectTransform.offsetMin = variationConfiguration.offsetMin;
			rectTransform.offsetMax = variationConfiguration.offsetMax;
		}

		/// <inheritdoc />
		public override IEnumerable<RectTransformVariationConfiguration> Configurations
		{
			get { return variations; }
		}
		
		[Serializable]
		public class RectTransformVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			public Vector2 offsetMin;
			
			[SerializeField]
			public Vector2 offsetMax;

			/// <inheritdoc />
			public override Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<RectTransform>();
			}
		}
	}
}
