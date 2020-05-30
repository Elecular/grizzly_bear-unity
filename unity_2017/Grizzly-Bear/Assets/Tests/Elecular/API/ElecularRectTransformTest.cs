
using Elecular.API;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Elecular.API
{
	public class ElecularRectTransformTest {

		[Test]
		public void ElecularRectTransformIsPositioned()
		{
			var variation = new ElecularRectTransform.RectTransformVariationConfiguration();
			
			variation.Init(
				new Vector2(0.3f, 0.5f),
				new Vector2(0.4f, 0.7f),
				new Vector2(0.35f, 0.55f),
				new Vector2(0.45f, 0.75f),
				new Vector2(0.6f, 0.6f),
				new Vector3(40, 20, 50),
				new Vector3(10, 60, 30)
			);
			
			var canvas = new GameObject().AddComponent<Canvas>();
			canvas.transform.position = new Vector3(10, 10, 10);
			canvas.transform.eulerAngles = new Vector3(10, 10, 10);
			canvas.transform.localScale = new Vector3(10, 10, 10);
			
			var gameObject =  new GameObject();
			gameObject.transform.SetParent(canvas.transform, false);
			var rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.localPosition = Vector3.zero;
			rectTransform.localEulerAngles = Vector3.zero;
			rectTransform.localScale = Vector3.one;
			Assert.AreEqual(rectTransform, variation.GetTarget(gameObject));
			
			variation.SetupTarget(gameObject);
			
			Assert.IsTrue(Vector2.Distance(new Vector2(0.3f, 0.5f), rectTransform.offsetMin) < 0.03f);
			Assert.IsTrue(Vector2.Distance(new Vector2(0.4f, 0.7f), rectTransform.offsetMax) < 0.03f);
			Assert.IsTrue(Vector2.Distance(new Vector2(0.35f, 0.55f), rectTransform.anchorMin) < 0.03f);
			Assert.IsTrue(Vector2.Distance(new Vector2(0.45f, 0.75f), rectTransform.anchorMax) < 0.03f);
			Assert.IsTrue(Vector2.Distance(new Vector2(0.6f, 0.6f), rectTransform.pivot) < 0.03f);

			Assert.IsTrue(Quaternion.Euler(new Vector3(40, 20, 50)) == rectTransform.localRotation);
			Assert.AreEqual(new Vector3(10, 60, 30), rectTransform.localScale);
		}
	}	
}
