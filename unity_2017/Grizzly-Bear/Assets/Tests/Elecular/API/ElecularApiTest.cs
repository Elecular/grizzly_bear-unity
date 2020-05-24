using System.Collections;
using Elecular.API;
using Elecular.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Elecular.API
{
	public class ElecularApiTest {

		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanMakeRequest()
		{
			var response = 0;
			ElecularApi.Instance.GetVariation("Experiment 1", variation =>
			{
				Assert.NotNull(variation.Name);
				response++;
			});
			yield return new WaitUntil(() => response == 1);
			Assert.AreEqual(GameObject.FindObjectsOfType<RequestCoroutineManager>().Length, 1);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CreatesNewSessionOnInstantiate()
		{
			ElecularApi.Instance.Initialize();
			yield return new WaitUntil(() => 
				GameObject.FindObjectOfType<RequestCoroutineManager>() != null &&
				GameObject.FindObjectOfType<SessionNotifier>() != null
			);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CallsOnErrorWithInvalidExperiment()
		{
			LogAssert.ignoreFailingMessages = true;
			var error = false;
			ElecularApi.Instance.GetVariation("Invalid Experiment", variation =>
			{
				Assert.Fail();

			}, () =>
			{
				error = true;
			});
			yield return new WaitUntil(() => error);
			Assert.AreEqual(GameObject.FindObjectsOfType<RequestCoroutineManager>().Length, 1);
		}
	}
}
