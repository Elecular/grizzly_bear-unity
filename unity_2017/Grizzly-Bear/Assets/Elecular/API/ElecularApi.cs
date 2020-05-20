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
		private ElecularSettings _settings;

		private static ElecularApi instance;
		
		private ElecularApi() {}
		
		/// <summary>
		/// Gets the Singleton instance of Elecular API
		/// </summary>
		public static ElecularApi Instance
		{
			get { return instance ?? (instance = new ElecularApi()); }
		}

		/// <summary>
		/// Gets the variation that is assigned to the user
		/// By default, the username is set to the device id. 
		/// </summary>
		/// <param name="experimentName">Name of the experiment</param>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		/// <param name="username">By default device id is used (Optional)</param>
		/// <returns>The variation assigned to this user</returns>
		public void GetVariation(
			string experimentName, 
			UnityAction<Variation> onResponse, 
			UnityAction onError=null, 
			string username=null
		)
		{
			if (username == null)
			{
				username = SystemInfo.deviceUniqueIdentifier;
			}
			ExperimentsApi.Instance.GetVariation(username, Settings.ProjectId, experimentName, onResponse, onError);
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
				Settings.ProjectId, 
				experimentName, 
				onResponse, 
				onError
			);
		}
		
		/// <summary>
		/// Gets the setting's value that is assigned to the user 
		/// </summary>
		/// <param name="experimentName">Name of the experiment</param>
		/// <param name="settingName">Name of the Setting</param>
		/// <param name="onResponse">Callback that is triggered when the setting value is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		/// <param name="username">By default device id is used (Optional)</param>
		public void GetSetting(
			string experimentName, 
			string settingName, 
			UnityAction<string> onResponse,
			UnityAction onError = null, 
			string username = null
		)
		{
			GetVariation(experimentName, variation =>
			{
				var setting  = variation.Settings.FirstOrDefault(
					s => s.Name.Equals(settingName)
				);
				if (setting == null)
				{
					Debug.LogError("Setting not found under given experiment");
					if (onError != null) onError();
					return;
				}
				onResponse(setting.Value);
			}, onError, username);
		}

		private ElecularSettings Settings
		{
			get
			{
				if (_settings != null) return _settings;
				_settings = Resources.Load<ElecularSettings>("Elecular/Settings");
				return _settings;
			}
		}
	}	
}

