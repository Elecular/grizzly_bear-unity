using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elecular.Core
{
	/// <summary>
	/// This class is used for getting experiments data from the Experiments API.
	/// </summary>
	public class ExperimentsApi
	{
		private const string URI = @"https://experiments.api.elecular.com";
		
		//This is a cache for storing variations that is assigned to each user
		private Dictionary<string, Variation> cache = new Dictionary<string, Variation>();
		
		//This is a cache for storing all variations of an experiment. This is usually only used in an editor
		private Dictionary<string, Variation[]> cachedExperiments = new Dictionary<string, Variation[]>();

		private static ExperimentsApi instance;

		private ExperimentsApi() {}
		
		/// <summary>
		/// Gets the Singleton Instance of Experiments API
		/// </summary>
		public static ExperimentsApi Instance
		{
			get { return instance ?? (instance = new ExperimentsApi()); }
		}
		
		/// <summary>
		/// Gets the variation that is assigned to the user
		/// </summary>
		public void GetVariation(
			string username, 
			string projectId, 
			string experimentName,
			UnityAction<Variation> onResponse,
			UnityAction onError
		)
		{
			var cacheKey =  projectId + experimentName + username;
			if (cache.ContainsKey(cacheKey))
			{
				onResponse(cache[cacheKey]);
				return;
			}
			
			var variationUri = string.Format(
				"{0}/projects/{1}/experiments/{2}/variations?userId={3}",
				URI,
				projectId,
				experimentName,
				username
			);
			Request.Get(variationUri).Send<Variation>(variation =>
			{
				cache.Remove(cacheKey);
				cache.Add(
					cacheKey,
					variation
				);
				onResponse(variation);
			}, onError);
		}
		
		/// <summary>
		/// When GetVariation is called, the variation is cached. This avoids making API calls to get the variation
		/// This method clears the cache, forcing an API call to get the variation
		/// </summary>
		public void ClearCache()
		{
			cache.Clear();
		}
		
		/// <summary>
		/// Gets all the variations of this experiment
		/// WARNING: This function is expensive and is only intended to be used in editor. 
		/// </summary>
		public void GetAllVariations(
			string projectId,
			string experimentName,
			UnityAction<Variation[]> onResponse,
			UnityAction onError
		)
		{
			if (experimentName != null && !experimentName.Equals("") && cachedExperiments.ContainsKey(experimentName))
			{
				onResponse(cachedExperiments[experimentName]);
				return;
			}
			var experimentUri = string.Format(
				"{0}/projects/{1}/experiments/{2}",
				URI,
				projectId,
				experimentName
			);
			Request.Get(experimentUri).Send<Experiments>(experiments =>
			{
				if (experiments == null || experiments.array == null || experiments.array.Length == 0)
				{
					if(onError != null) onError();
					return;
				} 

				if (!cachedExperiments.ContainsKey(experimentName))
				{
					cachedExperiments.Add(experimentName, experiments.array[0].variations);	
				}
				onResponse(experiments.array[0].variations);
			}, onError);
		}
		
		/// <summary>
		/// Used for parsing experiment from JSON to class in GetAllVariations method.
		/// This class is not used for anything else other than parsing JSON response 
		/// </summary>
		[Serializable]
		private class Experiments
		{
			public Experiment[] array = null;
		}
		
		/// <summary>
		/// Used for parsing experiment from JSON to class
		/// This class is not used for anything else other than parsing JSON response 
		/// </summary>
		[Serializable]
		private class Experiment
		{
			public Variation[] variations = null;
		}
	}	
}

