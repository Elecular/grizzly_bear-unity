using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Elecular.API;

namespace Tests.Elecular.API
{
	public class SessionNotifierTest 
	{
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CreateNewSessionOnInstantiate()
		{
			var newSession = 0;
			var stopSession = 0;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.RegisterOnNewSession(() =>
			{
				newSession++;
			});
			notifier.RegisterOnSessionStop(() =>
			{
				stopSession++;
			});
			yield return new WaitForSeconds(4);
			Assert.AreEqual(newSession, 1);
			Assert.AreEqual(stopSession, 0);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator StopsSessionOnDestroy()
		{
			var newSession = 0;
			var stopSession = 0;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.RegisterOnNewSession(() =>
			{
				newSession++;
			});
			notifier.RegisterOnSessionStop(() =>
			{
				stopSession++;
			});
			yield return new WaitForSeconds(3);
			GameObject.Destroy(notifier.gameObject);
			yield return new WaitForSeconds(3);
			Assert.AreEqual(newSession, 1);
			Assert.AreEqual(stopSession, 1);
		}
		
		[UnityTest]
		[Timeout(15000)]
		public IEnumerator CreateNewSessionWhenUnFocusAndFocusBack()
		{
			var newSession = 0;
			var stopSession = 0;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.RegisterOnNewSession(() =>
			{
				newSession++;
			});
			notifier.RegisterOnSessionStop(() =>
			{
				stopSession++;
			});
			
			notifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(2);
			notifier.SetSessionInactiveTimeThreshold(1);
			notifier.OnApplicationFocus(true);
			yield return new WaitForSeconds(3);
			Assert.AreEqual(newSession, 2);
			Assert.AreEqual(stopSession, 1);
			
			notifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(2);
			notifier.OnApplicationFocus(true);
			yield return new WaitForSeconds(3);
			Assert.AreEqual(newSession, 3);
			Assert.AreEqual(stopSession, 2);

		}
		
		[UnityTest]
		[Timeout(15000)]
		public IEnumerator DoesNotCreateNewSessionWhenFocusesBackInTime()
		{
			var numberOfSessions = 0;
			var stopSession = 0;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.RegisterOnNewSession(() =>
			{
				numberOfSessions++;
			});
			notifier.RegisterOnSessionStop(() =>
			{
				stopSession++;
			});
			
			notifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(1);
			notifier.SetSessionInactiveTimeThreshold(3);
			notifier.OnApplicationFocus(true);
			yield return new WaitForSeconds(4);
			
			Assert.AreEqual(numberOfSessions, 1);
			Assert.AreEqual(stopSession, 0);
		}
		
		[TearDown]
		public void TearDown()
		{
			ElecularApi.Instance.Reset();
		}
	}	
}

