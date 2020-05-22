using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tests.Elecular.API
{
	public class ElecularButtonTest 
	{
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanSetVariationOfElecularButton()
		{
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/TestScene");
			var button = GameObject.Find("Forced Control Group Experiment");
			yield return new WaitUntil(() => button.GetComponent<Button>().transition == Selectable.Transition.ColorTint);
			
			button = GameObject.Find("Forced Variation 1 Experiment");
			yield return new WaitUntil(() => button.GetComponent<Button>().transition == Selectable.Transition.SpriteSwap);
		}
	}
}

