﻿using System.Collections;
using System.Text;
using Elecular.API;
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

		protected UnityWebRequest[] unityWebRequests;

		private static RequestCoroutineManager coroutineManager;

		private static int mockRequestIndex;
		
		private static Request[] mockRequest; //Used for testing

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
			return ConstructRequest(requests);
		}

		/// <summary>
		/// Makes a new Request entity
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="json"></param>
		/// <returns></returns>
		public static Request Post(string uri, string json) 
		{
			var requests = new UnityWebRequest[NUMBER_OF_RETRIES];
			for (var count = 0; count < NUMBER_OF_RETRIES; count++)
			{
				requests[count] = UnityWebRequest.Post(uri, "POST");
				requests[count].uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
				requests[count].SetRequestHeader("Content-Type", "application/json");
				requests[count].SetRequestHeader("Accept", "*/*");
			}
			return ConstructRequest(requests);
		}

		private static Request ConstructRequest(UnityWebRequest[] requests)
		{
			#if UNITY_EDITOR
			if (mockRequest != null && mockRequest[mockRequestIndex] != null) //Used for testing	
			{
				mockRequest[mockRequestIndex].unityWebRequests = requests;
				var req = mockRequest[mockRequestIndex];
				mockRequestIndex = (mockRequestIndex + 1) % mockRequest.Length;
				return req;
			}
			if (!Application.isPlaying)
			{
				return new EditorRequest(requests); 
			}
			#endif
			return new Request(requests);
		}

		protected Request() {}
		
		protected Request(UnityWebRequest[] unityWebRequests)
		{
			if (coroutineManager == null) 
			{
				coroutineManager = new GameObject("Elecular - Request Coroutine Manager")
					.AddComponent<RequestCoroutineManager>();
				GameObject.DontDestroyOnLoad(coroutineManager.gameObject);
			}
			this.unityWebRequests = unityWebRequests;
		}
		
		/// <summary>
		/// Sends a request and returns the response in given type
		/// </summary>
		/// <param name="onResponse">Response callback</param>
		/// <param name="onError">Error callback</param>
		/// <typeparam name="T">Type of response. The response is parsed into the give type. It must be a simple serializable class</typeparam>
		public virtual void Send<T>(UnityAction<T> onResponse, UnityAction onError)
		{
			coroutineManager.StartCoroutine(StartProcessingRequest(res =>
			{
				onResponse(JsonUtility.FromJson<T>(res));
			}, onError));
		}
		
		/// <summary>
		/// This method is used for testing.
		/// Its sets a mock request that is returned when Get or Post is called
		/// </summary>
		/// <param name="request"></param>
		public static void SetMockRequest(Request request)
		{
			mockRequest = new []{request};
			mockRequestIndex = 0;
		}
		
		/// <summary>
		/// This method is used for testing.
		/// Its sets a mock request that is returned when Get or Post is called
		/// </summary>
		/// <param name="request"></param>
		public static void SetMultipleMockRequest(Request[] request)
		{
			mockRequest = request;
			mockRequestIndex = 0;
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
						var error = unityWebRequest.error;
						if (!unityWebRequest.downloadHandler.text.Equals(""))
						{
							error = unityWebRequest.downloadHandler.text;
						}
						
						if (unityWebRequest.isHttpError && unityWebRequest.responseCode < 500)
						{
							Debug.LogError(string.Format("The following error occured when Elecular was trying to make an API request:\n{0}", error));
							break;
						}
						Debug.Log(string.Format("The following error occured. Please check if you are online:\n{0}", error));
						continue;
					}

					var res = unityWebRequest.downloadHandler.text;
					if (res.Length > 0 && res[0] == '[' && res[res.Length - 1] == ']')
					{
						res = "{ \"array\": " + res + "}";
					}
					onResponse(res);
					success = true;
					break;
				}	
			}
			if (!success && onError != null) onError();
			yield return null;
		}
	}	
}
