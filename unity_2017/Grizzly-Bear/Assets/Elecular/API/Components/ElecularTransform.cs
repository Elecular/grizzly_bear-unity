using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	/// <summary>
	/// Attach to transform to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Transform))]
	public class ElecularTransform : ChangeableElement
	{
		[SerializeField]
		[HideInInspector]
		private List<TransformVariationConfiguration> variations;

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get { return variations.Cast<VariationConfiguration>(); }
		}
		
		/// <summary>
		/// Defines Transform for each configuration
		/// </summary>
		[Serializable]
		public class TransformVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			private Vector3 position;
			
			[SerializeField]
			private Vector3 rotation;
			
			[SerializeField]
			private Vector3 scale;

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<Transform>();
			}
			
			/// <inheritdoc />
			public override void DisableTarget(GameObject gameObject)
			{
				
			}

			/// <inheritdoc />
			public override void EnableTarget(GameObject gameObject)
			{
				
			}

			/// <inheritdoc />
			public override void SetupTarget(GameObject gameObject)
			{
				var transform = gameObject.GetComponent<Transform>();
				transform.localPosition = position;
				transform.localEulerAngles = rotation;
				transform.localScale = scale;
			}
		}
	}
	
}