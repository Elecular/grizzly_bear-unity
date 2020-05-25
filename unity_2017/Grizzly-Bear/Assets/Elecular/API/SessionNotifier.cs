using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elecular.API
{
	/// <summary>
	/// This class is used for getting notifications when a new user session starts
	/// </summary>
	public class SessionNotifier : MonoBehaviour
	{
		[NonSerialized] 
		private float sessionInactiveTimeThreshold;

		/// <summary>
		/// The last time the game lost focus
		/// </summary>
		[NonSerialized]
		private float focusLostTimestamp = float.MaxValue;

		[NonSerialized] 
		private UnityAction onNewSession;
		
		[NonSerialized]
		private UnityAction onSessionStop;

		private void Start()
		{
			StartNewSession();
			sessionInactiveTimeThreshold = ElecularSettings.SESSION_INACTIVE_TIME_THRESHOLD;
			DontDestroyOnLoad(gameObject);
			gameObject.name = "Elecular - Session Notifier";
		}

		private void OnDestroy()
		{
			StopSession();
		}

		public void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus && Time.realtimeSinceStartup >= focusLostTimestamp + sessionInactiveTimeThreshold)
			{
				StopSession();
				StartNewSession();
			}
			else
			{
				focusLostTimestamp = Time.realtimeSinceStartup;	
			}
		}

		private void StartNewSession()
		{
			if (onNewSession != null)
			{
				onNewSession();
			}
		}
		
		private void StopSession()
		{
			if (onSessionStop != null)
			{
				onSessionStop();
			}
		}

		/// <summary>
		/// Callback is triggered whenever there is a new user session created.
		/// </summary>
		/// <param name="onNewSession"></param>
		public void RegisterOnNewSession(UnityAction onNewSession)
		{
			this.onNewSession += onNewSession;
		}
		
		/// <summary>
		/// Callback is triggered whenever the current session stops. 
		/// </summary>
		/// <param name="onSessionStop"></param>
		public void RegisterOnSessionStop(UnityAction onSessionStop)
		{
			this.onSessionStop += onSessionStop;
		}
		
		/// <summary>
		/// If the player is inactive for this amount of time (seconds), Elecular will start a new user session
		/// This is only used for testing.
		/// </summary>
		/// <param name="threshold"></param>
		public void SetSessionInactiveTimeThreshold(float threshold)
		{
			sessionInactiveTimeThreshold = threshold;
		}
	}

}
