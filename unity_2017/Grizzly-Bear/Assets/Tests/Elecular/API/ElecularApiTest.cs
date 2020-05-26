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
			Assert.NotNull(GameObject.FindObjectOfType<RequestCoroutineManager>());
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
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Custom Event: Day 1 Retention (1)"))
				{
					day1 = true;
				}
				if (activity.Equals("Custom Event: Day 7 Retention (1)"))
				{
					day7 = true;
				}
				if (activity.Equals("Custom Event: Day 30 Retention (1)"))
				{
					day30 = true;
				}
			});
			
			ElecularApi.Instance.Initialize();
			yield return new WaitUntil(() => day1 && day7 && day30);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogAdImpression()
		{
			var logged = false;
			
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Ad Impression: video"))
				{
					logged = true;	
				}
			});
			
			ElecularApi.Instance.Initialize();
			yield return new WaitUntil(() => ElecularApi.Instance.IsInitialized);
			ElecularApi.Instance.LogAdImpression("video");
			yield return new WaitUntil(() => logged);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogAdClick()
		{
			var logged = false;
			
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Ad Click: video"))
				{
					logged = true;	
				}
			});
			
			ElecularApi.Instance.Initialize();
			yield return new WaitUntil(() => ElecularApi.Instance.IsInitialized);
			ElecularApi.Instance.LogAdClick("video");
			yield return new WaitUntil(() => logged);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogTransaction()
		{
			var logged = false;
			
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Transaction: item (0.99$)"))
				{
					logged = true;	
				}
			});
			
			ElecularApi.Instance.Initialize();
			yield return new WaitUntil(() => ElecularApi.Instance.IsInitialized);
			ElecularApi.Instance.LogTransaction("item", 0.99f);
			yield return new WaitUntil(() => logged);
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
			ElecularApi.Instance.Reset();
		}
	}
}
