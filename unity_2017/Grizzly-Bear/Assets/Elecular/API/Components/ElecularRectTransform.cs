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
			
			rectTransform.anchorMin = variationConfiguration.anchorMin;
			rectTransform.anchorMax = variationConfiguration.anchorMax;
			
			rectTransform.pivot = variationConfiguration.pivot;
			
			rectTransform.eulerAngles = variationConfiguration.rotation;
			rectTransform.localScale = variationConfiguration.scale;
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
			
			[SerializeField] 
			public Vector2 anchorMin;

			[SerializeField] 
			public Vector2 anchorMax;
			
			[SerializeField] 
			public Vector2 pivot;

			[SerializeField] 
			public Vector3 rotation;

			[SerializeField] 
			public Vector3 scale;

			/// <inheritdoc />
			public override Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<RectTransform>();
			}
		}
	}
}
