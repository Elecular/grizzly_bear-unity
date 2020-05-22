﻿using Elecular.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Elecular.API
{
	/// <summary>
	/// This ScriptableObject is used for storing Experiments Data.
	/// This can be used to get variations data, settings data etc. 
	/// </summary>
	[CreateAssetMenu(menuName = "Elecular/Experiment")]
	public class Experiment : ScriptableObject
	{
		[SerializeField]
		private string experimentName;

		/// <summary>
		/// Gets the variation that is assigned to the user
		/// By default, the device id is used as the username
		/// </summary>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		/// <param name="username">By default device id is used (Optional)</param>
		/// <returns>The variation assigned to this user</returns>
		public void GetVariation(
			UnityAction<Variation> onResponse, 
			UnityAction onError=null, 
			string username=null
		)
		{
			#if UNITY_EDITOR
			var forcedVariation = GetForcedVariation();
			if (forcedVariation != null && !forcedVariation.Equals(""))
			{
				ElecularApi.Instance.GetVariation(experimentName, forcedVariation, onResponse, onError);
				return;
			}
			#endif
			ElecularApi.Instance.GetVariation(experimentName, onResponse, onError, username);
		}
		
		/// <summary>
		/// Gets the setting's value that is assigned to the user
		/// </summary>
		/// <param name="settingName">Name of the Setting</param>
		/// <param name="onResponse">Callback that is triggered when the setting value is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		/// <param name="username">By default device id is used (Optional)</param>
		public void GetSetting(
			string settingName, 
			UnityAction<string> onResponse,
			UnityAction onError = null, 
			string username = null
		)
		{
			ElecularApi.Instance.GetSetting(experimentName, settingName, onResponse, onError, username);
		}
		
		/// <summary>
		/// Gets all the variations under this experiment
		/// WARNING: This function is expensive and is only meant to be used in editor
		/// </summary>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		public void GetAllVariations(
			UnityAction<Variation[]> onResponse,
			UnityAction onError = null
		)
		{
			ElecularApi.Instance.GetAllVariations(experimentName, onResponse, onError);
		}
		
		/// <summary>
		/// Name of the experiment
		/// </summary>
		public string ExperimentName
		{
			get { return experimentName; }
		}
		
		#if UNITY_EDITOR
		
		/// <summary>
		/// If the developer wants to see how his/her mobile game looks like in a certain variation,
		/// we can force set the variation.
		/// </summary>
		/// <returns></returns>
		private string GetForcedVariation()
		{
			var guid = UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(this));
			return UnityEditor.EditorPrefs.GetString(string.Format("elecular-{0}-selected-variation", guid));
		}
		
		#endif
	}	
}
