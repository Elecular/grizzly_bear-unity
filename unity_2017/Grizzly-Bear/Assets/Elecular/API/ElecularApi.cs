using System.Linq;
using Elecular.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Elecular.API
{
	/// <summary>
	/// This API is used for getting assigned variations and logging user activity.
	/// </summary>
	public class ElecularApi
	{
		private string sessionId;

		private float sessionStartTimestamp;
		
		private string environment;

		private UserData userData;

		private bool optOut;

		private UnityAction onNewSession;
		
		//This is triggered whenever there is a new activity logged
		private UnityAction<string> onActivityLog;
		
		private bool canUpdateVariationsWhileRunning;

		private SessionNotifier sessionNotifier;

		private static ElecularApi instance;
		
		private ElecularApi() {}

		/// <summary>
		///  Call this method once in a Start or Awake function to initialize Elecular.
		/// This method creates a new session in Elecular and caches all the experiment variations to avoid flickering effects
		/// </summary>
		public void Initialize()
		{
			if (sessionNotifier != null)
			{
				Debug.LogWarning("Calling Initialize a second time has no effect");
				return;
			}
			userData = new UserData();
			sessionNotifier = new GameObject().AddComponent<SessionNotifier>();
			sessionNotifier.RegisterOnNewSession(CreateSession);
			sessionNotifier.RegisterOnSessionStop(OnSessionStop);
		}
		
		private void CreateSession()
		{
			ElecularSettings.Instance.LoadAllExperiments();
			if (optOut) return;
			var session = new UserActivityApi.Session(
				SystemInfo.deviceUniqueIdentifier,
				userData.GetAllSegments(),
				GetEnvironment()
			);
			UserActivityApi.Instance.LogSession(
				ElecularSettings.Instance.ProjectId, 
				session,
				OnSessionCreated,
				() =>
				{
					Debug.LogError("Error while logging user session. Please check if project id is valid.");
				}
			);
		}

		private void OnSessionCreated(string id)
		{
			sessionId = id;
			sessionStartTimestamp = Time.realtimeSinceStartup;
			if (onActivityLog != null) onActivityLog("New Session");
			if (onNewSession != null) onNewSession();
			foreach (var retention in userData.RetentionSegments)
			{
				var retentionLog = retention;
				LogActivity(string.Format("retention/{0}", retention), 1, () =>
				{
					if(onActivityLog!=null) onActivityLog(string.Format("Logged retention: {0}", retentionLog));
				});
			}
		}

		private void OnSessionStop()
		{
			if (!CanUpdateVariationsWhileRunning) return;
			ExperimentsApi.Instance.ClearCache();
		}
		
		/// <summary>
		/// Used for storing user demographics and custom segments
		/// </summary>
		public UserData UserData
		{
			get
			{
				return userData;
			}
		}
		
		/// <summary>
		/// Sets the environment.
		/// By default it is dev in editor and prod in your shipped game
		/// </summary>
		/// <param name="env"></param>
		public void SetEnvironment(Environment env)
		{
			environment = env.GetString();
		}

		/// <summary>
		/// Gets the variation that is assigned to the user
		/// </summary>
		/// <param name="experimentName">Name of the experiment</param>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		/// <returns>The variation assigned to this user</returns>
		public void GetVariation(
			string experimentName, 
			UnityAction<Variation> onResponse, 
			UnityAction onError=null
		)
		{
			ExperimentsApi.Instance.GetVariation(
				SystemInfo.deviceUniqueIdentifier, 
				ElecularSettings.Instance.ProjectId, 
				experimentName, 
				onResponse, 
				onError
			);
		}
		
		/// <summary>
		/// Logs an ad impression.
		/// An ad impression is when an ad is displayed to the user
		/// </summary>
		/// <param name="placementId">Placement id of the ad</param>
		public void LogAdImpression(string placementId)
		{
			LogActivity("ads/impression", 1, () =>
			{
				LogActivity(string.Format("ads/impression/{0}", placementId), 1, () =>
				{
					if (onActivityLog != null) onActivityLog(string.Format("Ad Impression: {0}", placementId));
				});
			});
		}
		
		/// <summary>
		/// Logs an ad click.
		/// An ad click is when the user clicks an ad.
		/// </summary>
		/// <param name="placementId">Placement id of the ad</param>
		public void LogAdClick(string placementId)
		{
			LogActivity("ads/click", 1, () =>
			{
				LogActivity(string.Format("ads/click/{0}", placementId), 1, () =>
				{
					if (onActivityLog != null) onActivityLog(string.Format("Ad Click: {0}", placementId));
				});
			});
		}

		/// <summary>
		/// Logs a transaction.
		/// A transaction is when a player purchases an in-game item.
		/// </summary>
		/// <param name="productId">Product Id of the in-game item. Ex: 100 Gold</param>
		/// <param name="price">Price of the product</param>
		public void LogTransaction(string productId, decimal price)
		{
			LogActivity("transactions/complete", price, () =>
			{
				LogActivity(string.Format("transactions/complete/{0}", productId), price, () =>
				{
					if (onActivityLog != null) onActivityLog(string.Format("Transaction: {0} ({1}$)", productId, price.ToString()));
				});
			});
		}
		
		/// <summary>
		/// Logs a custom event
		/// Custom even could be a level up, achievement completion, button click etc.
		/// </summary>
		/// <param name="eventId">Name of the event. Example: Level Complete</param>
		/// <param name="amount">An amount associated with the event. Example: </param>
		public void LogCustomEvent(string eventId, decimal amount)
		{
			if (eventId.Contains("/"))
			{
				Debug.LogError("Custom event ids cannot contain front slashes (/)");
				return;
			}
			LogActivity(string.Format("custom/{0}", eventId), amount, () =>
			{
				if (onActivityLog != null) onActivityLog(string.Format("Custom Event: {0} ({1})", eventId, amount));
			});
		}

		/// <summary>
		/// Gets the variation that corresponds to the given variation name
		/// WARNING: This function is expensive and is only meant to be used in editor
		/// </summary>
		/// <param name="experimentName">Name of the experiment</param>
		/// <param name="variationName">Name of the variation</param>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		/// <returns>The variation assigned to this user</returns>
		public void GetVariation(
			string experimentName,
			string variationName,
			UnityAction<Variation> onResponse, 
			UnityAction onError=null
		)
		{
			GetAllVariations(experimentName, variations =>
			{
				onResponse(variations.FirstOrDefault(variation => variation.Name.Equals(variationName)));
			}, onError);
		}
		
		/// <summary>
		/// Gets all the variations under given experiment
		/// WARNING: This function is expensive and is only meant to be used in editor
		/// </summary>
		/// <param name="experimentName">Name of the experiment</param>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		public void GetAllVariations(
			string experimentName, 
			UnityAction<Variation[]> onResponse, 
			UnityAction onError=null
		)
		{
			ExperimentsApi.Instance.GetAllVariations(
				ElecularSettings.Instance.ProjectId, 
				experimentName, 
				onResponse, 
				onError
			);
		}
		
		/// <summary>
		/// Callback that is triggered whenever a new session is created.
		/// 
		/// Note: There are situations when you register your callback from a Monobehaviour.
		/// It is recommended to unregister your callback when the Monobehaviour is destroyed. 
		/// </summary>
		/// <param name="onNewSession"></param>
		public void RegisterOnNewSessionEvent(UnityAction onNewSession)
		{
			this.onNewSession += onNewSession;
		}
		
		/// <summary>
		/// Unregisters the callback from getting new session events.
		/// There are situations when you registered your callback from a Monobehaviour.
		/// It is recommended to unregister your callback when the Monobehaviour is destroyed. 
		/// </summary>
		/// <param name="onNewSession"></param>
		public void UnRegisterFromNewSessionEvent(UnityAction onNewSession)
		{
			this.onNewSession -= onNewSession;
		}
		
		/// <summary>
		/// Callback is triggered whenever a new user activity is logged.
		/// This is mainly used by the Debug Window
		/// </summary>
		/// <param name="onActivityLog"></param>
		public void RegisterOnActivityLog(UnityAction<string> onActivityLog)
		{
			this.onActivityLog += onActivityLog;
		}
		
		/// <summary>
		/// Unregisters from activity log
		/// </summary>
		/// <param name="onActivityLog"></param>
		public void UnRegisterFromActivityLog(UnityAction<string> onActivityLog)
		{
			this.onActivityLog -= onActivityLog;
		}
		
		/// <summary>
		/// If the API is tracking events
		/// This is usually false if ElecularApi is not initialized or OptOut is set to true.
		/// </summary>
		/// <returns></returns>
		public bool IsTracking
		{
			get
			{
				return sessionNotifier != null && sessionId != null && !OptOut;
			}
		}
		
		/// <summary>
		/// Gets current session id.
		/// This will be null until Elecular has initialized
		/// </summary>
		public string SessionId
		{
			get { return sessionId; }
		}
		
		/// <summary>
		/// Amount of time spend in the current session
		/// </summary>
		public float ElapsedSessionTime
		{
			get { return Time.realtimeSinceStartup - sessionStartTimestamp; }
		}
		
		/// <summary>
		/// If set to true, Elecular will pull the most updated variations whenever a new session is created
		/// Variations are usually updated when an experiment starts or stops.
		/// When an experiment starts, all users are redirected from control group to their designated variations.
		/// When the experiment stops, all  users are redirected from their variations back towards the control group.
		/// If the user never quits his/her game, the user will remain in their old variation and will not be redirected to the correct variation.
		/// Hence, if you want to avoid this situation, set this flag to true. This is not critical, but it does provide more accurate results.
		///
		/// You can listen to ElecularApi.Instance.RegisterOnNewSessionEvent to get notified whenever there is a new session and re-fetch the experiment variations/settings if necessary. 
		/// ElecularComponents automatically update themselves if this field is set to true. 
		/// </summary>
		public bool CanUpdateVariationsWhileRunning
		{
			get { return canUpdateVariationsWhileRunning; }
			set { canUpdateVariationsWhileRunning = value; }
		}
		
		/// <summary>
		/// Stops tracking any events. 
		/// </summary>
		public bool OptOut
		{
			get { return optOut; }
			set
			{
				//If we were opted out but now we are not opted out, we need to create a new session
				if (sessionNotifier != null && optOut && !value)
				{
					optOut = false;
					CreateSession();
				}
				optOut = value;
			}
		}

		private string GetEnvironment()
		{
			if (environment != null)
			{
				return environment;
			}
			return (Application.isEditor ? Environment.Dev : Environment.Prod).GetString();
		}
		
		/// <summary>
		/// Logs a user action.
		/// Use this instead of directly using UserActivityApi.LogActivity
		/// </summary>
		/// <param name="userAction"></param>
		/// <param name="amount"></param>
		/// <param name="onResponse"></param>
		private void LogActivity(string userAction, decimal amount, UnityAction onResponse=null)
		{
			if (userAction == null || userAction.Equals(""))
			{
				Debug.LogError("User action cannot be null or empty");
				return;
			}

			if (!IsTracking)
			{
				if (OptOut)
				{
					if (onActivityLog != null) onActivityLog("Player has opted out of tracking");
					return;
				}
				Debug.LogError("Elecular API is not initialized");
				return;
			}
			
			var activity = new UserActivityApi.Activity(sessionId, userAction, amount.ToString());
			UserActivityApi.Instance.LogActivity(
				ElecularSettings.Instance.ProjectId,
				activity,
				() =>
				{
					if (onResponse != null) onResponse();
				}, 
				() =>
				{
					Debug.LogError("Error while logging user activity: " + userAction);
				}
			);
		}

		/// <summary>
		/// Gets the Singleton instance of Elecular API
		/// </summary>
		public static ElecularApi Instance
		{
			get { return instance ?? (instance = new ElecularApi()); }
		}
	
		/// <summary>
		/// Removes current state and destroys all spawned objects. Once Reset is called, Elecular will need to be reinitialized again
		/// Only use for testing
		/// </summary>
		public void Reset()
		{
			GameObject.Destroy(GameObject.FindObjectOfType<RequestCoroutineManager>());
			GameObject.Destroy(GameObject.FindObjectOfType<SessionNotifier>());
			ExperimentsApi.Instance.ClearCache();
			instance = new ElecularApi();
		}
	}	
}

