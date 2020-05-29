using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class ElecularRectTransform : ChangeableElement
	{
		[SerializeField]
		[HideInInspector]
		private List<RectTransformVariationConfiguration> variations;

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get { return variations.Cast<VariationConfiguration>(); }
		}
		
		[Serializable]
		public class RectTransformVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			private Vector2 offsetMin;
			
			[SerializeField]
			private Vector2 offsetMax;
			
			[SerializeField] 
			private Vector2 anchorMin;

			[SerializeField] 
			private Vector2 anchorMax;
			
			[SerializeField] 
			private Vector2 pivot;

			[SerializeField] 
			private Vector3 rotation;

			[SerializeField] 
			private Vector3 scale;

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<RectTransform>();
			}
			
			/// <inheritdoc />
			public override void DisableTarget(GameObject gameObject)
			{
				
			}

			/// <inheritdoc />
			public override void EnableTarget(GameObject gameObject)
			{
				
			}

			public override void SetupTarget(GameObject gameObject)
			{
				var rectTransform = gameObject.GetComponent<RectTransform>();
			
				rectTransform.offsetMin = offsetMin;
				rectTransform.offsetMax = offsetMax;
			
				rectTransform.anchorMin = anchorMin;
				rectTransform.anchorMax = anchorMax;
			
				rectTransform.pivot = pivot;
			
				rectTransform.eulerAngles = rotation;
				rectTransform.localScale = scale;
			}
		}
	}
}
