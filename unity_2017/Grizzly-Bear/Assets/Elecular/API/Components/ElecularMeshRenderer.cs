using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elecular.API
{
    /// <summary>
    /// Add this component to a MeshRenderer to add variations
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class ElecularMeshRenderer : ChangeableElement
    {
        [SerializeField]
        [HideInInspector]
        private List<MeshRendererVariationConfiguration> variations;

        /// <inheritdoc />
        public override IEnumerable<VariationConfiguration> Configurations
        {
            get { return variations.Cast<VariationConfiguration>(); }
        }
		
        /// <summary>
        /// Defines how a text looks like in a variation
        /// </summary>
        [Serializable]
        public class MeshRendererVariationConfiguration : VariationConfiguration
        {
            [SerializeField] 
            private Material[] materials;

            /// <inheritdoc />
            public override Component GetTarget(GameObject gameObject)
            {
                return gameObject.GetComponent<Renderer>();
            }
            
            /// <inheritdoc />
            public override void DisableTarget(GameObject gameObject)
            {
                ((Renderer) GetTarget(gameObject)).enabled = false;
            }

            /// <inheritdoc />
            public override void EnableTarget(GameObject gameObject)
            {
                ((Renderer) GetTarget(gameObject)).enabled = true;
            }

            public override void SetupTarget(GameObject gameObject)
            {
                var meshRenderer = gameObject.GetComponent<Renderer>();
                meshRenderer.materials = materials;
            }
        }
    }	
}