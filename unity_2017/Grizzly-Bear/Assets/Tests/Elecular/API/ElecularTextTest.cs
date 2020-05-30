using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Elecular.API;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Elecular.API
{
	public class ElecularTextTest {

		[Test]
		public void TextRenders()
		{
			var variation = new ElecularText.TextVariationConfiguration();
			var font = new Font();
			variation.Init("hello world", font, FontStyle.BoldAndItalic, 40, TextAnchor.UpperRight, Color.cyan);
			
			var gameObject = new GameObject();
			var text = gameObject.AddComponent<Text>();

			Assert.AreEqual(text, variation.GetTarget(gameObject));
			
			variation.DisableTarget(gameObject);
			Assert.IsFalse(text.enabled);
			
			variation.SetupTarget(gameObject);
			Assert.AreEqual("hello world", text.text);
			Assert.AreEqual(font, text.font);
			Assert.AreEqual(FontStyle.BoldAndItalic, text.fontStyle);
			Assert.AreEqual(40, text.fontSize);
			Assert.AreEqual(TextAnchor.UpperRight, text.alignment);
			Assert.AreEqual(Color.cyan, text.color);
			
			variation.EnableTarget(gameObject);
			Assert.IsTrue(text.enabled);
		}
	}	
}
