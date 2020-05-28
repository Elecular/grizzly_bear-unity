using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	/// <summary>
	/// Attach to transform to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Transform))]
	public class ElecularTransform : ChangeableElement<ElecularTransform.TransformVariationConfiguration>
	{
		[SerializeField]
		[HideInInspector]
		private List<TransformVariationConfiguration> variations;

		/// <inheritdoc />
		protected override void Setup(TransformVariationConfiguration variationConfiguration)
		{
			var transform = GetComponent<Transform>();
			transform.position = variationConfiguration.position;
			transform.eulerAngles = variationConfiguration.rotation;
			transform.localScale = variationConfiguration.scale;
		}

		/// <inheritdoc />
		public override IEnumerable<TransformVariationConfiguration> Configurations
		{
			get { return variations; }
		}
		
		/// <summary>
		/// Defines Transform for each configuration
		/// </summary>
		[Serializable]
		public class TransformVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			public Vector3 position;
			
			[SerializeField]
			public Vector3 rotation;
			
			[SerializeField]
			public Vector3 scale;

			/// <inheritdoc />
			public override Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Transform>();
			}
		}
	}
	
}