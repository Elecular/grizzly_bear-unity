using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Elecular.Core
{
	/// <summary>
	/// This is a wrapper over UnityWebRequest.
	/// This wrapper provides better fault tolerance by retrying requests on failure
	/// and provides and easy interface
	/// </summary>
	public class Request
	{
		private const int NUMBER_OF_RETRIES = 3;

		private UnityWebRequest[] unityWebRequests;

		private static RequestCoroutineManager coroutineManager;
		
		/// <summary>
		/// Makes a new Request entity
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static Request Get(string uri)
		{
			var requests = new UnityWebRequest[NUMBER_OF_RETRIES];
			for (var count = 0; count < NUMBER_OF_RETRIES; count++)
			{
				requests[count] = UnityWebRequest.Get(uri);
			}
			return new Request(requests);
		}

		private Request(UnityWebRequest[] unityWebRequests)
		{
			if (coroutineManager == null)
			{
				coroutineManager = new GameObject("Elecular - Request Coroutine Manager")
					.AddComponent<RequestCoroutineManager>();
			}
			this.unityWebRequests = unityWebRequests;
		}
		
		/// <summary>
		/// Sends a request and returns the response in given type
		/// </summary>
		/// <param name="onResponse">Response callback</param>
		/// <param name="onError">Error callback</param>
		/// <typeparam name="T">Type of response. The response is parsed into the give type. It must be a simple serializable class</typeparam>
		public void Send<T>(UnityAction<T> onResponse, UnityAction onError=null)
		{
			coroutineManager.StartCoroutine(StartProcessingRequest(res =>
			{
				onResponse(JsonUtility.FromJson<T>(res));
			}, onError));
		}
		
		private IEnumerator StartProcessingRequest(UnityAction<string> onResponse, UnityAction onError)
		{
			var success = false;
			foreach (var unityWebRequest in unityWebRequests)
			{
				using (unityWebRequest)
				{
					yield return unityWebRequest.SendWebRequest();
					if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
					{
						Debug.LogError(!unityWebRequest.downloadHandler.text.Equals("")
							? unityWebRequest.downloadHandler.text 
							: unityWebRequest.error
						);
						continue;
					}
					
					onResponse(unityWebRequest.downloadHandler.text);
					success = true;
					break;
				}	
			}
			if (!success && onError != null) onError();
		}
	}	
}
