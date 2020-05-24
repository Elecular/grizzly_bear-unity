using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elecular.Core
{
	/// <summary>
	/// This class is used for logging user activity to the User-Activity service
	/// </summary>
	public class UserActivityApi
	{
		private const string URI = @"https://user-activity.api.elecular.com";
		
		private static UserActivityApi instance;

		private UserActivityApi() {}
		
		/// <summary>
		/// Gets the Singleton Instance of Experiments API
		/// </summary>
		public static UserActivityApi Instance
		{
			get { return instance ?? (instance = new UserActivityApi()); }
		}

		/// <summary>
		/// Logs a user session. 
		/// </summary>
		/// <param name="projectId"></param>
		/// <param name="session"></param>
		/// <param name="success"></param>
		/// <param name="error"></param>
		public void LogSession(
			string projectId, 
			Session session,
			UnityAction success, 
			UnityAction error
		)
		{
			var uri = string.Format("{0}/projects/{1}/user-session", URI, projectId);
			Request.Post(uri, JsonUtility.ToJson(session)).Send<object>(res =>
			{
				success();
			}, error);
		}
		
		/// <summary>
		/// Logs a user activity
		/// </summary>
		/// <param name="projectId"></param>
		/// <param name="activity"></param>
		/// <param name="success"></param>
		/// <param name="error"></param>
		public void LogActivity(
			string projectId,
			Activity activity,
			UnityAction success,
			UnityAction error
		)
		{
			var uri = string.Format("{0}/projects/{1}/user-activity", URI, projectId);
			Request.Post(uri, JsonUtility.ToJson(activity)).Send<object>(res =>
			{
				success();
			}, error);
		}
		
		/// <summary>
		/// This defines a user session
		/// </summary>
		[Serializable]
		public class Session
		{
			[SerializeField] 
			private string userId;

			[SerializeField] 
			private string[] segments;

			[SerializeField] 
			private string environment;

			public Session(string userId, string[] segments, string environment)
			{
				this.userId = userId;
				this.segments = segments;
				this.environment = environment;
			}
		}
		
		/// <summary>
		/// This defines a user session
		/// </summary>
		[Serializable]
		public class Activity
		{
			[SerializeField] 
			private string userId;

			[SerializeField] 
			private string sessionId;

			[SerializeField] 
			private string userAction;

			public Activity(string userId, string sessionId, string userAction)
			{
				this.userId = userId;
				this.sessionId = sessionId;
				this.userAction = userAction;
			}
		}
	}
}

