using System;
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
		private string sessionId = null;

		private float sessionStartTimestamp;
		
		private string environment;
		
		private UserData userData = new UserData();

		private UnityAction<string> onNewSession;
		
		private SessionNotifier sessionNotifier;

		private static ElecularApi instance;
		
		private ElecularApi() {}

		/// <summary>
		/// Call this method to initialize Elecular.
		/// Call this once in a Start or Awake function.
		/// </summary>
		public void Initialize()
		{
			if (sessionNotifier != null)
			{
				Debug.LogWarning("Do not call Initialize twice");
				return;
			}
			sessionNotifier = new GameObject("Elecular - Session Notifier").AddComponent<SessionNotifier>();
			GameObject.DontDestroyOnLoad(sessionNotifier.gameObject);
			sessionNotifier.Register(CreateSession);
		}
		
		private void CreateSession()
		{
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
			if (sessionId != null)
			{
				LogCustomEvent("Play time", Time.realtimeSinceStartup - sessionStartTimestamp);
				ExperimentsApi.Instance.ClearCache();
			}
			sessionId = id;
			sessionStartTimestamp = Time.realtimeSinceStartup;
			if (onNewSession != null) onNewSession(sessionId);
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
				LogActivity(string.Format("ads/impression/{0}", placementId), 1);
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
				LogActivity(string.Format("ads/click/{0}", placementId), 1);
			});
		}

		/// <summary>
		/// Logs a transaction.
		/// A transaction is when a player purchases an in-game item.
		/// </summary>
		/// <param name="productId">Product Id of the in-game item. Ex: 100 Gold</param>
		/// <param name="price">Price of the product</param>
		public void LogTransaction(string productId, float price)
		{
			LogActivity("transactions/complete", price, () =>
			{
				LogActivity(string.Format("transactions/complete/{0}", productId), price);
			});
		}
		
		/// <summary>
		/// Logs a custom event
		/// Custom even could be a level up, achievement completion, button click etc.
		/// </summary>
		/// <param name="eventId">Name of the event. Example: Level Complete</param>
		/// <param name="amount">An amount associated with the event. Example: </param>
		public void LogCustomEvent(string eventId, float amount)
		{
			if (eventId.Contains("/"))
			{
				Debug.LogError("Custom event ids cannot contain front slashes (/)");
				return;
			}
			LogActivity(string.Format("custom/{0}", eventId), amount);
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
		/// Whenever a new session is created, Elecular gets the most updated variations. Hence, it is recommended to listen to this event and refresh your components.
		/// It is not necessary but it provides better results.
		/// 
		/// All ElecularComponents already listen to this event and update themselves whenever there is a new session created.
		///
		/// Variations are usually updated when an experiment starts or stops.
		/// 1. When an experiment starts, all users are redirected from control group to their designated variations.
		/// 2. When the experiment stops, all  users are redirected towards the control group.
		/// If the user never quits his/her game, the user will remain in their old variation and will not be redirected to the correct variation. 
		/// </summary>
		/// <param name="onNewSession"></param>
		public void RegisterOnNewSession(UnityAction<string> onNewSession)
		{
			this.onNewSession += onNewSession;
		}
		
		/// <summary>
		/// Is the API initialized
		/// </summary>
		/// <returns></returns>
		public bool IsInitialized
		{
			get
			{
				return sessionNotifier != null && sessionId != null;
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

		private void LogActivity(string userAction, float amount, UnityAction onResponse=null)
		{
			if (userAction == null || userAction.Equals(""))
			{
				Debug.LogError("User action cannot be null or empty");
				return;
			}

			if (!IsInitialized)
			{
				Debug.LogError("Elecular API is not initialized yet");
				return;
			}
			
			var activity = new UserActivityApi.Activity(sessionId, userAction, amount);
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
	}	
}

