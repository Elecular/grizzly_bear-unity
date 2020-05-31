using System.Collections;
using Elecular.API;
using Elecular.Core;
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
			Request.SetMockRequest(null);
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/Audio TestScene");
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
			Request.SetMockRequest(null);
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/Audio TestScene");
			var audioSource = GameObject.Find("Audio Source (Control Group)").GetComponent<AudioSource>();

			yield return new WaitUntil(() => Mathf.Abs(audioSource.volume - 0.6f) < Mathf.Epsilon);
			Assert.IsTrue(!audioSource.isPlaying);
		}

		[Test]
		public void TestAudioSourceVariation()
		{
			var variation = new ElecularAudioSource.AudioVariationConfiguration();
			var audioClip = new AudioClip();
			variation.Init(audioClip, 0.5f, 0.4f, false);

			var gameObject = new GameObject();
			var audioSource = gameObject.AddComponent<AudioSource>();
			
			Assert.AreEqual(audioSource, variation.GetTarget(gameObject)); 
			
			variation.SetupTarget(gameObject);
			Assert.AreEqual(audioSource.clip, audioClip);
			Assert.AreApproximatelyEqual(audioSource.volume, 0.5f);
			Assert.AreApproximatelyEqual(audioSource.pitch, 0.4f);
			Assert.AreEqual(audioSource.playOnAwake, false);
		}

	}	
}
