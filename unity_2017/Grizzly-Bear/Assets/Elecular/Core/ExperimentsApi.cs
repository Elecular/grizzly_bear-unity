using UnityEngine.Networking;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Elecular.Core
{
	/// <summary>
	/// This class is used for getting experiments data from the Experiments API.
	/// This API is NOT meant to be used directly by a Unity developer.
	/// Please use <see cref="Elecular.API.ElecularApi"/> instead
	/// </summary>
	public class ExperimentsApi
	{
		private const string URI = @"https://experiments.api.elecular.com";

		/// <summary>
		/// Gets the variation assigned to given user in the given experiment
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="projectId">Project Id</param>
		/// <param name="experimentName">Experiment Name</param>
		/// <param name="onResponse">Callback that is triggered when a variation is returned</param>
		/// <param name="onError">Callback that is triggered when there is an error</param>
		public void GetVariation(
			string username, 
			string projectId, 
			string experimentName,
			UnityAction<Variation> onResponse,
			UnityAction onError=null
		)
		{
			var variationUri = string.Format(
				"{0}/projects/{1}/experiments/{2}/variations?userId={3}",
				URI,
				projectId,
				experimentName,
				username
			);
			Request.Get(variationUri).Send(onResponse, onError);
		}
	}	
}

