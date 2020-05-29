using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// Add this component to a MeshFilter to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	public class ElecularMeshFilter : ChangeableElement 
	{
		[SerializeField]
		[HideInInspector]
		private List<MeshFilterVariationConfiguration> variations;

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get { return variations.Cast<VariationConfiguration>(); }
		}
		
		/// <summary>
		/// Defines how a text looks like in a variation
		/// </summary>
		[Serializable]
		public class MeshFilterVariationConfiguration : VariationConfiguration
		{
			[SerializeField] 
			private Mesh mesh;

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<MeshFilter>();
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
				var meshFilter = gameObject.GetComponent<MeshFilter>();
				meshFilter.mesh = mesh;
			}
		}
	}	
}

