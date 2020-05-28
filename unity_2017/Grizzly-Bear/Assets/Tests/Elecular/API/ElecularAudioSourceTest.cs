using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Elecular
{
	public class ElecularAudioSourceTest : MonoBehaviour 
	{

		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanSetVariationOfElecularAudioSource()
		{
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/TestScene");
			var audioSource = GameObject.Find("Audio Source (Variation 1)").GetComponent<AudioSource>();
			yield return new WaitUntil(() => audioSource.clip != null);
			Assert.AreApproximatelyEqual(audioSource.volume, 0.5f);
			Assert.AreApproximatelyEqual(audioSource.pitch, 0.43f);
			yield return new WaitUntil(() => audioSource.isPlaying);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator DoesNotPlayOnAwakeWhenFlagIsDisabled()
		{
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/TestScene");
			var audioSource = GameObject.Find("Audio Source (Control Group)").GetComponent<AudioSource>();

			yield return new WaitUntil(() => Mathf.Abs(audioSource.volume - 0.6f) < Mathf.Epsilon);
			Assert.IsTrue(!audioSource.isPlaying);
		}
		
	}	
}
