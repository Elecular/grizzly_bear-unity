using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Elecular.API;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Elecular.API
{
	public class ElecularMeshRendererTest {

		[Test]
		public void MeshRendererRenders()
		{
			var variation = new ElecularMeshRenderer.MeshRendererVariationConfiguration();
			var materials = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshRenderer>().sharedMaterials;
			
			variation.Init(materials);
			
			var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			meshRenderer.materials = new Material[]{};
			
			Assert.AreEqual(meshRenderer, variation.GetTarget(gameObject));
			Assert.AreEqual(meshRenderer.materials.Length, 0);

			variation.SetupTarget(gameObject);
			Assert.AreEqual(meshRenderer.materials.Length, materials.Length);
		}
	}	
}
