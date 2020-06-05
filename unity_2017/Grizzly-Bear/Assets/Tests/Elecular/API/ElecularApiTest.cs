using System;
using System.Collections;
using Elecular.API;
using Elecular.Core;
using NUnit.Framework;
using Tests.Elecular.Core;
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
			Request.SetMockRequest(null);
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
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var sessions = 0;
			var onInitializedCalled = 0;
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (!activity.Equals("New Session")) return;
				sessions++;
			});
			ElecularApi.Instance.InitializeWithTracking(() =>
			{
				Assert.AreEqual(sessions, 1);
				onInitializedCalled++;
			});
			
			yield return new WaitForSeconds(3);
			
			Assert.NotNull(GameObject.FindObjectOfType<RequestCoroutineManager>());
			Assert.NotNull(GameObject.FindObjectOfType<SessionNotifier>());
			Assert.AreEqual(sessions, 1);
			Assert.AreEqual(onInitializedCalled, 1);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator DoesNotCreateNewSessionWhenOptedOut()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var sessions = 0;
			ElecularApi.Instance.OptOut = true;
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				sessions++;
			});
			
			ElecularApi.Instance.InitializeWithTracking();
			yield return new WaitForSeconds(3);
			Assert.NotNull(GameObject.FindObjectOfType<RequestCoroutineManager>());
			Assert.NotNull(GameObject.FindObjectOfType<SessionNotifier>());
			Assert.AreEqual(sessions, 0);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CreateNewSessionWhenOptsBackIn()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var sessions = 0;
			var initialized = 0;
			ElecularApi.Instance.OptOut = true;
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (!activity.Contains("New Session")) return;
				sessions++;
			});
			
			ElecularApi.Instance.InitializeWithTracking(() =>
			{
				Assert.AreEqual(sessions, 0);
				initialized++;
			});
			yield return new WaitForSeconds(3);
			ElecularApi.Instance.OptOut = false;
			yield return new WaitForSeconds(3);
			Assert.AreEqual(sessions, 1);
			Assert.AreEqual(initialized, 1);
			Assert.True(ElecularApi.Instance.IsTracking);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogUserRetention()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var day1 = false;
			var day7 = false;
			var day30 = false;
			
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromDays(37)).Ticks.ToString());
			PlayerPrefs.Save();
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Logged retention: Day 1"))
				{
					day1 = true;
				}
				if (activity.Equals("Logged retention: Day 7"))
				{
					day7 = true;
				}
				if (activity.Equals("Logged retention: Day 30"))
				{
					day30 = true;
				}
			});
			
			ElecularApi.Instance.InitializeWithTracking();
			yield return new WaitUntil(() => day1 && day7 && day30);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogAdImpression()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var logged = false;
			
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Ad Impression: video"))
				{
					logged = true;	
				}
			});
			
			ElecularApi.Instance.InitializeWithTracking();
			yield return new WaitUntil(() => ElecularApi.Instance.IsTracking);
			ElecularApi.Instance.LogAdImpression("video");
			yield return new WaitUntil(() => logged);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CannotLogAdImpressionWhenOptedOut()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var logged = false;
			var initialized = 0;
			
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Ad Impression: video"))
				{
					logged = true;	
				}
			});

			ElecularApi.Instance.OptOut = true;
			ElecularApi.Instance.InitializeWithTracking(() =>
			{
				Assert.False(logged);
				initialized++;
			});
			yield return new WaitForSeconds(3);
			ElecularApi.Instance.LogAdImpression("video");
			yield return new WaitForSeconds(3);
			Assert.False(logged);
			Assert.AreEqual(initialized, 1);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogAdClick()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var logged = false;
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Ad Click: video"))
				{
					logged = true;	
				}
			});
			
			ElecularApi.Instance.InitializeWithTracking();
			yield return new WaitUntil(() => ElecularApi.Instance.IsTracking);
			ElecularApi.Instance.LogAdClick("video");
			yield return new WaitUntil(() => logged);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanLogTransaction()
		{
			Request.SetMockRequest(null);
			LogAssert.ignoreFailingMessages = true;
			
			var logged = false;
			ElecularApi.Instance.RegisterOnActivityLog((activity) =>
			{
				if (activity.Equals("Transaction: item (0.99$)"))
				{
					logged = true;	
				}
			});
			
			ElecularApi.Instance.InitializeWithTracking();
			yield return new WaitUntil(() => ElecularApi.Instance.IsTracking);
			ElecularApi.Instance.LogTransaction("item", 0.99m);
			yield return new WaitUntil(() => logged);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CallsOnErrorWithInvalidExperiment()
		{
			Request.SetMockRequest(null);
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
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator InitializesEvenWithError()
		{
			Request.SetMockRequest(new MockRequest(null));
			
			var initialized = 0;
			ElecularApi.Instance.InitializeWithTracking(() =>
			{
				initialized++;
			});
			yield return new WaitUntil(() => initialized == 1);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CannotLogWithError()
		{
			Request.SetMockRequest(new MockRequest(null));
			
			var initialized = 0;
			ElecularApi.Instance.InitializeWithTracking(() =>
			{
				initialized++;
			});
			yield return new WaitUntil(() => initialized == 1);
			
			ElecularApi.Instance.LogAdClick("ad placement");
			yield return new WaitForSeconds(2);
		}
		
		[TearDown]
		public void TearDown()
		{
			ElecularApi.Instance.Reset();
		}
	}
}
