#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// This holds all the settings needed for integrating with Elecular
	/// </summary>
	public class ElecularSettings : ScriptableObject
	{
		private const string resourcePath = @"Elecular/Settings";
		
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
					instance = Resources.Load<ElecularSettings>(resourcePath);
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
		
		#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static void InitializeOnLoad()
		{
			if (Resources.Load<ElecularSettings>(resourcePath) == null)
			{
				if(!AssetDatabase.IsValidFolder("Assets/Resources"))
					AssetDatabase.CreateFolder("Assets", "Resources");
				if(!AssetDatabase.IsValidFolder("Assets/Resources/Elecular"))
					AssetDatabase.CreateFolder("Assets/Resources", "Elecular");
				AssetDatabase.CreateAsset(
					CreateInstance<ElecularSettings>(), 
					"Assets/Resources/Elecular/Settings.asset"
				);
			}
			EditorApplication.playModeStateChanged += InitializeSettings; 
		}

		private static void InitializeSettings(PlayModeStateChange state)
		{
			if (state != PlayModeStateChange.EnteredPlayMode) return;
			
			var settings = Resources.Load<ElecularSettings>(resourcePath);
			if (settings.projectId != null && !settings.projectId.Equals("")) return;
			
			EditorApplication.isPlaying = false;
			if (EditorUtility.DisplayDialog(
				"Please enter Elecular Project ID",
				"Please set your project id in the Settings file under '" 
				+ AssetDatabase.GetAssetPath(settings) 
				+ "'.\n\nYou can get your project id by logging into https://app.elecular.com",
				"ok"
			))
			{
				EditorGUIUtility.PingObject(settings);	
			}
		}
		#endif
	}	
}
