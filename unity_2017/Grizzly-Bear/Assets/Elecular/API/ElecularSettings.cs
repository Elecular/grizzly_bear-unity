using System;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// This holds all the settings needed for integrating with Elecular
	/// </summary>
	public class ElecularSettings : ScriptableObject
	{
		public const string RESOURCE_PATH = @"Elecular/Settings";
		
		/// <summary>
		/// If the player is inactive for this amount of time (seconds), Elecular will start a new user session
		/// </summary>
		public const int SESSION_INACTIVE_TIME_THRESHOLD = 15 * 60;
		
		[SerializeField]
		private string projectId;

		[NonSerialized]
		private static ElecularSettings instance;

		/// <summary>
		/// Gets the Singleton instance of Elecular Settings
		/// </summary>
		public static ElecularSettings Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<ElecularSettings>(RESOURCE_PATH);
				}
				return instance;
			}
		}

		/// <summary>
		/// Project Id of Elecular
		/// </summary>
		public string ProjectId
		{
			get
			{
				return projectId;
			}
		}
	}	
}
