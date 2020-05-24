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
		private SessionNotifier sessionNotifier;
		
		private static ElecularApi instance;

		private UserData userData = new UserData();

		private string environment;
		
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
		
		private void OnNewSession(string sessionId)
		{
			var session = new UserActivityApi.Session(
				SystemInfo.deviceUniqueIdentifier,
				userData.GetAllSegments(),
				GetEnvironment()
			);
			UserActivityApi.Instance.LogSession(
				ElecularSettings.Instance.ProjectId, 
				session, 
				() => {},
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
				try
				{
					onResponse(
						variations.FirstOrDefault(variation => variation.Name.Equals(variationName))
					);
				}
				catch (Exception e)
				{
					if(onError!=null) onError();
				}
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
		/// Gets the Singleton instance of Elecular API
		/// </summary>
		public static ElecularApi Instance
		{
			get { return instance ?? (instance = new ElecularApi()); }
		}

		private string GetEnvironment()
		{
			if (environment != null)
			{
				return environment;
			}
			return (Application.isEditor ? Environment.Dev : Environment.Prod).GetString();
		}
	}	
}

