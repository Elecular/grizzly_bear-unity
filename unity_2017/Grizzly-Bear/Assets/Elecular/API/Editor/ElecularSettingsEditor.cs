using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	public static class ElecularSettingsEditor
	{
		[InitializeOnLoadMethod]
		private static void InitializeOnLoad()
		{
			if (Resources.Load<ElecularSettings>(ElecularSettings.RESOURCE_PATH) == null)
			{
				if(!AssetDatabase.IsValidFolder("Assets/Resources"))
					AssetDatabase.CreateFolder("Assets", "Resources");
				if(!AssetDatabase.IsValidFolder("Assets/Resources/Elecular"))
					AssetDatabase.CreateFolder("Assets/Resources", "Elecular");
				AssetDatabase.CreateAsset(
					ScriptableObject.CreateInstance<ElecularSettings>(), 
					"Assets/Resources/Elecular/Settings.asset"
				);
			}
			EditorApplication.playModeStateChanged += InitializeSettings; 
		}

		private static void InitializeSettings(PlayModeStateChange state)
		{
			if (state != PlayModeStateChange.EnteredPlayMode) return;
			
			var settings = Resources.Load<ElecularSettings>(ElecularSettings.RESOURCE_PATH);
			if (settings.ProjectId != null && !settings.ProjectId.Equals("")) return;
			
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
	}
}
