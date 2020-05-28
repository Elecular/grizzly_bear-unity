using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	/// <summary>
	/// Add this component to a MeshFilter to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	public class ElecularMeshFilter : ChangeableElement<ElecularMeshFilter.MeshFilterVariationConfiguration> 
	{
		[SerializeField]
		[HideInInspector]
		private List<MeshFilterVariationConfiguration> variations;

		/// <inheritdoc />
		protected override void Setup(MeshFilterVariationConfiguration variationConfiguration)
		{
			var meshFilter = GetComponent<MeshFilter>();
			meshFilter.mesh = variationConfiguration.mesh;
		}

		/// <inheritdoc />
		public override IEnumerable<MeshFilterVariationConfiguration> Configurations
		{
			get { return variations; }
		}
		
		/// <summary>
		/// Defines how a text looks like in a variation
		/// </summary>
		[Serializable]
		public class MeshFilterVariationConfiguration : VariationConfiguration
		{
			[SerializeField] 
			public Mesh mesh;

			/// <inheritdoc />
			public override Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<MeshFilter>();
			}
		}
	}	
}

