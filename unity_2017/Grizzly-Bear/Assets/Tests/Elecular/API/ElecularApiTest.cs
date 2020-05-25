using System;
using System.Collections;
using Elecular.API;
using Elecular.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Elecular.API
{
	public class ElecularApiTest 
	{
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
		public IEnumerator CreatesNewSessionOnInitialize()
		{
			var sessions = 0;
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (!activity.Equals("New Session")) return;
				sessions++;
			});
			
			ElecularApi.Instance.Initialize();
			yield return new WaitForSeconds(3);
			Assert.NotNull(GameObject.FindObjectOfType<RequestCoroutineManager>());
			Assert.NotNull(GameObject.FindObjectOfType<SessionNotifier>());
			Assert.AreEqual(sessions, 1);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogUserRetention()
		{
			var day1 = false;
			var day7 = false;
			var day30 = false;
			
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromDays(37)).Ticks.ToString());
			PlayerPrefs.Save();
			yield return new WaitForSeconds(1);
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("User Retention Logged: Day 1"))
				{
					day1 = true;
				}
				if (activity.Equals("User Retention Logged: Day 7"))
				{
					day7 = true;
				}
				if (activity.Equals("User Retention Logged: Day 30"))
				{
					day30 = true;
				}
			});
			
			ElecularApi.Instance.Initialize();
			yield return new WaitUntil(() => day1 && day7 && day30);
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
		
		[TearDown]
		public void TearDown()
		{
			GameObject.Destroy(GameObject.FindObjectOfType<RequestCoroutineManager>());
			GameObject.Destroy(GameObject.FindObjectOfType<SessionNotifier>());
		}
	}
}
