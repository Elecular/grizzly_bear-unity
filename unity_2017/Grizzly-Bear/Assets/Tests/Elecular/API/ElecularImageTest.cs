using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Elecular.API;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Elecular.API
{
	public class ElecularImageTest {

		[Test]
		public void ElecularImageRenders()
		{
			var variation = new ElecularImage.ImageVariationConfiguration();
			var sourceImage = new Sprite();
			var material = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshRenderer>().material;
			
			variation.Init(sourceImage, Color.blue, material);
			
			var gameObject = new GameObject();
			var image = gameObject.AddComponent<Image>();
			
			Assert.AreEqual(image, variation.GetTarget(gameObject));
			
			variation.DisableTarget(gameObject);
			Assert.IsFalse(image.enabled);
			
			variation.SetupTarget(gameObject);
			Assert.AreEqual(image.sprite, sourceImage);
			Assert.AreEqual(image.color, Color.blue);
			Assert.AreEqual(image.material, material);
			
			variation.EnableTarget(gameObject);
			Assert.IsTrue(image.enabled);
		}
	}	
}
