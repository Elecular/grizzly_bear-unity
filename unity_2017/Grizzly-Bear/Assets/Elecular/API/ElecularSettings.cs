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
