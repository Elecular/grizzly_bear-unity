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
		private float focusLostTimestamp;

		[NonSerialized] 
		private UnityAction onNewSession;

		private void Start()
		{
			sessionInactiveTimeThreshold = ElecularSettings.SESSION_INACTIVE_TIME_THRESHOLD;
			StartNewSession(); 
		}

		public void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus && Time.realtimeSinceStartup >= focusLostTimestamp + sessionInactiveTimeThreshold)
			{
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

		/// <summary>
		/// The registered callback is triggered whenever there is a new user session created.
		/// </summary>
		/// <param name="onNewSession"></param>
		public void Register(UnityAction onNewSession)
		{
			this.onNewSession += onNewSession;
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
