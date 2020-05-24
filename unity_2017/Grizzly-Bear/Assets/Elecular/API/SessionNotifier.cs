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
		private string sessionId;

		[NonSerialized] 
		private float sessionStartTime;

		[NonSerialized] 
		private float sessionInactiveTimeThreshold;
		
		/// <summary>
		/// The last time the game lost focus
		/// </summary>
		[NonSerialized]
		private float focusLostTimestamp;

		[NonSerialized] 
		private UnityAction<string> onNewSession;

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
			sessionId = Guid.NewGuid().ToString();
			sessionStartTime = Time.realtimeSinceStartup;
			if (onNewSession != null)
			{
				onNewSession(sessionId);
			}
		}
		
		/// <summary>
		/// Current User Session Id. If user is inactive for 15 minutes, a new session id is assigned
		/// </summary>
		public string SessionId
		{
			get { return sessionId; }
		}
		
		/// <summary>
		/// The registered callback is triggered whenever there is a new user session created.
		/// </summary>
		/// <param name="onNewSession"></param>
		public void Register(UnityAction<string> onNewSession)
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
			Debug.LogWarning("Should only be called during testing");
			if (!Application.isEditor) return;
			sessionInactiveTimeThreshold = threshold;
		}
		
		private void OnGUI()
		{
			if (!ElecularSettings.Instance.DebugMode) return;
			DrawWindow();
		}
		
		private void DrawWindow()
		{
			GUILayout.Window(0, new Rect(10, 10, 260, 25), DrawWindowContents, "Elecular Session Info");
		}

		private void DrawWindowContents(int windowId)
		{
			GUILayout.Label(string.Format("Session Id: {0}", sessionId));
			GUILayout.Label(string.Format("Elapsed Time: {0}", Mathf.Round(Time.realtimeSinceStartup - sessionStartTime)));
		}
	}

}
