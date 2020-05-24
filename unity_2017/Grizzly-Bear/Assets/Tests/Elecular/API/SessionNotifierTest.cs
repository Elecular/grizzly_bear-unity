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
			string sessionId = null;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.Register(id =>
			{
				sessionId = id;
			});
			yield return new WaitUntil(() => sessionId != null);
		}
		
		[UnityTest]
		[Timeout(15000)]
		public IEnumerator CreateNewSessionWhenUnFocusAndFocusBack()
		{
			var numberOfSessions = 0;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.Register(id =>
			{
				numberOfSessions++;
			});
			
			notifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(4);
			notifier.SetSessionInactiveTimeThreshold(2);
			notifier.OnApplicationFocus(true);
			yield return new WaitUntil(() => numberOfSessions == 2);
			
			notifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(4);
			notifier.OnApplicationFocus(true);
			yield return new WaitUntil(() => numberOfSessions == 3);

		}
		
		[UnityTest]
		[Timeout(15000)]
		public IEnumerator DoesNotCreateNewSessionWhenFocusesBackInTime()
		{
			var numberOfSessions = 0;
			var notifier = new GameObject().AddComponent<SessionNotifier>();
			notifier.Register(id =>
			{
				numberOfSessions++;
			});
			
			notifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(1);
			notifier.SetSessionInactiveTimeThreshold(3);
			notifier.OnApplicationFocus(true);
			yield return new WaitForSeconds(4);
			Assert.AreEqual(numberOfSessions, 1);
		}
	}	
}

