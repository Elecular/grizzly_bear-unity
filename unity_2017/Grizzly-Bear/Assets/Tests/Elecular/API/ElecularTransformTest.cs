using UnityEngine;
using NUnit.Framework;
using Elecular.API;

namespace Tests.Elecular.API
{
	public class ElecularTransformTest {

		[Test]
		public void ElecularTransformIsPositioned() 
		{
			var variation = new ElecularTransform.TransformVariationConfiguration();

			variation.Init(new Vector3(5, 54, 53), new Vector3(158, 154, 151), new Vector3(45, 85, 75));
			
			var parent = new GameObject();
			parent.transform.position = new Vector3(10, 10, 10);
			parent.transform.eulerAngles = new Vector3(15, 15, 15);
			parent.transform.localScale = new Vector3(2, 2, 2);
			
			var gameObject = new GameObject();
			gameObject.transform.parent = parent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.zero;
			
			Assert.AreEqual(gameObject.transform, variation.GetTarget(gameObject));
			
			variation.SetupTarget(gameObject);

			Assert.AreEqual(new Vector3(5, 54, 53), gameObject.transform.localPosition);
			Assert.IsTrue(gameObject.transform.localRotation == Quaternion.Euler(new Vector3(158, 154, 151)));
			Assert.AreEqual(new Vector3(45, 85, 75), gameObject.transform.localScale);
		}
	}
	
}
