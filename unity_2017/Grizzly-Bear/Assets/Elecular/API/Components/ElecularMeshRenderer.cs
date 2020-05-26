using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elecular.API
{
    /// <summary>
    /// Add this component to a MeshRenderer to add variations
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    public class ElecularMeshRenderer : ChangeableElement<ElecularMeshRenderer.MeshRendererVariationConfiguration> 
    {
        [SerializeField]
        [HideInInspector]
        private List<MeshRendererVariationConfiguration> variations;

        /// <inheritdoc />
        protected override void Setup(MeshRendererVariationConfiguration variationConfiguration)
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.materials = variationConfiguration.materials;
        }

        /// <inheritdoc />
        public override IEnumerable<MeshRendererVariationConfiguration> Configurations
        {
            get { return variations; }
        }
		
        /// <summary>
        /// Defines how a text looks like in a variation
        /// </summary>
        [Serializable]
        public class MeshRendererVariationConfiguration : VariationConfiguration
        {
            [SerializeField] 
            public Material[] materials;
        }
    }	
}