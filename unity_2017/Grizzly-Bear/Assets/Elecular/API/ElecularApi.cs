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
		private string sessionId;
		
		private string environment;
		
		private UserData userData = new UserData();

		private SessionNotifier sessionNotifier;

		private static ElecularApi instance;
		
		private ElecularApi() {}

		/// <summary>
		/// Call this method to initialize Elecular.
		/// Call this once in a Start function. Do NOT call this during Awake. 
		/// </summary>
		public void Initialize()
		{
			sessionNotifier = new GameObject("Session Notifier").AddComponent<SessionNotifier>();
			GameObject.DontDestroyOnLoad(sessionNotifier.gameObject);
			sessionNotifier.Register(OnNewSession);
		}
		
		private void OnNewSession()
		{
			var session = new UserActivityApi.Session(
				SystemInfo.deviceUniqueIdentifier,
				userData.GetAllSegments(),
				GetEnvironment()
			);
			UserActivityApi.Instance.LogSession(
				ElecularSettings.Instance.ProjectId, 
				session,
				id =>
				{
					sessionId = id;
				},
				() =>
				{
					Debug.LogError("Error while logging user session. Please check if project id is valid.");
				}
			);
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
			if (userAction == null)
			{
				Debug.LogError("User action cannot be null");
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

