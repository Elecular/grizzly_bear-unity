using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Elecular.API;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Elecular.API
{
	public class ElecularMeshFilterTest {

		[Test]
		public void MeshFilterRenders()
		{
			var variation = new ElecularMeshFilter.MeshFilterVariationConfiguration();
			var sphereMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshFilter>().sharedMesh;
			
			variation.Init(sphereMesh);
			
			var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			
			Assert.AreEqual(meshFilter, variation.GetTarget(gameObject));
			
			variation.SetupTarget(gameObject);
			Assert.AreEqual(meshFilter.sharedMesh, sphereMesh);
		}
	}	
}
